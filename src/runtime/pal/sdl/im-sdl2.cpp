/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * im-sdl2.cpp
 *
 * Copyright 2010 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#include <config.h>
#include "im-sdl2.h"

using namespace Moonlight;

MoonIMContextSDL2::MoonIMContextSDL2 ()
{
}

MoonIMContextSDL2::~MoonIMContextSDL2 ()
{
}

void
MoonIMContextSDL2::SetUsePreedit (bool flag)
{
}

void
MoonIMContextSDL2::SetClientWindow (MoonWindow* window)
{
}

bool
MoonIMContextSDL2::FilterKeyPress (MoonKeyEvent* event)
{
	return false;
}

void
MoonIMContextSDL2::SetSurroundingText (const char *text, int offset, int length)
{
}

void
MoonIMContextSDL2::Reset ()
{
}


void
MoonIMContextSDL2::FocusIn ()
{
}

void
MoonIMContextSDL2::FocusOut ()
{
}

void
MoonIMContextSDL2::SetCursorLocation (Rect r)
{
}

void
MoonIMContextSDL2::SetRetrieveSurroundingCallback (MoonCallback cb, gpointer data)
{
}

void
MoonIMContextSDL2::SetDeleteSurroundingCallback (MoonCallback cb, gpointer data)
{
}

void
MoonIMContextSDL2::SetCommitCallback (MoonCallback cb, gpointer data)
{
}

gpointer
MoonIMContextSDL2::GetPlatformIMContext ()
{
	return NULL;
}

