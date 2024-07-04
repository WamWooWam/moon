#include "clipboard-sdl2.h"

Moonlight::MoonClipboardSDL2::MoonClipboardSDL2(MoonWindowSDL2 *win, MoonClipboardType clipboardType) {
}

bool Moonlight::MoonClipboardSDL2::ContainsText() {
    return false;
}

void Moonlight::MoonClipboardSDL2::SetText(const char *text) {
    
}

void Moonlight::MoonClipboardSDL2::AsyncGetText(MoonClipboardGetTextCallback cb, gpointer data) {
    char* text = GetText();
    cb(this, text, data);
    // g_free(text);
}

char *Moonlight::MoonClipboardSDL2::GetText() {
    return NULL;
}
