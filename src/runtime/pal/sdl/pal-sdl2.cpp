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

        // reverse source1 and source2 here from above, since lower
        // priority values represent higher priorities
        return source2->priority - source1->priority;
    }

    // this one must be signed
    gint32 time_remaining;

    bool pending_destroy;
    guint source_id;
    int priority;
    gint32 interval;
    MoonSourceFunc source_func;
    gpointer data;
};

/// our windowing system KKomrade

enum class MoonWindowCreateType {
    Message,
    Regular,
    Offscreen
};

struct MoonCreateParams {
    MoonWindowCreateType type;
    void *data;
};

MoonWindowingSystemSDL2::MoonWindowingSystemSDL2(bool out_of_browser)
    : sourceMutex(false) {
    source_id = 1;
    sources = NULL;
    timer = NULL;
    before = -1;
    emitting_sources = false;
    pool = 0;
    stride = 0;

    SDL_Init(SDL_INIT_VIDEO | SDL_INIT_TIMER | SDL_INIT_AUDIO);
}

MoonWindowingSystemSDL2::~MoonWindowingSystemSDL2() {
}

void MoonWindowingSystemSDL2::ShowCodecsUnavailableMessage() {
    // FIXME
}

cairo_surface_t *MoonWindowingSystemSDL2::CreateSurface() {
    // FIXME...
    g_assert_not_reached();
}

void MoonWindowingSystemSDL2::ExitApplication() {
    SDL_Quit();
}


MoonWindow *MoonWindowingSystemSDL2::CreateWindow(MoonWindowType windowType, int width, int height, MoonWindow *parentWindow, Surface *surface) {
    MoonWindowSDL2 *window = new MoonWindowSDL2(windowType, width, height, parentWindow, surface, this);
    return window;
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
}

void MoonWindowingSystemSDL2::UnregisterWindow(MoonWindow *window) {
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
    // FIXME
    return NULL;
}

MoonModifier
MoonWindowingSystemSDL2::GetCommandModifier() {
    return MoonModifier_Control;
}

guint MoonWindowingSystemSDL2::GetCursorBlinkTimeout(MoonWindow *moon_window) {
    return GetCaretBlinkTime();
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
                    // first time through here, so we do what glib does, and limit the sources we
                    // dispatch on to those at or above this priority.
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

    emitting_sources = false;
    sourceMutex.Unlock();

    AddSDL2Timer();
}

void MoonWindowingSystemSDL2::AddSDL2Timer() {
    int timeout = -1;

    sourceMutex.Lock();
    if (timer != NULL) {
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
        // timer = (gpointer)SetTimer(message_window, MoonSDL2TimerId, timeout, nullptr);
        timer = SDL_AddTimer(
            timeout, [](Uint32 interval, void *param) -> Uint32 {
                SDL_PushEvent(new SDL_Event{ SDL_USEREVENT });
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

    SDL_Event event;
    while (SDL_WaitEvent(&event)) {
        if (event.type == SDL_QUIT) {
            break;
        }
        else if (event.type == SDL_USEREVENT) {
            OnTick();
        }
    }
}

guint32
MoonWindowingSystemSDL2::GetScreenHeight(MoonWindow *moon_window) {
    return -1;
}

guint32
MoonWindowingSystemSDL2::GetScreenWidth(MoonWindow *moon_window) {
    return -1;
}


gchar *MoonWindowingSystemSDL2::GetTemporaryFolder() {
    return NULL;
}

gchar *MoonWindowingSystemSDL2::GetUserConfigFolder() {
    return NULL;
}
