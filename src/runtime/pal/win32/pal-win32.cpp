/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#include <config.h>

#include <string>

#include "pal-win32.h"

#include "runtime.h"
#include "window-win32.h"
#include "pixbuf-win32.h"
#include "im-win32.h"
#include "debug.h"

#include <glib.h>
#include <wrl.h>
#include <commctrl.h>
#include <shobjidl.h>
#include <shlwapi.h>
#include <shlobj.h>
#include <timeapi.h>

#include "helpers/dialogeventhandler.h"

using namespace Moonlight;
using namespace Microsoft::WRL;


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

MoonWindowingSystemWin32::MoonWindowingSystemWin32(bool out_of_browser)
    : sourceMutex(false) {

    // LoadSystemColors();

    WNDCLASSEX wndClass{};
    wndClass.cbSize = sizeof(WNDCLASSEX);
    wndClass.hCursor = LoadCursor(NULL, IDC_ARROW);
    wndClass.lpfnWndProc = MoonWindowingSystemWin32::WndProc;
    wndClass.hInstance = GetModuleHandle(NULL);
    wndClass.lpszClassName = L"Moon_Window";
    wndClass.style = CS_HREDRAW | CS_VREDRAW;
    RegisterClassEx(&wndClass);

    WNDCLASSEX msgWndClass{};
    msgWndClass.cbSize = sizeof(WNDCLASSEX);
    msgWndClass.lpfnWndProc = MoonWindowingSystemWin32::WndProc;
    msgWndClass.lpszClassName = L"Moon_MessageWindow";
    msgWndClass.hInstance = GetModuleHandle(nullptr);
    RegisterClassEx(&msgWndClass);

    auto params = new MoonCreateParams();
    params->type = MoonWindowCreateType::Message;
    params->data = this;
    message_window = CreateWindowEx(0, L"Moon_MessageWindow", nullptr, 0, 0, 0, 0, 0, HWND_MESSAGE, nullptr, GetModuleHandle(nullptr), params);

    source_id = 1;
    sources = NULL;
    timer = NULL;
    before = -1;
    emitting_sources = false;
    pool = 0;
    stride = 0;
}

MoonWindowingSystemWin32::~MoonWindowingSystemWin32() {
    // for (int i = 0; i < (int)NumSystemColors; i++)
    //     delete system_colors[i];
}

void MoonWindowingSystemWin32::ShowCodecsUnavailableMessage() {
    // FIXME
}

cairo_surface_t *MoonWindowingSystemWin32::CreateSurface() {
    // FIXME...
    g_assert_not_reached();
}

void MoonWindowingSystemWin32::ExitApplication() {
    PostQuitMessage(0);
}


MoonWindow *MoonWindowingSystemWin32::CreateWindow(MoonWindowType windowType, int width, int height, MoonWindow *parentWindow, Surface *surface) {  
    DWORD dwStyle = 0;
    int ix = CW_USEDEFAULT;
    int iy = CW_USEDEFAULT;
    int iw = width;
    int ih = height;

    switch (windowType) {
    case MoonWindowType_Desktop: {
        dwStyle = WS_OVERLAPPEDWINDOW;
        break;
    }
    case MoonWindowType_FullScreen: {
        MONITORINFO info{ sizeof(MONITORINFO) };
        HMONITOR monitor = MonitorFromWindow((HWND)parentWindow->GetPlatformWindow(), MONITOR_DEFAULTTONEAREST);
        GetMonitorInfo(monitor, &info);

        dwStyle = WS_POPUP;
        ix = info.rcMonitor.left;
        iy = info.rcMonitor.top;
        iw = info.rcMonitor.right - info.rcMonitor.left;
        ih = info.rcMonitor.bottom - info.rcMonitor.top;
        break;
    }
    case MoonWindowType_Plugin:
    default: {
        // nop
        break;
    }
    }

    MoonWindowWin32 *window = new MoonWindowWin32(windowType, iw, ih, parentWindow, surface, this);
    MoonCreateParams *params = new MoonCreateParams();
    params->type = MoonWindowCreateType::Regular;
    params->data = window;
    HWND hwnd = CreateWindowEx(
        0,
        L"Moon_Window",
        L"Moon_Window",
        dwStyle,
        ix,
        iy,
        iw,
        ih,
        parentWindow != nullptr ? (HWND)parentWindow->GetPlatformWindow() : nullptr,
        NULL,
        GetModuleHandle(NULL),
        params);

    if (hwnd == NULL) {
        delete window;
        return 0;
    }

    // window->Show();

    return window;
}

