/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#include "config.h"

#include <glib.h>

#include "window-sdl2.h"
#include "clipboard-sdl2.h"
#include "pixbuf-sdl2.h"
#include "deployment.h"
#include "timemanager.h"
#include "enums.h"
#include "context-cairo.h"

#include "gl/context-sdl2.h"
#include "gl/surface-sdl2.h"

using namespace Moonlight;


MoonWindowSDL2::MoonWindowSDL2(MoonWindowType windowType, int w, int h, MoonWindow *parent, Surface *surface, MoonWindowingSystem *windowingSystem)
    : MoonWindow(windowType, w, h, parent, surface, windowingSystem) {
    this->width = w;
    this->height = h;
    this->left = 0;
    this->top = 0;
    this->quitOnClose = false;
    this->damage = new Region();
    this->clipboard = NULL;
    this->activeCursor = NULL;

    gltarget = NULL;
    glctx = NULL;
    has_swap_rect = false;
    renderer = NULL;

    Uint32 flags = SDL_WINDOW_OPENGL | SDL_WINDOW_RESIZABLE;
    if (windowType == MoonWindowType_FullScreen)
        flags |= SDL_WINDOW_FULLSCREEN_DESKTOP;

    this->window = SDL_CreateWindow("Moonlight", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, w, h, flags);

    if (!window) {
        g_warning("Failed to create SDL window: %s", SDL_GetError());
        return;
    }

    // this->renderer = SDL_CreateRenderer(window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);

    if (windowingSystem)
        ((MoonWindowingSystemSDL2*)windowingSystem)->RegisterWindow(this);
}

MoonWindowSDL2::~MoonWindowSDL2() {
    if (GetWindowingSystem())
        ((MoonWindowingSystemSDL2*)GetWindowingSystem())->UnregisterWindow(this);

    if (glctx)
        delete glctx;
    if (gltarget)
        gltarget->unref();
    if (damage)
        delete damage;
    if (clipboard)
        delete clipboard;
    if (activeCursor)
        SDL_FreeCursor(activeCursor);
    if (window)
        SDL_DestroyWindow(window);
}

void MoonWindowSDL2::ConnectToContainerPlatformWindow(gpointer container_window) {
}

MoonClipboard *MoonWindowSDL2::GetClipboard(MoonClipboardType clipboardType) {
    if (this->clipboard)
        return this->clipboard;
    return this->clipboard = new MoonClipboardSDL2(this, clipboardType);
}

gpointer MoonWindowSDL2::GetPlatformWindow() {
    return window;
}

void MoonWindowSDL2::Resize(int width, int height) {
    if (this->width == width && this->height == height)
        return;

    SDL_SetWindowSize(this->window, width, height);

    this->width = width;
    this->height = height;

    delete damage;
    damage = new Region(0.0, 0.0, width, height);

    if (gltarget)
        gltarget->Reshape(width, height);

    if (surface)
        surface->HandleUIWindowAllocation(true);
}

void MoonWindowSDL2::SetBackgroundColor(Color *color) {
}

void MoonWindowSDL2::SetCursor(CursorType cursor) {
    if (!window)
        return;

    SDL_Cursor *c = NULL;
    switch (cursor) {
    case CursorTypeDefault:
    case CursorTypeArrow:
        c = SDL_CreateSystemCursor(SDL_SYSTEM_CURSOR_ARROW);
        break;
    case CursorTypeHand:
        c = SDL_CreateSystemCursor(SDL_SYSTEM_CURSOR_HAND);
        break;
    case CursorTypeWait:
        c = SDL_CreateSystemCursor(SDL_SYSTEM_CURSOR_WAITARROW);
        break;
    case CursorTypeIBeam:
        c = SDL_CreateSystemCursor(SDL_SYSTEM_CURSOR_IBEAM);
        break;
    case CursorTypeSizeNS:
        c = SDL_CreateSystemCursor(SDL_SYSTEM_CURSOR_SIZENS);
        break;
    case CursorTypeSizeWE:
        c = SDL_CreateSystemCursor(SDL_SYSTEM_CURSOR_SIZEWE);
        break;
    case CursorTypeStylus:
    case CursorTypeEraser:
        c = SDL_CreateSystemCursor(SDL_SYSTEM_CURSOR_CROSSHAIR);
        break;
    case CursorTypeNone:
    default:
        SDL_ShowCursor(SDL_DISABLE);
        if (activeCursor) {
            SDL_FreeCursor(activeCursor);
            activeCursor = NULL;
        }
        return;
    }

    if (c) {
        if (activeCursor)
            SDL_FreeCursor(activeCursor);
        activeCursor = c;
        SDL_SetCursor(c);
        SDL_ShowCursor(SDL_ENABLE);
    }
}

