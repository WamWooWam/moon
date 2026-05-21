#include "clipboard-sdl2.h"
#include <SDL.h>

Moonlight::MoonClipboardSDL2::MoonClipboardSDL2(MoonWindowSDL2 *win, MoonClipboardType clipboardType) {
}

bool Moonlight::MoonClipboardSDL2::ContainsText() {
    return SDL_HasClipboardText() == SDL_TRUE;
}

void Moonlight::MoonClipboardSDL2::SetText(const char *text) {
    SDL_SetClipboardText(text);
}

void Moonlight::MoonClipboardSDL2::AsyncGetText(MoonClipboardGetTextCallback cb, gpointer data) {
    char *text = GetText();
    cb(this, text, data);
    if (text)
        g_free(text);
}

char *Moonlight::MoonClipboardSDL2::GetText() {
    if (!SDL_HasClipboardText())
        return NULL;
    // SDL_GetClipboardText returns memory that must be freed with SDL_free;
    // callers expect g_malloc'd memory, so copy it.
    char *sdl_text = SDL_GetClipboardText();
    if (!sdl_text)
        return NULL;
    char *result = g_strdup(sdl_text);
    SDL_free(sdl_text);
    return result;
}
