/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#include <config.h>

#include <string>

#include "pal-sdl2.h"

#include "runtime.h"
#include "window-sdl2.h"
#include "pixbuf-sdl2.h"
#include "im-sdl2.h"
#include "debug.h"

#include <glib.h>

#include "events/button-sdl2.h"
#include "events/motion-sdl2.h"
#include "events/wheel-sdl2.h"
#include "events/key-sdl2.h"
#include "events/crossing-sdl2.h"
#include "events/focus-sdl2.h"

#undef CreateWindow

using namespace Moonlight;

class SDL2Source {
public:
    SDL2Source(int source_id, int priority, int interval, MoonSourceFunc source_func, gpointer data) {
        this->source_id = source_id;
        this->priority = priority;
        this->interval = interval;
        this->source_func = source_func;
        this->data = data;
        time_remaining = interval;
        pending_destroy = false;
    }

    bool InvokeSourceFunc() {
        return source_func(data);
    }

    static gint Compare(gconstpointer p1, gconstpointer p2) {
        const SDL2Source *source1 = (const SDL2Source *)p1;
        const SDL2Source *source2 = (const SDL2Source *)p2;

        gint result = source1->time_remaining - source2->time_remaining;
        if (result != 0)
            return result;

        return source2->priority - source1->priority;
    }

    gint32 time_remaining;
    bool pending_destroy;
    guint source_id;
    int priority;
    gint32 interval;
    MoonSourceFunc source_func;
    gpointer data;
};

MoonWindowingSystemSDL2::MoonWindowingSystemSDL2(bool out_of_browser)
    : sourceMutex(false) {
    source_id = 1;
    sources = NULL;
    windows = NULL;
    timer = 0;
    before = -1;
    emitting_sources = false;

    SDL_Init(SDL_INIT_VIDEO | SDL_INIT_TIMER | SDL_INIT_AUDIO);
}

MoonWindowingSystemSDL2::~MoonWindowingSystemSDL2() {
    if (windows)
        g_list_free(windows);
}

void MoonWindowingSystemSDL2::ShowCodecsUnavailableMessage() {
}

cairo_surface_t *MoonWindowingSystemSDL2::CreateSurface() {
    g_assert_not_reached();
}

void MoonWindowingSystemSDL2::ExitApplication() {
    SDL_Quit();
}

MoonWindow *MoonWindowingSystemSDL2::CreateWindow(MoonWindowType windowType, int width, int height, MoonWindow *parentWindow, Surface *surface) {
    return new MoonWindowSDL2(windowType, width, height, parentWindow, surface, this);
}

MoonWindow *MoonWindowingSystemSDL2::CreateWindowless(int width, int height, PluginInstance *forPlugin) {
    g_warning("no windowless support on SDL2");
    return NULL;
}

MoonMessageBoxResult MoonWindowingSystemSDL2::ShowMessageBox(MoonMessageBoxType message_type, const char *caption, const char *text, MoonMessageBoxButton button) {
    return MoonMessageBoxResult::MessageBoxResultOk;
}

char **MoonWindowingSystemSDL2::ShowOpenFileDialog(const char *title, bool multsel, const char *filter, int idx) {
    return NULL;
}

char *MoonWindowingSystemSDL2::ShowSaveFileDialog(const char *title, const char *filter, int idx) {
    return NULL;
}

bool MoonWindowingSystemSDL2::ShowConsentDialog(const char *question, const char *detail, const char *website, bool *remember) {
    return false;
}

void MoonWindowingSystemSDL2::RegisterWindow(MoonWindow *window) {
    windows = g_list_append(windows, window);
}

void MoonWindowingSystemSDL2::UnregisterWindow(MoonWindow *window) {
    windows = g_list_remove(windows, window);
}

MoonWindowSDL2 *MoonWindowingSystemSDL2::FindWindowByID(Uint32 windowID) {
    for (GList *l = windows; l; l = l->next) {
        MoonWindowSDL2 *w = (MoonWindowSDL2 *)l->data;
        SDL_Window *sdlwin = (SDL_Window *)w->GetPlatformWindow();
        if (sdlwin && SDL_GetWindowID(sdlwin) == windowID)
            return w;
    }
    return NULL;
}

Color *MoonWindowingSystemSDL2::GetSystemColor(SystemColor id) {
    return NULL;
}