MoonWindow *MoonWindowingSystemWin32::CreateWindowless(int width, int height, PluginInstance *forPlugin) {
    g_warning("no windowless support on Win32");
    return NULL;
}

wchar_t *MoonWindowingSystemWin32::ToWString(const char *src) {
    if (!src)
        return nullptr;

    auto len = strlen(src);
    auto size = MultiByteToWideChar(CP_UTF8, 0, src, len, NULL, NULL);

    wchar_t *buffer = g_new(wchar_t, size + 1);
    if (!buffer)
        return nullptr;

    MultiByteToWideChar(CP_UTF8, 0, src, len, buffer, size);
    buffer[len] = L'\0';

    return buffer;
}

char *MoonWindowingSystemWin32::ToString(const wchar_t *src) {
    if (!src)
        return nullptr;

    auto len = lstrlenW(src);
    auto size = WideCharToMultiByte(CP_UTF8, 0, src, len, NULL, 0, NULL, NULL);

    char *buffer = g_new(char, size + 1);
    if (!buffer)
        return nullptr;

    WideCharToMultiByte(CP_UTF8, 0, src, len, buffer, size, NULL, NULL);
    buffer[size] = '\0';

    return buffer;
}

MoonMessageBoxResult MoonWindowingSystemWin32::ShowMessageBox(MoonMessageBoxType message_type, const char *caption, const char *text, MoonMessageBoxButton button) {
    wchar_t *w_caption = ToWString(caption);
    wchar_t *w_text = ToWString(text);

    auto window = Deployment::GetCurrent()->GetSurface()->GetNormalWindow();

    TASKDIALOGCONFIG config{};
    config.cbSize = sizeof(TASKDIALOGCONFIG);
    config.hwndParent = (HWND)window->GetPlatformWindow(); // TODO: fix this
    config.hInstance = GetModuleHandle(NULL);

    config.pszWindowTitle = L"Moonlight";
    config.pszMainInstruction = w_caption;
    config.pszContent = w_text;

    switch (button) {
    case MoonMessageBoxButton::MessageBoxButtonOk:
        config.dwCommonButtons = TDCBF_OK_BUTTON;
        break;
    case MoonMessageBoxButton::MessageBoxButtonOkCancel:
        config.dwCommonButtons = TDCBF_OK_BUTTON | TDCBF_CANCEL_BUTTON;
        break;
    case MoonMessageBoxButton::MessageBoxButtonYesNo:
        config.dwCommonButtons = TDCBF_YES_BUTTON | TDCBF_NO_BUTTON;
        break;
    }

    switch (message_type) {
    case MoonMessageBoxType::MessageBoxTypeInfo:
        config.hMainIcon = LoadIcon(NULL, IDI_INFORMATION);
        break;
    case MoonMessageBoxType::MessageBoxTypeWarning:
        config.hMainIcon = LoadIcon(NULL, IDI_WARNING);
        break;
    case MoonMessageBoxType::MessageBoxTypeQuestion:
        config.hMainIcon = LoadIcon(NULL, IDI_QUESTION);
        break;
    }

    config.dwFlags = TDF_USE_HICON_MAIN;

    int btn;
    TaskDialogIndirect(&config, &btn, NULL, NULL);

    if (w_text)
        g_free(w_text);
    if (w_caption)
        g_free(w_caption);

    return (MoonMessageBoxResult)btn;
}

