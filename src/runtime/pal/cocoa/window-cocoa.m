/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * window-cocoa.cpp: MoonWindow implementation using cocoa widgets.
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2007-2008 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

#include "config.h"

#include <glib.h>

#include "window-cocoa.h"
#include "clipboard-cocoa.h"
#include "config-dialog-cocoa.h"
#include "pixbuf-cocoa.h"
#include "deployment.h"
#include "timemanager.h"
#include "enums.h"
#include "context-cairo.h"

#include "MLEvent.h"
#include "MLView.h"
#include "MLWindow.h"
#include <AppKit/AppKit.h>

#include <cairo.h>
#include "cairo-quartz.h"

#ifdef USE_CGL
#include "context-cgl.h"
#endif

using namespace Moonlight;

MoonWindowCocoa::MoonWindowCocoa (MoonWindowType windowType, int w, int h, MoonWindow *parent, Surface *surface)
	: MoonWindow (windowType, w, h, parent, surface)
{
	MLWindow *window;
	MLView *view;

	native = NULL;
	ctx = NULL;

	window = [[MLWindow alloc] initWithContentRect: NSMakeRect (0, 0, w, h) styleMask: NSTitledWindowMask | NSClosableWindowMask | NSMiniaturizableWindowMask | NSResizableWindowMask backing: NSBackingStoreBuffered defer: YES];
	view = [[[MLView alloc] initWithFrame: NSMakeRect (0, 0, w, h)] autorelease];

	window.moonwindow = this;
	view.moonwindow = this;

	window.acceptsMouseMovedEvents = YES;
	window.contentView = view;

	[(MLView *)view setAutoresizingMask: NSViewWidthSizable | NSViewHeightSizable];

	this->width = w;
	this->height = h;
	this->window = window;
	this->view = view;

#ifdef USE_CGL
	cgltarget = NULL;
	cglctx = NULL;
#endif
}

MoonWindowCocoa::~MoonWindowCocoa ()
{
#ifdef USE_CGL
	if (cglctx)
		delete cglctx;
	if (cgltarget)
		cgltarget->unref ();
#endif
}

void
MoonWindowCocoa::ConnectToContainerPlatformWindow (gpointer container_window)
{
}

MoonClipboard*
MoonWindowCocoa::GetClipboard (MoonClipboardType clipboardType)
{
	if (clipboardType == MoonClipboard_Primary)
		return NULL; // OSX doesn't have a primary clipboard

	return new MoonClipboardCocoa (this, clipboardType);
}

gpointer
MoonWindowCocoa::GetPlatformWindow ()
{
	return this->view;
}

void
MoonWindowCocoa::Resize (int width, int height)
{
	NSRect frame = [(MLWindow *)window frame];

	frame.size.width = width;
	frame.size.height = height;

	[(MLWindow *)window setFrame: frame display: YES];
}

void
MoonWindowCocoa::ResizeInternal (int width, int height)
{
	this->width = width;
	this->height = height;
}

void
MoonWindowCocoa::SetBackgroundColor (Color *color)
{
}

void
MoonWindowCocoa::SetCursor (CursorType cursor)
{
}

void
MoonWindowCocoa::Invalidate (Rect r)
{
	/*
	 * For some reason the render layer is delivering invalidations with bogus bounds
	 * This wreaks havoc with Cgl.  To do proper partial invalidations we need to 
	 * correct the bounds before passing them to Cgl
	 *
	 * This code is currently buggy with the framerate counter, and as such is disable
	 */

	/*
	r = r.Intersection (Rect (0, 0, GetWidth(), GetHeight())).RoundOut ();

	[(MLView *)view setNeedsDisplayInRect: NSMakeRect ((float) r.x, (float) r.y, (float) r.width, (float) r.height)];
	*/
	[(MLView *)view setNeedsDisplayInRect: [(MLView *)view frame]];
}

void
MoonWindowCocoa::ProcessUpdates ()
{
}

gboolean
MoonWindowCocoa::HandleEvent (gpointer platformEvent)
{
	// nothing to do here, since we don't pump events into the cocoa
	// window, cocoa calls our signal handlers directly.
	return TRUE;
}

void
MoonWindowCocoa::Show ()
{
	[(MLWindow *)window makeKeyAndOrderFront:nil];

}

void
MoonWindowCocoa::Hide ()
{
}

void
MoonWindowCocoa::EnableEvents (bool first)
{
	g_warning ("implement me: EnableEvents");
}

void
MoonWindowCocoa::DisableEvents ()
{
	g_warning ("implement me: DisableEvents");
}

void
MoonWindowCocoa::GrabFocus ()
{
	[(MLWindow *)window makeKeyWindow];
}

bool
MoonWindowCocoa::HasFocus ()
{
	return [(MLWindow *)window isKeyWindow];
}

