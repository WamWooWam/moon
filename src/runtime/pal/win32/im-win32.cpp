/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * im-win32.cpp
 *
 * Copyright 2010 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#include <config.h>
#include "im-win32.h"

using namespace Moonlight;

MoonIMContextWin32::MoonIMContextWin32 ()
{
}

MoonIMContextWin32::~MoonIMContextWin32 ()
{
}

void
MoonIMContextWin32::SetUsePreedit (bool flag)
{
}

void
MoonIMContextWin32::SetClientWindow (MoonWindow* window)
{
}

bool
MoonIMContextWin32::FilterKeyPress (MoonKeyEvent* event)
{
	return false;
}

void
MoonIMContextWin32::SetSurroundingText (const char *text, int offset, int length)
{
}

void
MoonIMContextWin32::Reset ()
{
}


void
MoonIMContextWin32::FocusIn ()
{
}

void
MoonIMContextWin32::FocusOut ()
{
}

void
MoonIMContextWin32::SetCursorLocation (Rect r)
{
}

void
MoonIMContextWin32::SetRetrieveSurroundingCallback (MoonCallback cb, gpointer data)
{
}

void
MoonIMContextWin32::SetDeleteSurroundingCallback (MoonCallback cb, gpointer data)
{
}

void
MoonIMContextWin32::SetCommitCallback (MoonCallback cb, gpointer data)
{
}

gpointer
MoonIMContextWin32::GetPlatformIMContext ()
{
	return NULL;
}