HRESULT MoonWindowingSystemWin32::SetDialogFilters(ComPtr<IFileDialog> &chooser, const char *filter, int idx) {
    if (!filter || (strlen(filter) <= 1))
        return S_OK;
    char **filters = g_strsplit(filter, "|", 0);

    // to be valid (managed code) we know we have an even number of items
    // (if not we're still safe by dropping the last one)
    int pos = 0;
    int n = g_strv_length(filters) >> 1;

    int numFilter = 0;
    auto dlgFilters = new COMDLG_FILTERSPEC[n];
    memset(dlgFilters, 0, sizeof(COMDLG_FILTERSPEC) * n);

    for (int i = 0; i < n; i++) {
        char *name = g_strstrip(filters[pos++]);
        if (strlen(name) < 1)
            continue;

        char *pattern = g_strstrip(filters[pos++]);
        if (strlen(pattern) < 1)
            continue;

        dlgFilters[numFilter] = { ToWString(name), ToWString(pattern) };
        numFilter++;
    }

    HRESULT hr = chooser->SetFileTypes(numFilter, dlgFilters);
    if (SUCCEEDED(hr)) {
        chooser->SetFileTypeIndex(idx);
    }

    g_strfreev(filters);
    for (int i = 0; i < numFilter; i++) {
        if (dlgFilters[i].pszName) {
            g_free((void *)dlgFilters[i].pszName);
        }
        if (dlgFilters[i].pszSpec) {
            g_free((void *)dlgFilters[i].pszSpec);
        }
    }
    delete[] dlgFilters;
    return hr;
}

char **MoonWindowingSystemWin32::ShowOpenFileDialog(const char *title, bool multsel, const char *filter, int idx) {

    auto window = Deployment::GetCurrent()->GetSurface()->GetNormalWindow();

    ComPtr<IFileDialog> rawFd;
    ComPtr<IFileOpenDialog> pfd;
    ComPtr<IFileDialogEvents> pfde;
    ComPtr<IShellItemArray> psiaResult;

    HRESULT hr = CoCreateInstance(CLSID_FileOpenDialog, NULL, CLSCTX_INPROC_SERVER, IID_IFileOpenDialog, (void **)pfd.GetAddressOf());
    if (FAILED(hr))
        return nullptr;

    hr = pfd.As<IFileDialog>(&rawFd);
    if (FAILED(hr))
        return nullptr;

    hr = CDialogEventHandler_CreateInstance(IID_IFileDialogEvents, (void **)pfde.GetAddressOf());
    if (FAILED(hr))
        return nullptr;

    DWORD cookie;
    hr = pfd->Advise(pfde.Get(), &cookie);
    if (FAILED(hr))
        return nullptr;

    DWORD dwFlags;
    hr = pfd->GetOptions(&dwFlags);
    if (FAILED(hr))
        return nullptr;

    hr = pfd->SetOptions(dwFlags | FOS_FORCEFILESYSTEM | (multsel ? FOS_ALLOWMULTISELECT : 0));
    if (FAILED(hr))
        return nullptr;

    hr = SetDialogFilters(rawFd, filter, idx);
    if (FAILED(hr))
        return nullptr;

    hr = pfd->Show((HWND)window->GetPlatformWindow());
    if (FAILED(hr))
        return nullptr;

    hr = pfd->GetResults(psiaResult.GetAddressOf());
    if (FAILED(hr))
        return nullptr;

    gchar **ret = NULL;
    {
        PWSTR pszFilePath = NULL;
        DWORD dwNumItems = 0;
        hr = psiaResult->GetCount(&dwNumItems);
        if (FAILED(hr))
            return nullptr;

        ret = g_new(gchar *, dwNumItems + 1);
        ret[dwNumItems] = NULL;

        for (DWORD i = 0; i < dwNumItems; i++) {
            ComPtr<IShellItem> psi;
            hr = psiaResult->GetItemAt(i, &psi);

            if (FAILED(hr))
                continue;

            hr = psi->GetDisplayName(SIGDN_FILESYSPATH, &pszFilePath);
            if (SUCCEEDED(hr)) {
                ret[i] = ToString(pszFilePath);
                CoTaskMemFree(pszFilePath);
            }
        }
    }

    return ret;
}

