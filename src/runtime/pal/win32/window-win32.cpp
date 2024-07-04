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
#include <cairo-win32.h>

#include "window-win32.h"
#include "clipboard-win32.h"
#include "pixbuf-win32.h"
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

#include "events/button-win32.h"
#include "events/motion-win32.h"
#include "events/wheel-win32.h"
#include "events/key-win32.h"

using namespace Moonlight;


MoonWindowWin32::MoonWindowWin32(MoonWindowType windowType, int w, int h, MoonWindow *parent, Surface *surface, MoonWindowingSystem *windowingSystem)
    : MoonWindow(windowType, w, h, parent, surface, windowingSystem) {
    this->width = w;
    this->height = h;
    this->quitOnClose = false;
    this->handle = (HWND)INVALID_HANDLE_VALUE;
    this->monitor = (HMONITOR)INVALID_HANDLE_VALUE;
    this->damage = new Region();
    this->last_click_time = -1;
    this->num_clicks = 0;
    this->rc_clicks = { 0, 0, 0, 0 };

#ifdef USE_WGL
    gltarget = NULL;
    glctx = NULL;
    has_swap_rect = FALSE;
#else
    CreateCairoContext();
#endif
}

MoonWindowWin32::~MoonWindowWin32() {
#ifdef USE_WGL
    if (glctx)
        delete glctx;
    if (gltarget)
        gltarget->unref();
#endif

    if (damage)
        delete damage;
    if (clipboard)
        delete clipboard;

    DestroyWindow(handle);
}

void MoonWindowWin32::ConnectToContainerPlatformWindow(gpointer container_window) {
}

MoonClipboard *MoonWindowWin32::GetClipboard(MoonClipboardType clipboardType) {
    if (this->clipboard)
        return this->clipboard;

    return this->clipboard = new MoonClipboardWin32(this, clipboardType);
}

gpointer MoonWindowWin32::GetPlatformWindow() {
    return handle;
}

void MoonWindowWin32::Resize(int width, int height) {
    if (this->width == width && this->height == height)
        return;

    SetWindowPos(handle, nullptr, 0, 0, width, height, SWP_NOMOVE);

    g_warning("buffer = (%d,%d) surface = (%d,%d)", width, height, this->width, this->height);

    this->width = width;
    this->height = height;

    delete damage;
    damage = new Region(0.0, 0.0, width, height);

#if !defined(USE_WGL)
    CreateCairoContext();
#else
    gltarget->Reshape(width, height);
#endif

    if (surface)
        surface->HandleUIWindowAllocation(true);
}

void MoonWindowWin32::SetBackgroundColor(Color *color) {
}

void MoonWindowWin32::SetCursor(CursorType cursor) {
    LPWSTR pCursor = NULL;
    switch (cursor) {
    case CursorTypeDefault:
    case CursorTypeArrow:
        pCursor = IDC_ARROW;
        break;
    case CursorTypeHand:
        pCursor = IDC_HAND;
        break;
    case CursorTypeWait:
        pCursor = IDC_WAIT;
        break;
    case CursorTypeIBeam:
        pCursor = IDC_IBEAM;
        break;
    case CursorTypeSizeNS:
        pCursor = IDC_SIZENS;
        break;
    case CursorTypeSizeWE:
        pCursor = IDC_SIZEWE;
        break;
    case CursorTypeSizeNESW:
        pCursor = IDC_SIZENESW;
        break;
    case CursorTypeSizeNWSE:
        pCursor = IDC_SIZENWSE;
        break;
    case CursorTypeStylus:
    case CursorTypeEraser:
    case CursorTypeNone:
        break;
    }

    if (pCursor) {
        SetClassLongPtr(handle,                   // window handle
            GCLP_HCURSOR,                         // change cursor
            (LONG_PTR)LoadCursor(NULL, pCursor)); // new cursor
    }
}

void MoonWindowWin32::Invalidate(Rect r) {
    // FIXME
    // damage->Union(r);

    RECT rect = { r.x, r.y, r.x + r.width, r.y + r.height };
    InvalidateRect(handle, &rect, false);
}

void MoonWindowWin32::ProcessUpdates() {
}

