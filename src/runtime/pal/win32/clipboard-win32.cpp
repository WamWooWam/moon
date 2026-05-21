#include "clipboard-win32.h"

Moonlight::MoonClipboardWin32::MoonClipboardWin32(MoonWindowWin32 *win, MoonClipboardType clipboardType) {
    this->hwnd = (HWND)win->GetPlatformWindow();
}

bool Moonlight::MoonClipboardWin32::ContainsText() {
    if (IsClipboardFormatAvailable(CF_TEXT) || IsClipboardFormatAvailable(CF_UNICODETEXT)) {
        return true;
    }

    return false;
}

void Moonlight::MoonClipboardWin32::SetText(const char *text) {
    wchar_t *textW = reinterpret_cast<wchar_t *>(g_utf8_to_utf16(text, -1, NULL, NULL, NULL));
    if (textW == NULL)
        return;

    if (!OpenClipboard(hwnd)) {
        g_free(textW);
        return;
    }

    EmptyClipboard();

    // Only place CF_UNICODETEXT: Windows synthesizes CF_TEXT/CF_OEMTEXT from
    // it on demand, which avoids corrupting non-ASCII text (the old code
    // copied raw UTF-8 bytes straight into CF_TEXT).
    size_t bytes = (wcslen(textW) + 1) * sizeof(wchar_t);
    HGLOBAL hglbCopyW = GlobalAlloc(GMEM_MOVEABLE, bytes);
    if (hglbCopyW == NULL) {
        CloseClipboard();
        g_free(textW);
        return;
    }

    wchar_t *lptstrCopyW = (wchar_t *)GlobalLock(hglbCopyW);
    memcpy(lptstrCopyW, textW, bytes);
    GlobalUnlock(hglbCopyW);

    if (SetClipboardData(CF_UNICODETEXT, hglbCopyW) == NULL) {
        // ownership wasn't transferred; free our copy
        GlobalFree(hglbCopyW);
    }

    CloseClipboard();
    g_free(textW);
}

void Moonlight::MoonClipboardWin32::AsyncGetText(MoonClipboardGetTextCallback cb, gpointer data) {
    char* text = GetText();
    cb(this, text, data);
    g_free(text);
}

char *Moonlight::MoonClipboardWin32::GetText() {
    if (!OpenClipboard(hwnd)) {
        return NULL;
    }

    HANDLE hData = GetClipboardData(CF_UNICODETEXT);
    if (hData == NULL) {
        CloseClipboard();
        return NULL;
    }

    wchar_t *lptstr = (wchar_t *)GlobalLock(hData);
    if (lptstr == NULL) {
        CloseClipboard();
        return NULL;
    }

    char *text = g_utf16_to_utf8(reinterpret_cast<gunichar2 *>(lptstr), -1, NULL, NULL, NULL);

    GlobalUnlock(hData);
    CloseClipboard();

    return text;
}
