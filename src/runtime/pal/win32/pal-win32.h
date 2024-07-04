/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#pragma once
#include "pal.h"
#include <wrl.h>

struct IFileDialog;

namespace Moonlight {

    constexpr int MoonWin32TimerId = 6969;

    class MoonWindowingSystemWin32 : public MoonWindowingSystem {
    public:
        MoonWindowingSystemWin32(bool out_of_browser);
        virtual ~MoonWindowingSystemWin32();

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
        void AddWin32Timer();
        void RemoveSource(guint sourceId);
        wchar_t *ToWString(const char *str);
        char *ToString(const wchar_t *str);
        HRESULT SetDialogFilters(Microsoft::WRL::ComPtr<IFileDialog> &chooser, const char *filter, int idx);

        LRESULT WndProc(UINT msg, WPARAM wParam, LPARAM lParam);
        static LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam);

        void *pool;
        int stride;

        HWND message_window;

        // glib terminology here.  we mean "timeouts and idles"
        GList *sources;
        guint source_id;
        bool emitting_sources;
        MoonMutex sourceMutex;
        guint32 before;
        gpointer timer;
    };

    class MoonInstallerServiceWin32 : public MoonInstallerService {
        char *base_install_dir;

    protected:
        virtual const char *GetBaseInstallDir();

    public:
        MoonInstallerServiceWin32();
        virtual ~MoonInstallerServiceWin32();

        virtual bool Install(Deployment *deployment, bool unattended);
        virtual bool Uninstall(Deployment *deployment);
    };
};
