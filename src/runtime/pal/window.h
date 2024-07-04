/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * window.h:
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2007-2008 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */
 
#ifndef __MOON_WINDOW__
#define __MOON_WINDOW__

#include "pal.h"
#include "rect.h"
#include "enums.h"
#include "color.h"
#include "deployment.h"

namespace Moonlight {

class MoonEvent;
class MoonWindowingSystem;

/* @Namespace=System.Windows */
class MoonWindow {
public:
	// FIXME: do something with parentWindow here.
	MoonWindow (MoonWindowType windowType, int width = -1, int height = -1, MoonWindow *parentWindow = NULL, Surface *s = NULL, MoonWindowingSystem *windowingSystem = NULL)
	: width(width), height(height), surface(s), windowType (windowType), transparent(false), windowingSystem(windowingSystem) { }

	MoonWindow (int width = -1, int height = -1, PluginInstance *plugin = NULL, MoonWindowingSystem *windowingSystem = NULL)
	: width(width), height(height), surface(NULL), windowType (MoonWindowType_Plugin), transparent(false), windowingSystem(windowingSystem) { }

	virtual ~MoonWindow () { }

	virtual void ConnectToContainerPlatformWindow (gpointer container_window) = 0;

	virtual void Resize (int width, int height) = 0;

	virtual void SetCursor (CursorType cursor) = 0;
	virtual void Invalidate (Rect r) = 0;
	virtual void Invalidate () { Invalidate (Rect (0, 0, width, height)); }
	virtual void ProcessUpdates () = 0;

	virtual gboolean HandleEvent (gpointer platformEvent) = 0;

	virtual void Show () = 0;
	virtual void Hide () = 0;

	virtual void EnableEvents (bool first) = 0;
	virtual void DisableEvents () = 0;
	
	virtual void GrabFocus () = 0;
	virtual bool HasFocus () = 0;

	int GetWidth () { return width; }
	int GetHeight () { return height; }



	virtual void SetLeft (double left) = 0;
	virtual double GetLeft () = 0;

	virtual void SetTop (double top) = 0;
	virtual double GetTop () = 0;

	virtual void SetWidth (double width) = 0;

	virtual void SetHeight (double height) = 0;

	virtual void SetTitle (const char *title) = 0;

	virtual void SetIconFromPixbuf (MoonPixbuf *pixbuf) = 0;

	virtual void SetStyle (WindowStyle style) = 0;

	virtual void SetSurface (Surface* s) { surface = s; }
	Surface *GetSurface () { return surface; }

	/* @GeneratePInvoke */
	void SetTransparent (bool flag) { if (transparent != flag) Invalidate (); transparent = flag; }
	
	virtual void SetBackgroundColor (Color *color) { Invalidate (); };
	
	/* @GeneratePInvoke */
	bool GetTransparent () { return transparent; }

	bool IsFullScreen () { return windowType == MoonWindowType_FullScreen; }

	void SetCurrentDeployment ();

	/* @GeneratePInvoke */
	virtual MoonClipboard *GetClipboard (MoonClipboardType clipboardType) = 0;

	/* @GeneratePInvoke */
	virtual gpointer GetPlatformWindow () = 0;

	MoonWindowingSystem* GetWindowingSystem () { return windowingSystem; }

protected:
	int width;
	int height;
	Surface *surface;
	MoonWindowType windowType;
	bool transparent;

	MoonWindowingSystem* windowingSystem;
};

};
#endif /* __MOON_WINDOW__ */
