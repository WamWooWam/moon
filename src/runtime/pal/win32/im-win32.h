/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * im-win32.h
 *
 * Copyright 2010 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#ifndef MOON_IM_WIN32_H
#define MOON_IM_WIN32_H

#include "pal.h"
#include "window.h"

namespace Moonlight {

class MoonIMContextWin32  : public MoonIMContext {
public:
	MoonIMContextWin32 ();
	virtual ~MoonIMContextWin32 ();

	virtual void SetUsePreedit (bool flag);
	virtual void SetClientWindow (MoonWindow*  window);
	virtual void SetSurroundingText (const char *text, int offset, int length);
	virtual void Reset ();

	virtual void FocusIn ();
	virtual void FocusOut ();

	virtual void SetCursorLocation (Rect r);

	virtual bool FilterKeyPress (MoonKeyEvent* event);

	virtual void SetRetrieveSurroundingCallback (MoonCallback cb, gpointer data);
	virtual void SetDeleteSurroundingCallback (MoonCallback cb, gpointer data);
	virtual void SetCommitCallback (MoonCallback cb, gpointer data);

	virtual gpointer GetPlatformIMContext ();

 private:
};

};
#endif /* MOON_IM_WIN32_H */