char *MoonWindowingSystemWin32::ShowSaveFileDialog(const char *title, const char *filter, int idx) {
    auto window = Deployment::GetCurrent()->GetSurface()->GetNormalWindow();

    ComPtr<IFileDialog> pfd;
    ComPtr<IFileDialogEvents> pfde;
    ComPtr<IShellItem> psiResult;

    HRESULT hr = CoCreateInstance(CLSID_FileSaveDialog, NULL, CLSCTX_INPROC_SERVER, IID_IFileDialog, (void **)pfd.GetAddressOf());
    if (FAILED(hr))
        return nullptr;

    hr = CDialogEventHandler_CreateInstance(IID_IFileDialogEvents, (void **)pfde.GetAddressOf());
    if (FAILED(hr))
        return nullptr;

    DWORD cookie;
    hr = pfd->Advise(pfde.Get(), &cookie);
    if (FAILED(hr))
        return nullptr;

    DWORD dwFlags;
    hr = pfd->GetOptions(&dwFlags);
    if (FAILED(hr))
        return nullptr;

    hr = pfd->SetOptions(dwFlags | FOS_FORCEFILESYSTEM);
    if (FAILED(hr))
        return nullptr;

    hr = SetDialogFilters(pfd, filter, idx);
    if (FAILED(hr))
        return nullptr;

    hr = pfd->Show((HWND)window->GetPlatformWindow());
    if (FAILED(hr))
        return nullptr;

    hr = pfd->GetResult(psiResult.GetAddressOf());
    if (FAILED(hr))
        return nullptr;

    gchar *ret = nullptr;
    LPWSTR pszFilePath = NULL;
    hr = psiResult->GetDisplayName(SIGDN_FILESYSPATH, &pszFilePath);
    if (SUCCEEDED(hr)) {
        ret = ToString(pszFilePath);
        CoTaskMemFree(pszFilePath);
    }

    return ret;
}

bool MoonWindowingSystemWin32::ShowConsentDialog(const char *question, const char *detail, const char *website, bool *remember) {
    wchar_t *w_question = ToWString(question);
    wchar_t *w_detail = ToWString(detail);
    wchar_t *w_website = ToWString(website);

    auto window = Deployment::GetCurrent()->GetSurface()->GetNormalWindow();

    TASKDIALOGCONFIG config{};
    config.cbSize = sizeof(TASKDIALOGCONFIG);
    config.hwndParent = (HWND)window->GetPlatformWindow(); // TODO: fix this
    config.hInstance = GetModuleHandle(NULL);

    config.pszWindowTitle = L"Moonlight";
    config.pszMainInstruction = w_question;
    config.pszContent = w_detail;
    config.pszFooter = w_website;
    config.pszVerificationText = L"Don't ask me again";
    config.dwCommonButtons = TDCBF_YES_BUTTON | TDCBF_NO_BUTTON;
    config.hMainIcon = LoadIcon(NULL, IDI_QUESTION);

    config.dwFlags = TDF_USE_HICON_MAIN;

    int btn;
    BOOL bRemember;
    TaskDialogIndirect(&config, &btn, NULL, &bRemember);

    if (w_question)
        g_free(w_question);
    if (w_detail)
        g_free(w_detail);

    *remember = bRemember != 0;
    return btn == IDYES;
}

void MoonWindowingSystemWin32::RegisterWindow(MoonWindow *window) {
}

void MoonWindowingSystemWin32::UnregisterWindow(MoonWindow *window) {
}

Color *MoonWindowingSystemWin32::GetSystemColor(SystemColor id) {
    if (id < 0 || id >= (int)NumSystemColors)
        return NULL;

    return new Color(GetSysColor(id));
}