void
MoonWindowCocoa::SetLeft (double left)
{
	NSRect frame = [(MLWindow *)window frame];

	frame.origin.x = left;

	[(MLWindow *)window setFrame: frame display: YES];
}

double
MoonWindowCocoa::GetLeft ()
{
	NSRect frame = [(MLWindow *)window frame];

	return frame.origin.x;
}

void
MoonWindowCocoa::SetTop (double top)
{
	NSRect frame = [(MLWindow *)window frame];

	frame.origin.y = top;

	[(MLWindow *)window setFrame: frame display: YES];
}

double
MoonWindowCocoa::GetTop ()
{
	NSRect frame = [(MLWindow *)window frame];

	return frame.origin.y;
}

void
MoonWindowCocoa::SetWidth (double width)
{
	NSRect frame = [(MLWindow *)window frame];

	frame.size.width = width;

	[(MLWindow *)window setFrame: frame display: YES];

	this->width = width;
}

void
MoonWindowCocoa::SetHeight (double height)
{
	NSRect frame = [(MLWindow *)window frame];

	frame.size.height = height;

	[(MLWindow *)window setFrame: frame display: YES];

	this->height = height;
}

void
MoonWindowCocoa::SetTitle (const char *title)
{
	if (title == NULL)
		title = "";

	NSString *titleNSString = [[NSString alloc] initWithUTF8String: title];

	[(MLWindow *)window setTitle: titleNSString];
}

void
MoonWindowCocoa::SetIconFromPixbuf (MoonPixbuf *pixbuf)
{
	g_assert_not_reached ();
}

void
MoonWindowCocoa::SetStyle (WindowStyle style)
{
	g_assert_not_reached ();
}

void
MoonWindowCocoa::ClearPlatformContext ()
{
#ifdef USE_CGL
	if (cglctx)
		delete cglctx;
	if (cgltarget)
		cgltarget->unref ();

	cglctx = NULL;
	cgltarget = NULL;
#endif
}


cairo_surface_t *
MoonWindowCocoa::CreateCairoSurface ()
{
	CGContextRef context = (CGContextRef) [[NSGraphicsContext currentContext] graphicsPort];
	int width = ((MLView *) view).frame.size.width;
	int height = ((MLView *) view).frame.size.height;

	CGContextTranslateCTM (context, 0.0, height);
	CGContextScaleCTM (context, 1.0, -1.0);

	return cairo_quartz_surface_create_for_cg_context (context, width, height);
}

void
MoonWindowCocoa::ButtonPressEvent (void *evt)
{
	MLEvent *event = [[MLEvent alloc] initWithEvent: (NSEvent *) evt view: (MLView *) this->view];
	MoonEventStatus status = MoonEventNotHandled;

	SetCurrentDeployment ();

	if (surface) {
		MoonButtonEvent *mevent = (MoonButtonEvent*)Runtime::GetWindowingSystem ()->CreateEventFromPlatformEvent (event);
		status = mevent->DispatchToWindow (this);
		delete mevent;
	}
}

void
MoonWindowCocoa::KeyDownEvent (void *evt)
{
	MLEvent *event = [[MLEvent alloc] initWithEvent: (NSEvent *) evt view: (MLView *) this->view];
	MoonEventStatus status = MoonEventNotHandled;

	SetCurrentDeployment ();

	if (surface) {
		MoonKeyEvent *mevent = (MoonKeyEvent*)Runtime::GetWindowingSystem ()->CreateEventFromPlatformEvent (event);
		status = mevent->DispatchToWindow (this);
		delete mevent;
	}
}

void
MoonWindowCocoa::KeyUpEvent (void *evt)
{
	MLEvent *event = [[MLEvent alloc] initWithEvent: (NSEvent *) evt view: (MLView *) this->view];
	MoonEventStatus status = MoonEventNotHandled;

	SetCurrentDeployment ();

	if (surface) {
		MoonKeyEvent *mevent = (MoonKeyEvent*)Runtime::GetWindowingSystem ()->CreateEventFromPlatformEvent (event);
		status = mevent->DispatchToWindow (this);
		delete mevent;
	}
}

void
MoonWindowCocoa::FlagsChangedEvent (void *evt)
{
	MLEvent *event = [[MLEvent alloc] initWithEvent: (NSEvent *) evt view: (MLView *) this->view];
	MoonEventStatus status = MoonEventNotHandled;

	SetCurrentDeployment ();

	if (surface) {
		MoonKeyEvent *mevent = (MoonKeyEvent*)Runtime::GetWindowingSystem ()->CreateEventFromPlatformEvent (event);
		status = mevent->DispatchToWindow (this);
		delete mevent;
	}
}

void
MoonWindowCocoa::ButtonReleaseEvent (void *evt)
{
	MLEvent *event = [[MLEvent alloc] initWithEvent: (NSEvent *) evt view: (MLView *) this->view];
	MoonEventStatus status = MoonEventNotHandled;

	SetCurrentDeployment ();

	if (surface) {
		MoonButtonEvent *mevent = (MoonButtonEvent*)Runtime::GetWindowingSystem ()->CreateEventFromPlatformEvent (event);
		status = mevent->DispatchToWindow (this);
		delete mevent;
	}
}

