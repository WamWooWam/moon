/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#pragma once
#include "pal.h"

#include <SDL.h>

namespace Moonlight {

    constexpr int MoonSDL2TimerId = 6969;

    class MoonWindowingSystemSDL2 : public MoonWindowingSystem {
    public:
        MoonWindowingSystemSDL2(bool out_of_browser);
        virtual ~MoonWindowingSystemSDL2();

        // creates a platform/windowing system specific surface
        virtual cairo_surface_t *CreateSurface();
        virtual void ExitApplication();

        MoonWindow *CreateWindow(MoonWindowType windowType, int width, int height, MoonWindow *parentWindow, Surface *surface);
        MoonWindow *CreateWindowless(int width, int height, PluginInstance *forPlugin);

        virtual MoonMessageBoxResult ShowMessageBox(MoonMessageBoxType message_type, const char *caption, const char *text, MoonMessageBoxButton button);

        virtual bool ShowConsentDialog(const char *question, const char *detail, const char *website, bool *remember);

        virtual gchar **ShowOpenFileDialog(const char *title, bool multsel, const char *filter, int idx);
        virtual char *ShowSaveFileDialog(const char *title, const char *filter, int idx);

        virtual Color *GetSystemColor(SystemColor id);

        virtual guint AddTimeout(gint priority, gint ms, MoonSourceFunc timeout, gpointer data);
        virtual void RemoveTimeout(guint timeoutId);

        virtual guint AddIdle(MoonSourceFunc idle, gpointer data);
        virtual void RemoveIdle(guint idle_id);

        virtual MoonIMContext *CreateIMContext();

        virtual MoonEvent *CreateEventFromPlatformEvent(gpointer platformEvent);

        virtual MoonModifier GetCommandModifier();
        virtual guint GetCursorBlinkTimeout(MoonWindow *window);

        virtual MoonPixbufLoader *CreatePixbufLoader(const char *imageType);

        /* @GeneratePInvoke */
        virtual void RunMainLoop(MoonWindow *main_window = NULL, bool quit_on_window_close = true);

        void RegisterWindow(MoonWindow *window);
        void UnregisterWindow(MoonWindow *window);

        virtual void ShowCodecsUnavailableMessage();

        virtual guint32 GetScreenHeight(MoonWindow *moon_window);
        virtual guint32 GetScreenWidth(MoonWindow *moon_window);

        virtual gchar *GetTemporaryFolder();
        virtual gchar *GetUserConfigFolder();

    private:
        void OnTick();
        void AddSDL2Timer();
        void RemoveSource(guint sourceId);
        
        void *pool;
        int stride;

        // glib terminology here.  we mean "timeouts and idles"
        GList *sources;
        guint source_id;
        bool emitting_sources;
        MoonMutex sourceMutex;
        guint32 before;
        SDL_TimerID timer;
    };
};