gboolean MoonWindowWin32::HandleEvent(gpointer platformEvent) {
    // we have a message loop in the main thread, so we don't need to
    // handle events here
    return TRUE;
}

void MoonWindowWin32::Show() {
    if (surface) {
        surface->HandleUIWindowUnavailable();
        surface->HandleUIWindowAvailable();
    }

    ShowWindow(handle, SW_SHOWDEFAULT);
}

void MoonWindowWin32::Hide() {
    if (surface)
        surface->HandleUIWindowAvailable();

    ShowWindow(handle, SW_HIDE);
}

void MoonWindowWin32::EnableEvents(bool first) {
    // FIXME
}

void MoonWindowWin32::DisableEvents() {
    // FIXME
}

void MoonWindowWin32::GrabFocus() {
    SetFocus(handle);
}

bool MoonWindowWin32::HasFocus() {
    return GetActiveWindow() == handle;
}

void MoonWindowWin32::SetLeft(double left) {
    if (this->left == left)
        return;

    SetWindowPos(handle, nullptr, left, 0, 0, 0, SWP_NOSIZE);
}

double MoonWindowWin32::GetLeft() {
    return this->left;
}

void MoonWindowWin32::SetTop(double top) {
    if (this->top == top)
        return;

    SetWindowPos(handle, nullptr, 0, top, 0, 0, SWP_NOSIZE);
}

double MoonWindowWin32::GetTop() {
    return this->top;
}

void MoonWindowWin32::SetWidth(double width) {
    if (this->width == width)
        return;

    Resize(width, this->height);
}

void MoonWindowWin32::SetHeight(double height) {
    if (this->height == height)
        return;

    Resize(this->width, height);
}

void MoonWindowWin32::SetTitle(const char *title) {
    wchar_t *wTitle = reinterpret_cast<wchar_t *>(g_utf8_to_utf16(title, -1, NULL, NULL, NULL));

    SetWindowText(handle, wTitle);

    g_free(wTitle);
}

void MoonWindowWin32::SetIconFromPixbuf(MoonPixbuf *pixbuf) {
    auto pixbufWin32 = static_cast<MoonPixbufWin32 *>(pixbuf);
    if (!pixbufWin32)
        return;

    HBITMAP hBitmap = (HBITMAP)pixbufWin32->GetPlatformPixbuf();
    if (!hBitmap)
        return;

    HBITMAP hBitmapMask = CreateCompatibleBitmap(GetDC(handle), pixbuf->GetWidth(), pixbuf->GetHeight());
    ICONINFO iconInfo = { 0 };
    iconInfo.fIcon = TRUE;
    iconInfo.hbmColor = hBitmap;
    iconInfo.hbmMask = hBitmapMask;

    HICON hIcon = CreateIconIndirect(&iconInfo);
    SendMessage(handle, WM_SETICON, ICON_BIG, (LPARAM)hIcon);
    SendMessage(handle, WM_SETICON, ICON_SMALL, (LPARAM)hIcon);

    DeleteObject(hBitmapMask);
}

typedef HRESULT(WINAPI *PFNSETWINDOWATTRIBUTE)(HWND hWnd, DWORD dwAttribute, LPCVOID pvAttribute, DWORD cbAttribute);
enum DWM_WINDOW_CORNER_PREFERENCE {
    DWMWCP_DEFAULT = 0,
    DWMWCP_DONOTROUND = 1,
    DWMWCP_ROUND = 2,
    DWMWCP_ROUNDSMALL = 3
};
enum DWMWINDOWATTRIBUTE {
    DWMWA_WINDOW_CORNER_PREFERENCE = 33
};

void MoonWindowWin32::SetStyle(WindowStyle style) {
    DWORD dwStyle = 0;
    switch (style) {
    case WindowStyleBorderlessRoundCornersWindow:
        dwStyle = WS_OVERLAPPED | WS_THICKFRAME;
        break;
    case WindowStyleNone:
        dwStyle = WS_POPUP;
        break;
    default:
        dwStyle = WS_OVERLAPPEDWINDOW;
        break;
    }

    HMODULE hDwmApi = LoadLibrary(L"dwmapi.dll");
    if (hDwmApi) {
        // funny windows 11 magic :D
        auto *pfnSetWindowAttribute = reinterpret_cast<PFNSETWINDOWATTRIBUTE>(
            ::GetProcAddress(hDwmApi, "DwmSetWindowAttribute"));
        if (pfnSetWindowAttribute) {
            auto preference = style == WindowStyleBorderlessRoundCornersWindow ? DWMWCP_ROUNDSMALL : DWMWCP_DEFAULT;
            pfnSetWindowAttribute(this->handle, DWMWA_WINDOW_CORNER_PREFERENCE, &preference, sizeof(DWM_WINDOW_CORNER_PREFERENCE));
        }
        ::FreeLibrary(hDwmApi);
    }

    SetWindowLongPtr(this->handle, GWL_STYLE, dwStyle);
}

