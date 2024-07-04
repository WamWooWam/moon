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
    if (!OpenClipboard(hwnd)) {
        return;
    }

    EmptyClipboard();

    const wchar_t *textW = reinterpret_cast<wchar_t *>(g_utf8_to_utf16(text, -1, NULL, NULL, NULL));

    HGLOBAL hglbCopy = GlobalAlloc(GMEM_MOVEABLE, strlen(text) + 1);
    HGLOBAL hglbCopyW = GlobalAlloc(GMEM_MOVEABLE, (wcslen(textW) + 1) * sizeof(wchar_t));
    if (hglbCopy == NULL || hglbCopyW == NULL) {
        CloseClipboard();
        return;
    }

    char *lptstrCopy = (char *)GlobalLock(hglbCopy);
    wchar_t *lptstrCopyW = (wchar_t *)GlobalLock(hglbCopyW);

    strcpy(lptstrCopy, text);
    wcscpy(lptstrCopyW, textW);

    GlobalUnlock(hglbCopy);
    GlobalUnlock(hglbCopyW);

    // BUGBUG: smushing UTF-8 into CF_TEXT is not correct, but it's what we do for now
    SetClipboardData(CF_TEXT, hglbCopy);
    SetClipboardData(CF_UNICODETEXT, hglbCopyW);

    CloseClipboard();
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