guint MoonWindowingSystemSDL2::AddTimeout(gint priority, gint ms, MoonSourceFunc timeout, gpointer data) {
    sourceMutex.Lock();

    int new_source_id = source_id;
    SDL2Source *new_source = new SDL2Source(new_source_id, priority, ms, timeout, data);
    sources = g_list_insert_sorted(sources, new_source, SDL2Source::Compare);
    source_id++;

    sourceMutex.Unlock();

    AddSDL2Timer();

    return new_source_id;
}

void MoonWindowingSystemSDL2::RemoveSource(guint sourceId) {
    sourceMutex.Lock();

    for (GList *l = sources; l; l = l->next) {
        SDL2Source *s = (SDL2Source *)l->data;
        if (s->source_id == sourceId) {
            if (emitting_sources) {
                s->pending_destroy = true;
            }
            else {
                sources = g_list_delete_link(sources, l);
                delete s;
            }
            break;
        }
    }

    sourceMutex.Unlock();
}

void MoonWindowingSystemSDL2::RemoveTimeout(guint timeoutId) {
    RemoveSource(timeoutId);
}

guint MoonWindowingSystemSDL2::AddIdle(MoonSourceFunc idle, gpointer data) {
    sourceMutex.Lock();

    int new_source_id = source_id;
    SDL2Source *new_source = new SDL2Source(new_source_id, MOON_PRIORITY_DEFAULT_IDLE, 0, idle, data);
    sources = g_list_insert_sorted(sources, new_source, SDL2Source::Compare);
    source_id++;

    sourceMutex.Unlock();
    return new_source_id;
}

void MoonWindowingSystemSDL2::RemoveIdle(guint idleId) {
    RemoveSource(idleId);
}

MoonIMContext *MoonWindowingSystemSDL2::CreateIMContext() {
    return new MoonIMContextSDL2();
}

MoonEvent *MoonWindowingSystemSDL2::CreateEventFromPlatformEvent(gpointer platformEvent) {
    if (!platformEvent)
        return NULL;

    SDL_Event *ev = (SDL_Event *)platformEvent;
    switch (ev->type) {
    case SDL_MOUSEMOTION:
        return new MoonMotionEventSDL2(1.0f, ev->motion.x, ev->motion.y, SDL_GetModState());
    case SDL_MOUSEBUTTONDOWN:
    case SDL_MOUSEBUTTONUP: {
        int btn = ev->button.button == SDL_BUTTON_RIGHT ? 3 : ev->button.button;
        bool rel = ev->type == SDL_MOUSEBUTTONUP;
        return new MoonButtonEventSDL2(btn, rel, 1.0f, ev->button.x, ev->button.y, ev->button.clicks, SDL_GetModState());
    }
    case SDL_MOUSEWHEEL: {
        float delta = ev->wheel.direction == SDL_MOUSEWHEEL_FLIPPED ? -(float)ev->wheel.y : (float)ev->wheel.y;
        int mx, my;
        SDL_GetMouseState(&mx, &my);
        return new MoonWheelEventSDL2(1.0f, mx, my, delta);
    }
    case SDL_KEYDOWN:
        return new MoonKeyEventSDL2(true, false, ev->key.keysym.sym, ev->key.keysym.scancode, (SDL_Keymod)ev->key.keysym.mod);
    case SDL_KEYUP:
        return new MoonKeyEventSDL2(false, false, ev->key.keysym.sym, ev->key.keysym.scancode, (SDL_Keymod)ev->key.keysym.mod);
    default:
        return NULL;
    }
}

MoonModifier
MoonWindowingSystemSDL2::GetCommandModifier() {
    return MoonModifier_Control;
}

guint MoonWindowingSystemSDL2::GetCursorBlinkTimeout(MoonWindow *moon_window) {
#if defined(GetCaretBlinkTime)
    return GetCaretBlinkTime();
#else
    return CURSOR_BLINK_TIMEOUT_DEFAULT;
#endif
}

MoonPixbufLoader *MoonWindowingSystemSDL2::CreatePixbufLoader(const char *imageType) {
    return new MoonPixbufLoaderSDL2(imageType);
}

static gint32 get_now_in_millis(void) {
    return SDL_GetTicks();
}