void MoonWindowSDL2::Invalidate(Rect r) {
    damage->Union(r);
}

void MoonWindowSDL2::ProcessUpdates() {
    this->Paint();
}

gboolean MoonWindowSDL2::HandleEvent(gpointer platformEvent) {
    return TRUE;
}

void MoonWindowSDL2::Show() {
    if (!window)
        return;

    SDL_ShowWindow(window);

    if (surface) {
        surface->HandleUIWindowUnavailable();
        surface->HandleUIWindowAvailable();
    }
}

void MoonWindowSDL2::Hide() {
    if (!window)
        return;

    SDL_HideWindow(window);

    if (surface)
        surface->HandleUIWindowUnavailable();
}

void MoonWindowSDL2::EnableEvents(bool first) {
}

void MoonWindowSDL2::DisableEvents() {
}

void MoonWindowSDL2::GrabFocus() {
    if (window)
        SDL_RaiseWindow(window);
}

bool MoonWindowSDL2::HasFocus() {
    if (!window)
        return false;
    return (SDL_GetWindowFlags(window) & SDL_WINDOW_INPUT_FOCUS) != 0;
}

void MoonWindowSDL2::SetLeft(double left) {
    this->left = left;
    if (window) {
        int x, y;
        SDL_GetWindowPosition(window, &x, &y);
        SDL_SetWindowPosition(window, (int)left, y);
    }
}

double MoonWindowSDL2::GetLeft() {
    return this->left;
}

void MoonWindowSDL2::SetTop(double top) {
    this->top = top;
    if (window) {
        int x, y;
        SDL_GetWindowPosition(window, &x, &y);
        SDL_SetWindowPosition(window, x, (int)top);
    }
}

double MoonWindowSDL2::GetTop() {
    return this->top;
}

void MoonWindowSDL2::SetWidth(double width) {
    if (this->width == (int)width)
        return;
    Resize((int)width, this->height);
}

void MoonWindowSDL2::SetHeight(double height) {
    if (this->height == (int)height)
        return;
    Resize(this->width, (int)height);
}

void MoonWindowSDL2::SetTitle(const char *title) {
    if (window)
        SDL_SetWindowTitle(window, title);
}

void MoonWindowSDL2::SetIconFromPixbuf(MoonPixbuf *pixbuf) {
}

void MoonWindowSDL2::SetStyle(WindowStyle style) {
    if (!window)
        return;

    switch (style) {
    case WindowStyleBorderlessRoundCornersWindow:
    case WindowStyleNone:
        SDL_SetWindowBordered(window, SDL_FALSE);
        break;
    default:
        SDL_SetWindowBordered(window, SDL_TRUE);
        break;
    }
}

void MoonWindowSDL2::CreateGlContext() {
    gltarget = new MoonSDLGLSurface(this->window, this->renderer);
    glctx = new MoonSDLGLContext(gltarget);
}

void MoonWindowSDL2::Paint() {
    if (!window || !surface)
        return;

    SetCurrentDeployment();

    if (!gltarget) {
        gltarget = new MoonSDLGLSurface(this->window, this->renderer);
        auto context = new MoonSDLGLContext(gltarget);

        if (context->Initialize()) {
            glctx = context;
        }
        else {
            delete context;
            gltarget->unref();
            gltarget = NULL;
        }
    }

    if (gltarget && glctx) {
        Rect r0 = Rect(0, 0, width, height);
        Region r1 = Region(r0);

        gltarget->Reshape(width, height);

        static_cast<Context *>(glctx)->Push(Context::Clip(r0));
        surface->Paint(glctx, &r1, GetTransparent(), true);
        static_cast<Context *>(glctx)->Pop();

        glctx->Flush();
        gltarget->SwapBuffers();

        delete damage;
        damage = new Region();
    }
}

void MoonWindowSDL2::SetQuitOnClose(bool quitOnClose) {
    this->quitOnClose = quitOnClose;
}
