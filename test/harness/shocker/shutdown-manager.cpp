/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * shutdown-manager.cpp: When shutdown is signaled, the shutdown manager
 * 			 will wait until everything (image capture) is done
 *			 before it will actually shutdown the app.
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2007-2010 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#include <glib.h>

#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>

#include "debug.h"
#include "shutdown-manager.h"
#include "input.h"
#include "shocker-plugin.h"
#include "harness.h"

static gint wait_count = 0;

static void execute_shutdown ();
static gboolean attempt_clean_shutdown (gpointer data);

void
shutdown_manager_wait_increment ()
{
	g_atomic_int_inc (&wait_count);
}

void
shutdown_manager_wait_decrement ()
{
	g_atomic_int_dec_and_test (&wait_count);
}

static gboolean
force_shutdown (gpointer data)
{
	if (getenv ("MOONLIGHT_SHOCKER_DONT_EXIT") == NULL) {
		printf ("[%i shocker] Could not shutdown nicely, exiting the process. Set MOONLIGHT_SHOCKER_DONT_EXIT to disable.\n", getpid ());
		exit (0);
	} else {
		printf ("[%i shocker] Could not shutdown nicely, but won't exit process since MOONLIGHT_SHOCKER_DONT_EXIT is set.\n", getpid ());
	}
	return false;
}

static gboolean
send_alt_f4 (gpointer dummy)
{
	return false;
	printf ("[%i shocker] sending Alt+F4 to browser...\n", getpid ());
	InputProvider *input = InputProvider::GetInstance ();
	// send alt-f4
	input->SendKeyInput (VK_MENU, true, false, false);
	input->SendKeyInput (VK_F4, true, false, false);
	input->SendKeyInput (VK_F4, false, false, false);
	input->SendKeyInput (VK_MENU, false, false, false);
	return false;
}

static gboolean
send_ctrl_w (gpointer dummy)
{
	printf ("[%i shocker] sending Ctrl-W to browser...\n", getpid ());
	// This doesn't work with oob - there is no menu so nothing handles ctrl-q
	InputProvider *input = InputProvider::GetInstance ();
	// send ctrl-q
	input->SendKeyInput (VK_CONTROL, true, false, false);
	input->SendKeyInput (VK_W, true, false, false);
	input->SendKeyInput (VK_W, false, false, false);
	input->SendKeyInput (VK_CONTROL, false, false, false);
	return false;
}

static int xlib_shutdown_error_handler (Display *d, XErrorEvent *error_event)
{
	char error_msg [1024];

	printf ("[%i shocker] xlib error while trying to shutdown the browser\n", getpid ());
	printf ("[%i shocker] type: %i serial: %lu error_code: %d request_code: %d minor_code: %i resourceid: %i\n",
		getpid (), error_event->type, error_event->serial, error_event->error_code, error_event->request_code, error_event->minor_code, (int) error_event->resourceid);

	XGetErrorText (d, error_event->error_code, error_msg, sizeof (error_msg));
	printf ("[%i shocker] %s\n", getpid (), error_msg);

	return 0;
}

static gboolean
send_wm_delete (Window window)
{
	printf ("[%i shocker] sending WM_DELETE_WINDOW event to %p\n", getpid (), (void*) window);

	Display *display = XOpenDisplay (NULL);
	Atom WM_PROTOCOLS = XInternAtom (display, "WM_PROTOCOLS", False);
	Atom WM_DELETE_WINDOW = XInternAtom (display, "WM_DELETE_WINDOW", False);
	XClientMessageEvent ev;
	int (*previous_handler)(Display *, XErrorEvent *);

	XSynchronize (display, true);
	previous_handler = XSetErrorHandler (xlib_shutdown_error_handler);

	ev.type = ClientMessage;
	ev.window = window;
	ev.message_type = WM_PROTOCOLS;
	ev.format = 32;
	ev.data.l [0] = WM_DELETE_WINDOW;
	ev.data.l [1] = 0;

	XSendEvent (display, ev.window, False, 0, (XEvent*) &ev);
	XCloseDisplay (display);

	XSetErrorHandler (previous_handler);

	return false;
}

static void
execute_shutdown ()
{
	LOG_SHUTDOWN ("[%i shocker] execute_shutdown\n", getpid ())

	char *dont_die = getenv ("MOONLIGHT_SHOCKER_DONT_DIE");
	if (dont_die != NULL && dont_die [0] != 0)
		return;

	g_type_init ();

	if (PluginObject::browser_app_context != 0) {
		printf ("[%i shocker] shutting down browser...\n", getpid ());

		g_timeout_add (50, (GSourceFunc) send_wm_delete, (void *) PluginObject::browser_app_context);
		PluginObject::browser_app_context = 0;
	} else {
		WindowInfoEx ex;
		int backup_timeout = 5000;
		int pid_tree [16];
		int count;
		Harness::GetProcessTree (getpid (), pid_tree, sizeof (pid_tree) / sizeof (int), &count);
		bool wm_delete_sent = false;
		for (int i = count - 1; i >= 0; i--) {
			if (pid_tree [i] == 0)
				continue;

			memset (&ex, 0, sizeof (ex));
			if (!WindowHelper_GetWindowInfoInternal (pid_tree [i], (WindowInfo *) &ex, true) == 0 && ex.window != 0) {
				continue;
			}

			int timeout = (count - i - 1) * 1000 + 50;
			g_timeout_add (timeout, (GSourceFunc) send_wm_delete, (void *) ex.window);
			wm_delete_sent = true;
		}

		if (!wm_delete_sent)
			backup_timeout = 10; // no need for the backup to wait if there is nothing else we can do

		// have a backup
		// note that this is racy: the current process might not be the one with the focus.
		g_timeout_add (backup_timeout, send_ctrl_w, NULL); // this doesn't work for oob apps
		g_timeout_add (backup_timeout, send_alt_f4, NULL); // this doesn't work if there is no window manager (inside xvfb while running tests for instance)
	}
	
	// Have a backup in case the above fails.
	g_timeout_add (25000, force_shutdown, NULL);
}

static gboolean
attempt_clean_shutdown (gpointer data)
{
	LOG_SHUTDOWN ("[%i shocker] attempt_clean_shutdown\n", getpid ())

	bool ready_for_shutdown = false;

	ready_for_shutdown = g_atomic_int_get (&wait_count) <= 0;

	if (ready_for_shutdown) {
		execute_shutdown ();
		return FALSE;
	}

	return TRUE;
}

void
shutdown_manager_queue_shutdown ()
{
	LOG_SHUTDOWN ("[%i shocker] shutdown_manager_queue_shutdown\n", getpid ())

	if (g_atomic_int_get (&wait_count) == 0)
		return execute_shutdown ();

	printf ("[%i shocker] Unable to execute shutdown immediately (pending screenshots), attempting a clean shutdown.\n", getpid ());
	
	if (!g_timeout_add (100, attempt_clean_shutdown, NULL)) {
		printf ("[%i shocker] Unable to create timeout for queued shutdown, executing immediate shutdown.\n", getpid ());
		execute_shutdown ();
	}
}

void
SignalShutdown (const char *window_name)
{
	LOG_SHUTDOWN ("[%i shocker] SignalShutdown\n", getpid ())
	shutdown_manager_queue_shutdown ();
}

void
TestHost_SignalShutdown (const char *window_name)
{
	LOG_SHUTDOWN ("[%i shocker] TestHost_SignalShutdown\n", getpid ())
	SignalShutdown (window_name);
}
