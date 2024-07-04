/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * window-gtk.cpp: MoonWindow implementation using gtk widgets.
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

#include "window-sdl2.h"
#include "clipboard-sdl2.h"
#include "pixbuf-sdl2.h"
#include "deployment.h"
#include "timemanager.h"
#include "enums.h"
#include "context-cairo.h"

#define EGL_EGLEXT_PROTOTYPES 1

#ifdef USE_EGL
#include "context-egl.h"
#endif

#ifdef USE_WGL
#include "context-wgl.h"
#endif

#include <windowsx.h>

#include "events/button-sdl2.h"
#include "events/motion-sdl2.h"
#include "events/wheel-sdl2.h"
#include "events/key-sdl2.h"

using namespace Moonlight;


MoonWindowSDL2::MoonWindowSDL2(MoonWindowType windowType, int w, int h, MoonWindow *parent, Surface *surface, MoonWindowingSystem *windowingSystem)
    : MoonWindow(windowType, w, h, parent, surface, windowingSystem) {
    this->width = w;
    this->height = h;
    this->quitOnClose = false;
    this->damage = new Region();

    gltarget = NULL;
    glctx = NULL;
    has_swap_rect = FALSE;

    this->window = SDL_CreateWindow("balls", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, w, h, SDL_WINDOW_OPENGL);

    if (!window) {
        g_warning("Failed to create window: %s", SDL_GetError());
        return;
    }
}

MoonWindowSDL2::~MoonWindowSDL2() {
    if (glctx)
        delete glctx;
    if (gltarget)
        gltarget->unref();
    if (damage)
        delete damage;
    if (clipboard)
        delete clipboard;
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

    g_warning("buffer = (%d,%d) surface = (%d,%d)", width, height, this->width, this->height);

    this->width = width;
    this->height = height;

    delete damage;
    damage = new Region(0.0, 0.0, width, height);
    gltarget->Reshape(width, height);

    if (surface)
        surface->HandleUIWindowAllocation(true);
}

void MoonWindowSDL2::SetBackgroundColor(Color *color) {
}

void MoonWindowSDL2::SetCursor(CursorType cursor) {
}

void MoonWindowSDL2::Invalidate(Rect r) {
    // FIXME
    damage->Union(r);
}

void MoonWindowSDL2::ProcessUpdates() {
    this->Paint();
}

gboolean MoonWindowSDL2::HandleEvent(gpointer platformEvent) {
    // we have a message loop in the main thread, so we don't need to
    // handle events here
    return TRUE;
}

void MoonWindowSDL2::Show() {
    if (surface) {
        surface->HandleUIWindowUnavailable();
        surface->HandleUIWindowAvailable();
    }

    // ShowWindow(handle, SW_SHOWDEFAULT);
}

void MoonWindowSDL2::Hide() {
    if (surface)
        surface->HandleUIWindowAvailable();

    // ShowWindow(handle, SW_HIDE);
}

void MoonWindowSDL2::EnableEvents(bool first) {
    // FIXME
}

void MoonWindowSDL2::DisableEvents() {
    // FIXME
}

void MoonWindowSDL2::GrabFocus() {
}

bool MoonWindowSDL2::HasFocus() {
    return false;
}

void MoonWindowSDL2::SetLeft(double left) {
}

double MoonWindowSDL2::GetLeft() {
    return this->left;
}

void MoonWindowSDL2::SetTop(double top) {
}

double MoonWindowSDL2::GetTop() {
    return this->top;
}

void MoonWindowSDL2::SetWidth(double width) {
    if (this->width == width)
        return;

    Resize(width, this->height);
}

void MoonWindowSDL2::SetHeight(double height) {
    if (this->height == height)
        return;

    Resize(this->width, height);
}

void MoonWindowSDL2::SetTitle(const char *title) {
}

void MoonWindowSDL2::SetIconFromPixbuf(MoonPixbuf *pixbuf) {
}

void MoonWindowSDL2::SetStyle(WindowStyle style) {
}

void MoonWindowSDL2::CreateGlContext() {
    gltarget = new MoonSDLGLSurface(this->window, this->renderer);
    glctx = new MoonSDLGLContext(gltarget);
}

void MoonWindowSDL2::Paint() {
    SetCurrentDeployment();

    if (!gltarget) {
        gltarget = new MoonSDLGLSurface(this->window, this->renderer);
        auto context = new MoonSDLGLContext(gltarget);

        if (context->Initialize()) {
            glctx = context;
        }
        else {
            delete context;
        }
    }

    if (damage->IsEmpty()) {
        // g_warning ("no damage");
        // return;
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
    }
    else {
        g_warning("uhoh");
    }
}

void Moonlight::MoonWindowSDL2::SetQuitOnClose(bool quitOnClose) {
    this->quitOnClose = quitOnClose;
}