void
MoonWindowCocoa::MotionEvent (void *evt)
{
	MLEvent *event = [[MLEvent alloc] initWithEvent: (NSEvent *) evt view: (MLView *) this->view];
	MoonEventStatus status = MoonEventNotHandled;

	SetCurrentDeployment ();

	if (surface) {
		MoonMotionEvent *mevent = (MoonMotionEvent*) Runtime::GetWindowingSystem ()->CreateEventFromPlatformEvent (event);
		status = mevent->DispatchToWindow (this);
		delete mevent;
	}
}

void
MoonWindowCocoa::MouseEnteredEvent (void *evt)
{
	MLEvent *event = [[MLEvent alloc] initWithEvent: (NSEvent *) evt view: (MLView *) this->view];
	MoonEventStatus status = MoonEventNotHandled;

	SetCurrentDeployment ();

	if (surface) {
		MoonCrossingEvent *mevent = (MoonCrossingEvent*)Runtime::GetWindowingSystem ()->CreateEventFromPlatformEvent (event);
		status = mevent->DispatchToWindow (this);
		delete mevent;
	}
}

void
MoonWindowCocoa::MouseExitedEvent (void *evt)
{
	MLEvent *event = [[MLEvent alloc] initWithEvent: (NSEvent *) evt view: (MLView *) this->view];
	MoonEventStatus status = MoonEventNotHandled;

	SetCurrentDeployment ();

	if (surface) {
		MoonCrossingEvent *mevent = (MoonCrossingEvent*)Runtime::GetWindowingSystem ()->CreateEventFromPlatformEvent (event);
		status = mevent->DispatchToWindow (this);
		delete mevent;
	}
}

void
MoonWindowCocoa::ExposeEvent (Rect r)
{
	SetCurrentDeployment ();

	Region *region = new Region (r);

#if USE_CGL
	if (!cgltarget) {
		const NSOpenGLPixelFormatAttribute attr[] = {
			NSOpenGLPFAWindow,
			NSOpenGLPFADoubleBuffer,
			NSOpenGLPFAColorSize, 24,
			NSOpenGLPFAAlphaSize, 8,
			0
		};

		NSOpenGLContext *glcontext;
		NSOpenGLPixelFormat *format;
		CGLContext *context;
		MLView *mlview = (MLView *) view;

		format = [[[NSOpenGLPixelFormat alloc] initWithAttributes: attr] autorelease];
		glcontext = [[NSOpenGLContext alloc] initWithFormat: format shareContext: nil];

		[mlview setOpenGLContext: glcontext];

		cgltarget = new CGLSurface ((CGLContextObj) [glcontext CGLContextObj], width, height);
		context = new CGLContext (cgltarget);

		if (context->Initialize ()) {
			this->cglctx = context;
			this->nsoglcontext = glcontext;
		} else {
			delete context;
		}  
	}

	if (cgltarget && cglctx) {
		NSOpenGLContext *glcontext = (NSOpenGLContext *) nsoglcontext;
		Rect r0 = Rect (0, 0, width, height);
		Region *region = new Region (r); 
		int y = height - (r.y + r.height);

		CGLSetCurrentContext ((CGLContextObj) [glcontext CGLContextObj]);

		static_cast<OpenGLSurface *> (cgltarget)->Reshape (width, height);

		static_cast<Context *> (cglctx)->Push (Context::Clip (r0));
		surface->Paint (cglctx, region, GetTransparent (), true);
		static_cast<Context *> (cglctx)->Pop ();

		static_cast<OpenGLContext *> (cglctx)->Flush ();

		if (region->RectIn (r0) == CAIRO_REGION_OVERLAP_IN) {
			[glcontext flushBuffer];
		} else {
			glDrawBuffer (GL_FRONT);
			glViewport (-1, -1, 2, 2);
			glRasterPos2f (0, 0);
			glViewport (0, 0, width, height);

			glBitmap (0, 0, 0, 0, r.x, y, NULL);

			glCopyPixels (r.x, y, r.width, r.height, GL_COLOR);

			glDrawBuffer (GL_BACK);
			glFlush ();
		}

		return;
	}
#endif
				
	cairo_surface_t *native = CreateCairoSurface ();
	CairoSurface *target = new CairoSurface (width, height);
	Context *ctx = new CairoContext (target);

	surface->Paint (ctx, region, GetTransparent (), NO);

	cairo_t *cr = cairo_create (native);
	cairo_set_source_surface (cr, target->Cairo (), 0, 0);
	cairo_paint (cr);
	cairo_destroy (cr);

	cairo_surface_destroy (native);

	target->unref ();
	delete ctx;
	delete region;
}