#ifdef USE_WGL
void MoonWindowWin32::CreateGlContext() {
    gltarget = new MoonWGLSurface((HWND)this->GetPlatformWindow(), GetDC((HWND)this->GetPlatformWindow()));
    glctx = new MoonWGLContext(gltarget);
}
#else
void MoonWindowWin32::CreateCairoContext() {
    target = new CairoSurface(width, height);
    ctx = new CairoContext(target);
}
#endif

void MoonWindowWin32::SetHandle(HWND hWnd) {
    this->handle = hWnd;
}

#if USE_WGL
void MoonWindowWin32::Paint() {
    PAINTSTRUCT ps;
    HDC hdc = BeginPaint(handle, &ps);

    SetCurrentDeployment();

    if (!gltarget) {
        gltarget = new MoonWGLSurface(this->handle, GetDC(this->handle));
        auto context = new MoonWGLContext(gltarget);

        if (context->Initialize()) {
            glctx = context;
        }
        else {
            delete context;
        }
    }

    if (damage->IsEmpty()) {
        // g_warning ("no damage");
        return;
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

    EndPaint(handle, &ps);
}
#else
void MoonWindowWin32::Paint() {
    PAINTSTRUCT ps;
    HDC hdc = BeginPaint(handle, &ps);

    SetCurrentDeployment();

    cairo_surface_t *native = cairo_win32_surface_create(hdc);
    cairo_t *cr = cairo_create(native);

    Context *ctx;
    cairo_rectangle_int_t area;
    area.x = ps.rcPaint.left;
    area.y = ps.rcPaint.top;
    area.width = ps.rcPaint.right - ps.rcPaint.left;
    area.height = ps.rcPaint.bottom - ps.rcPaint.top;

    MoonSurface *src;

    Rect r = Rect(area.x, area.y, area.width, area.height);
    Region *region = new Region(r);

    CairoSurface *target = new CairoSurface(1, 1);
    ctx = new CairoContext(target);
    target->unref();
    ctx->Push(Context::Group(r));
    surface->Paint(ctx, region, transparent, true);
    r = ctx->Pop(&src);

    if (!r.IsEmpty()) {
        cairo_surface_t *image = (cairo_surface_t *)src->Cairo();
        cairo_surface_flush(image);
        cairo_set_source_surface(cr, image, r.x, r.y);
        cairo_set_operator(cr, transparent ? CAIRO_OPERATOR_SOURCE : CAIRO_OPERATOR_OVER);
        region->Draw(cr);
        cairo_fill(cr);

        cairo_surface_destroy(image);
        src->unref();
    }

    cairo_destroy(cr);
    cairo_surface_destroy(native);
    // cairo_region_destroy(t_region);

    EndPaint(handle, &ps);

    delete ctx;
    delete region;
    // delete[] rects;
}
#endif

LRESULT MoonWindowWin32::WndProc(UINT msg, WPARAM wParam, LPARAM lParam) {
    switch (msg) {
    case WM_PAINT: {
        this->Paint();
        return 0;
    }
    case WM_SIZE: {
        this->width = LOWORD(lParam);
        this->height = HIWORD(lParam);

        delete damage;
        damage = new Region(0.0, 0.0, width, height);

        if (GetSurface())
            GetSurface()->HandleUIWindowAllocation(true);

        return 0;
    }

    case WM_MOVE: {
        this->left = LOWORD(lParam);
        this->top = HIWORD(lParam);
        return 0;
    }

    case WM_DESTROY: {
#ifdef USE_WGL
#else
        if (target != nullptr)
            delete target;
        target = nullptr;
        if (ctx != nullptr)
            delete ctx;
        ctx = nullptr;
#endif

        if (this->quitOnClose)
            PostQuitMessage(0);

        return 0;
    }

    case WM_MOUSEMOVE: {
        auto x = GET_X_LPARAM(lParam);
        auto y = GET_Y_LPARAM(lParam);
        auto event = new MoonMotionEventWin32(wParam, 0, x, y);
        event->DispatchToWindow(this);
        return 0;
    }
    case WM_LBUTTONDOWN:
    case WM_RBUTTONDOWN: {
        auto x = GET_X_LPARAM(lParam);
        auto y = GET_Y_LPARAM(lParam);

        DWORD time = GetMessageTime();

        if (!PtInRect(&rc_clicks, { x, y }) || time - last_click_time > GetDoubleClickTime()) {
            num_clicks = 0;
        }
        num_clicks++;
        last_click_time = time;

        SetRect(&rc_clicks, x, y, x, y);
        InflateRect(&rc_clicks, GetSystemMetrics(SM_CXDOUBLECLK) / 2, GetSystemMetrics(SM_CYDOUBLECLK) / 2);

        auto event = new MoonButtonEventWin32(msg == WM_RBUTTONDOWN ? 3 : 1, false, 0, x, y, this->num_clicks);
        event->DispatchToWindow(this);
        break;
    }
    case WM_LBUTTONUP:
    case WM_RBUTTONUP: {
        auto x = GET_X_LPARAM(lParam);
        auto y = GET_Y_LPARAM(lParam);

        auto event = new MoonButtonEventWin32(msg == WM_RBUTTONUP ? 3 : 1, true, 0, x, y, this->num_clicks);
        event->DispatchToWindow(this);
        break;
    }
    case WM_MOUSEWHEEL: {
        auto x = GET_X_LPARAM(lParam);
        auto y = GET_Y_LPARAM(lParam);
        auto keys = GET_KEYSTATE_WPARAM(wParam);
        auto delta = (float)GET_WHEEL_DELTA_WPARAM(wParam) / WHEEL_DELTA;

        auto event = new MoonWheelEventWin32(0, x, y, delta);
        event->DispatchToWindow(this);
        return 0;
    }

    case WM_KEYDOWN: {
        if ((wParam >= 'A' && wParam <= 'Z') || (wParam >= '0' && wParam >= '9') || wParam == VK_SPACE)
            break;
        auto event = new MoonKeyEventWin32(true, false, wParam, lParam, 0);
        event->DispatchToWindow(this);
        break;
    }

    case WM_CHAR: {
        if (wParam == 0x8 || wParam == 0xA || wParam == 0x1B || wParam == 0x09 || wParam == 0x0D)
            break;
        auto event = new MoonKeyEventWin32(true, true, wParam, lParam, 0);
        event->DispatchToWindow(this);
        break;
    }

    case WM_KEYUP: {
        auto event = new MoonKeyEventWin32(false, false, wParam, lParam, 0);
        event->DispatchToWindow(this);
        break;
    }

    case WM_WINDOWPOSCHANGED: {
        HMONITOR currentMonitor = MonitorFromWindow(this->handle, MONITOR_DEFAULTTONEAREST);
        if (currentMonitor != this->monitor) {
            this->monitor = currentMonitor;

            MONITORINFOEX monitorInfo{};
            monitorInfo.cbSize = sizeof(MONITORINFOEX);
            if (!GetMonitorInfo(currentMonitor, (LPMONITORINFO)&monitorInfo))
                break;

            DEVMODE devMode{};
            devMode.dmSize = sizeof(DEVMODE);
            if (!EnumDisplaySettings(monitorInfo.szDevice, ENUM_CURRENT_SETTINGS, &devMode))
                break;

            if (this->surface)
                this->surface->GetTimeManager()
                    ->SetMaximumRefreshRate(devMode.dmDisplayFrequency);
        }
        break;
    }
    case WM_DISPLAYCHANGE: {
        this->monitor = nullptr;
        break;
    }
    }

    return DefWindowProc(handle, msg, wParam, lParam);
}

void Moonlight::MoonWindowWin32::SetQuitOnClose(bool quitOnClose) {
    this->quitOnClose = quitOnClose;
}