class Win32Source {
public:
    Win32Source(int source_id, int priority, int interval, MoonSourceFunc source_func, gpointer data) {
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
        const Win32Source *source1 = (const Win32Source *)p1;
        const Win32Source *source2 = (const Win32Source *)p2;

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


guint MoonWindowingSystemWin32::AddTimeout(gint priority, gint ms, MoonSourceFunc timeout, gpointer data) {
    sourceMutex.Lock();

    int new_source_id = source_id;

    Win32Source *new_source = new Win32Source(new_source_id, priority, ms, timeout, data);
    sources = g_list_insert_sorted(sources, new_source, Win32Source::Compare);
    source_id++;

    sourceMutex.Unlock();

    AddWin32Timer();

    return new_source_id;
}

void MoonWindowingSystemWin32::RemoveSource(guint sourceId) {
    sourceMutex.Lock();

    for (GList *l = sources; l; l = l->next) {
        Win32Source *s = (Win32Source *)l->data;
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

void MoonWindowingSystemWin32::RemoveTimeout(guint timeoutId) {
    RemoveSource(timeoutId);
}

guint MoonWindowingSystemWin32::AddIdle(MoonSourceFunc idle, gpointer data) {
    sourceMutex.Lock();

    int new_source_id = source_id;

    Win32Source *new_source = new Win32Source(new_source_id, MOON_PRIORITY_DEFAULT_IDLE, 0, idle, data);
    sources = g_list_insert_sorted(sources, new_source, Win32Source::Compare);
    source_id++;

    sourceMutex.Unlock();
    return new_source_id;
}

void MoonWindowingSystemWin32::RemoveIdle(guint idleId) {
    RemoveSource(idleId);
}

MoonIMContext *MoonWindowingSystemWin32::CreateIMContext() {
    return new MoonIMContextWin32();
}

MoonEvent *MoonWindowingSystemWin32::CreateEventFromPlatformEvent(gpointer platformEvent) {
    // FIXME
    return NULL;
}

MoonModifier
MoonWindowingSystemWin32::GetCommandModifier() {
    return MoonModifier_Control;
}

guint MoonWindowingSystemWin32::GetCursorBlinkTimeout(MoonWindow *moon_window) {
    return GetCaretBlinkTime();
}


MoonPixbufLoader *MoonWindowingSystemWin32::CreatePixbufLoader(const char *imageType) {
    return new MoonPixbufLoaderWin32(imageType);
}

// static WndProc callback
LRESULT CALLBACK MoonWindowingSystemWin32::WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam) {
    if (msg == WM_NCCREATE) {
        CREATESTRUCT *pCreate = reinterpret_cast<CREATESTRUCT *>(lParam);
        MoonCreateParams *params = reinterpret_cast<MoonCreateParams *>(pCreate->lpCreateParams);
        if (params != nullptr) {
            if (params->type == MoonWindowCreateType::Regular) {
                ((MoonWindowWin32 *)params->data)->SetHandle(hwnd);
            }

            SetWindowLongPtr(hwnd, GWLP_USERDATA, (LONG_PTR)params);
            return TRUE;
        }
    }

    MoonCreateParams *params = reinterpret_cast<MoonCreateParams *>(GetWindowLongPtr(hwnd, GWLP_USERDATA));
    if (params != nullptr) {
        // messages targeting the message window get sent to the Windowing System
        if (params->type == MoonWindowCreateType::Message) {
            MoonWindowingSystemWin32 *system = reinterpret_cast<MoonWindowingSystemWin32 *>(params->data);
            if (system != nullptr) {
                return system->WndProc(msg, wParam, lParam);
            }
        }
        else {
            // Otherwise they're forwareded to the window itself
            MoonWindowWin32 *window = reinterpret_cast<MoonWindowWin32 *>(params->data);
            if (window != nullptr)
                return window->WndProc(msg, wParam, lParam);
        }
    }

    return DefWindowProc(hwnd, msg, wParam, lParam);
}

// Windowing System instance WndProc callback
LRESULT MoonWindowingSystemWin32::WndProc(UINT msg, WPARAM wParam, LPARAM lParam) {
    if (msg == WM_TIMER && wParam == MoonWin32TimerId) {
        this->OnTick();
    }

    return DefWindowProc(message_window, msg, wParam, lParam);
}

static gint32 get_now_in_millis(void) {
    return timeGetTime();
}

void MoonWindowingSystemWin32::OnTick() {
    gint32 after = get_now_in_millis();

    sourceMutex.Lock();

    KillTimer(message_window, MoonWin32TimerId);
    timer = nullptr;

    emitting_sources = true;

    GList *sources_to_dispatch = NULL;
    if (sources) {
        int max_priority = G_MAXINT;
        gint32 delta = before - after;
        GList *l = sources;

        while (l) {
            Win32Source *s = (Win32Source *)l->data;

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
        Win32Source *s = (Win32Source *)l->data;
        if (!s->pending_destroy) {
            bool pending_destroy = !s->InvokeSourceFunc();
            if (!s->pending_destroy)
                s->pending_destroy = pending_destroy;
        }
    }

    g_list_free(sources_to_dispatch);

    sourceMutex.Lock();
    for (GList *l = sources; l;) {
        Win32Source *s = (Win32Source *)l->data;
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

    AddWin32Timer();
}

void MoonWindowingSystemWin32::AddWin32Timer() {
    int timeout = -1;

    sourceMutex.Lock();
    if (timer != NULL) {
        sourceMutex.Unlock();
        return;
    }
    if (sources != NULL) {
        Win32Source *s = (Win32Source *)sources->data;
        timeout = s->time_remaining;
        if (timeout < 0)
            timeout = 0;
    }

    if (timeout >= 0) {
        timer = (gpointer)SetTimer(message_window, MoonWin32TimerId, timeout, nullptr);
        before = get_now_in_millis();
    }

    sourceMutex.Unlock();
}

void MoonWindowingSystemWin32::RunMainLoop(MoonWindow *window, bool quit_on_window_close) {
    if (window) {
        reinterpret_cast<MoonWindowWin32 *>(window)->SetQuitOnClose(quit_on_window_close);
        window->Show();
    }

    AddWin32Timer();

    MSG msg;
    while (GetMessage(&msg, NULL, 0, 0) > 0) {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }
}

guint32
MoonWindowingSystemWin32::GetScreenHeight(MoonWindow *moon_window) {
    MONITORINFO monInfo{};
    HMONITOR mon = MonitorFromWindow((HWND)moon_window->GetPlatformWindow(), MONITOR_DEFAULTTONEAREST);
    GetMonitorInfoW(mon, &monInfo);

    return monInfo.rcWork.bottom - monInfo.rcWork.top;
}

guint32
MoonWindowingSystemWin32::GetScreenWidth(MoonWindow *moon_window) {
    MONITORINFO monInfo{};
    HMONITOR mon = MonitorFromWindow((HWND)moon_window->GetPlatformWindow(), MONITOR_DEFAULTTONEAREST);
    GetMonitorInfoW(mon, &monInfo);

    return monInfo.rcWork.right - monInfo.rcWork.left;
}

static gchar *g_TmpFolderPath = nullptr;

gchar *MoonWindowingSystemWin32::GetTemporaryFolder() {
    if (g_TmpFolderPath != nullptr)
        return g_TmpFolderPath;

    wchar_t tempPath[MAX_PATH + 1] = { 0 };
    DWORD length = GetTempPathW(MAX_PATH, tempPath);
    DWORD lengthNeeded = WideCharToMultiByte(CP_UTF8, 0, tempPath, length, NULL, 0, NULL, NULL);

    gchar *tempPathUtf8 = (gchar *)malloc((lengthNeeded + 1) * sizeof(gchar));
    WideCharToMultiByte(CP_UTF8, 0, tempPath, length, tempPathUtf8, lengthNeeded, NULL, NULL);
    tempPathUtf8[lengthNeeded] = '\0';

    return g_TmpFolderPath = tempPathUtf8;
}

gchar *MoonWindowingSystemWin32::GetUserConfigFolder() {
    wchar_t* tempPath;
    if (FAILED(SHGetKnownFolderPath(FOLDERID_LocalAppData, 0, NULL, &tempPath))) {
        g_assert_not_reached();
        return nullptr;
    }

    DWORD length = lstrlenW(tempPath);
    DWORD lengthNeeded = WideCharToMultiByte(CP_UTF8, 0, tempPath, length, NULL, 0, NULL, NULL);

    gchar *tempPathUtf8 = (gchar *)malloc((lengthNeeded + 1) * sizeof(gchar));
    WideCharToMultiByte(CP_UTF8, 0, tempPath, length, tempPathUtf8, lengthNeeded, NULL, NULL);
    tempPathUtf8[lengthNeeded] = '\0';

    CoTaskMemFree(tempPath);

    return tempPathUtf8;
}