void MoonWindowingSystemSDL2::OnTick() {
    gint32 after = get_now_in_millis();

    sourceMutex.Lock();

    emitting_sources = true;

    GList *sources_to_dispatch = NULL;
    if (sources) {
        int max_priority = G_MAXINT;
        gint32 delta = before - after;
        GList *l = sources;

        while (l) {
            SDL2Source *s = (SDL2Source *)l->data;

            if (s->time_remaining + delta < 0) {
                if (max_priority == G_MAXINT) {
                    max_priority = s->priority;
                    sources_to_dispatch = g_list_prepend(sources_to_dispatch, s);
                }
                else {
                    s->time_remaining += delta;
                    if (s->priority >= max_priority && s->time_remaining < 0)
                        sources_to_dispatch = g_list_prepend(sources_to_dispatch, s);
                }
            }
            else {
                s->time_remaining += delta;
            }

            l = l->next;
        }

        sources_to_dispatch = g_list_reverse(sources_to_dispatch);
    }

    sourceMutex.Unlock();

    for (GList *l = sources_to_dispatch; l; l = l->next) {
        SDL2Source *s = (SDL2Source *)l->data;
        if (!s->pending_destroy) {
            bool pending_destroy = !s->InvokeSourceFunc();
            if (!s->pending_destroy)
                s->pending_destroy = pending_destroy;
        }
    }

    g_list_free(sources_to_dispatch);

    sourceMutex.Lock();
    for (GList *l = sources; l;) {
        SDL2Source *s = (SDL2Source *)l->data;
        if (s->pending_destroy) {
            GList *next = l->next;
            sources = g_list_delete_link(sources, l);
            delete s;
            l = next;
        }
        else {
            l = l->next;
        }
    }

    timer = 0;
    emitting_sources = false;
    sourceMutex.Unlock();

    AddSDL2Timer();
}

void MoonWindowingSystemSDL2::AddSDL2Timer() {
    int timeout = -1;

    sourceMutex.Lock();
    if (timer != 0) {
        sourceMutex.Unlock();
        return;
    }
    if (sources != NULL) {
        SDL2Source *s = (SDL2Source *)sources->data;
        timeout = s->time_remaining;
        if (timeout < 0)
            timeout = 0;
    }

    if (timeout >= 0) {
        timer = SDL_AddTimer(
            timeout, [](Uint32 interval, void *param) -> Uint32 {
                SDL_Event ev;
                SDL_memset(&ev, 0, sizeof(ev));
                ev.type = SDL_USEREVENT;
                SDL_PushEvent(&ev);
                return 0;
            },
            this);
        before = get_now_in_millis();
    }

    sourceMutex.Unlock();
}

