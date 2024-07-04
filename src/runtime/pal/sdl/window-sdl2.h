/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * window-android.h: MoonWindow implementation using android widgets.
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2007 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#pragma once

#include <glib.h>

#include "window.h"
#include "runtime.h"

#include "pal-sdl2.h"
#include "context-sdl2.h"

namespace Moonlight {

#if USE_WGL
    class MoonWGLSurface;
    class MoonWGLContext;
#endif

    /* @Namespace=System.Windows */
    class MoonWindowSDL2 : public MoonWindow {
        friend class MoonWindowSystemSDL2;

    public:
        MoonWindowSDL2(MoonWindowType windowType, int w = -1, int h = -1, MoonWindow *parent = NULL, Surface *surface = NULL, MoonWindowingSystem *windowingSystem = NULL);

        virtual ~MoonWindowSDL2();

        virtual void ConnectToContainerPlatformWindow(gpointer container_window);

        virtual void Resize(int width, int height);
        virtual void SetCursor(CursorType cursor);
        virtual void SetBackgroundColor(Color *color);
        virtual void Invalidate(Rect r);
        virtual void ProcessUpdates();
        virtual gboolean HandleEvent(gpointer platformEvent);
        virtual void Show();
        virtual void Hide();
        virtual void EnableEvents(bool first);
        virtual void DisableEvents();

        virtual void SetLeft(double left);
        virtual double GetLeft();

        virtual void SetTop(double top);
        virtual double GetTop();

        virtual void SetWidth(double width);

        virtual void SetHeight(double height);

        virtual void SetTitle(const char *title);

        virtual void SetIconFromPixbuf(MoonPixbuf *pixbuf);

        virtual void SetStyle(WindowStyle style);

        virtual void GrabFocus();
        virtual bool HasFocus();

        virtual MoonClipboard *GetClipboard(MoonClipboardType clipboardType);

        virtual gpointer GetPlatformWindow();

        void Paint();

        void SetQuitOnClose(bool quitOnClose);

    private:
        void CreateGlContext();
        MoonSDLGLSurface *gltarget;
        MoonSDLGLContext *glctx;
        bool has_swap_rect;
        Region *damage;

        SDL_Window *window;
        SDL_Renderer *renderer;

        double left;
        double top;

        MoonClipboard *clipboard;

        bool quitOnClose;
    };

};
