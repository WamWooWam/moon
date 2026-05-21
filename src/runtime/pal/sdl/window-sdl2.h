/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#pragma once

#include <glib.h>

#include "window.h"
#include "runtime.h"

#include "pal-sdl2.h"
#include "gl/context-sdl2.h"

namespace Moonlight {

    /* @Namespace=System.Windows */
    class MoonWindowSDL2 : public MoonWindow {
        friend class MoonWindowingSystemSDL2;

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
        bool GetQuitOnClose() { return quitOnClose; }

        void RegisterWindow(MoonWindow *window);

    private:
        void CreateGlContext();

        MoonSDLGLSurface *gltarget;
        MoonSDLGLContext *glctx;
        bool has_swap_rect;
        Region *damage;

        SDL_Window *window;
        SDL_Renderer *renderer;
        SDL_Cursor *activeCursor;

        double left;
        double top;

        MoonClipboard *clipboard;
        bool quitOnClose;
    };

};