void MoonWindowingSystemSDL2::RunMainLoop(MoonWindow *window, bool quit_on_window_close) {
    if (window) {
        reinterpret_cast<MoonWindowSDL2 *>(window)->SetQuitOnClose(quit_on_window_close);
        window->Show();
    }

    AddSDL2Timer();

    bool running = true;
    SDL_Event event;
    while (running && SDL_WaitEvent(&event)) {
        switch (event.type) {
        case SDL_QUIT:
            running = false;
            break;

        case SDL_USEREVENT:
            OnTick();
            break;

        case SDL_MOUSEMOTION: {
            MoonWindowSDL2 *win = FindWindowByID(event.motion.windowID);
            if (win) {
                auto ev = new MoonMotionEventSDL2(1.0f, event.motion.x, event.motion.y, SDL_GetModState());
                ev->DispatchToWindow(win);
                delete ev;
            }
            break;
        }

        case SDL_MOUSEBUTTONDOWN:
        case SDL_MOUSEBUTTONUP: {
            MoonWindowSDL2 *win = FindWindowByID(event.button.windowID);
            if (win) {
                int btn = event.button.button == SDL_BUTTON_RIGHT ? 3 : event.button.button;
                bool is_release = event.type == SDL_MOUSEBUTTONUP;
                auto ev = new MoonButtonEventSDL2(btn, is_release, 1.0f, event.button.x, event.button.y, event.button.clicks, SDL_GetModState());
                ev->DispatchToWindow(win);
                delete ev;
            }
            break;
        }

        case SDL_MOUSEWHEEL: {
            MoonWindowSDL2 *win = FindWindowByID(event.wheel.windowID);
            if (win) {
                float delta = event.wheel.direction == SDL_MOUSEWHEEL_FLIPPED
                                  ? -(float)event.wheel.y
                                  : (float)event.wheel.y;
                int mx, my;
                SDL_GetMouseState(&mx, &my);
                auto ev = new MoonWheelEventSDL2(1.0f, (float)mx, (float)my, delta);
                ev->DispatchToWindow(win);
                delete ev;
            }
            break;
        }

        case SDL_KEYDOWN: {
            MoonWindowSDL2 *win = FindWindowByID(event.key.windowID);
            if (win) {
                SDL_Keycode sym = event.key.keysym.sym;
                SDL_Keymod mod = (SDL_Keymod)event.key.keysym.mod;
                // Skip printable ASCII keys — SDL_TEXTINPUT handles those,
                // except when a modifier is held (Ctrl+key, etc.)
                bool is_printable = (sym >= SDLK_SPACE && sym <= 0x7e);
                if (!is_printable || (mod & (KMOD_CTRL | KMOD_ALT))) {
                    auto ev = new MoonKeyEventSDL2(true, false, sym, event.key.keysym.scancode, mod);
                    ev->DispatchToWindow(win);
                    delete ev;
                }
            }
            break;
        }

        case SDL_KEYUP: {
            MoonWindowSDL2 *win = FindWindowByID(event.key.windowID);
            if (win) {
                SDL_Keymod mod = (SDL_Keymod)event.key.keysym.mod;
                auto ev = new MoonKeyEventSDL2(false, false, event.key.keysym.sym, event.key.keysym.scancode, mod);
                ev->DispatchToWindow(win);
                delete ev;
            }
            break;
        }

        case SDL_TEXTINPUT: {
            MoonWindowSDL2 *win = FindWindowByID(event.text.windowID);
            if (win && event.text.text[0]) {
                gunichar uc = g_utf8_get_char(event.text.text);
                if (uc != 0 && uc != (gunichar)-1) {
                    auto ev = new MoonKeyEventSDL2(true, true, (int32_t)uc, 0, SDL_GetModState());
                    ev->DispatchToWindow(win);
                    delete ev;
                }
            }
            break;
        }

        case SDL_WINDOWEVENT: {
            MoonWindowSDL2 *win = FindWindowByID(event.window.windowID);
            if (!win)
                break;
            switch (event.window.event) {
            case SDL_WINDOWEVENT_ENTER: {
                int mx, my;
                SDL_GetMouseState(&mx, &my);
                auto ev = new MoonCrossingEventSDL2(true, (float)mx, (float)my);
                ev->DispatchToWindow(win);
                delete ev;
                break;
            }
            case SDL_WINDOWEVENT_LEAVE: {
                int mx, my;
                SDL_GetMouseState(&mx, &my);
                auto ev = new MoonCrossingEventSDL2(false, (float)mx, (float)my);
                ev->DispatchToWindow(win);
                delete ev;
                break;
            }
            case SDL_WINDOWEVENT_FOCUS_GAINED: {
                auto ev = new MoonFocusEventSDL2(true);
                ev->DispatchToWindow(win);
                delete ev;
                break;
            }
            case SDL_WINDOWEVENT_FOCUS_LOST: {
                auto ev = new MoonFocusEventSDL2(false);
                ev->DispatchToWindow(win);
                delete ev;
                break;
            }
            case SDL_WINDOWEVENT_RESIZED:
            case SDL_WINDOWEVENT_SIZE_CHANGED:
                win->Resize(event.window.data1, event.window.data2);
                break;
            case SDL_WINDOWEVENT_EXPOSED:
                win->ProcessUpdates();
                break;
            case SDL_WINDOWEVENT_CLOSE:
                if (win->GetQuitOnClose())
                    running = false;
                break;
            }
            break;
        }
        }
    }
}

guint32
MoonWindowingSystemSDL2::GetScreenHeight(MoonWindow *moon_window) {
    SDL_Rect bounds;
    if (SDL_GetDisplayBounds(0, &bounds) == 0)
        return bounds.h;
    return 0;
}

guint32
MoonWindowingSystemSDL2::GetScreenWidth(MoonWindow *moon_window) {
    SDL_Rect bounds;
    if (SDL_GetDisplayBounds(0, &bounds) == 0)
        return bounds.w;
    return 0;
}

gchar *MoonWindowingSystemSDL2::GetTemporaryFolder() {
    return g_strdup(g_get_tmp_dir());
}

gchar *MoonWindowingSystemSDL2::GetUserConfigFolder() {
    char *data = SDL_GetPrefPath("moonlight", "moonlight");
    char *ret = g_strdup(data);
    SDL_free(data);
    return ret;
}
