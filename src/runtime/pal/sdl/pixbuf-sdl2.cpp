/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#include <config.h>
#include <string.h>

#include <SDL.h>
#include <SDL_image.h>

#include "pal-sdl2.h"
#include "runtime.h"
#include "pixbuf-sdl2.h"

using namespace Moonlight;

MoonPixbufLoaderSDL2::MoonPixbufLoaderSDL2(const char *imageType) {
    crc_error = false;
    data = g_byte_array_new();
    image_type = imageType;
    offset = 0;
    pixbuf = NULL;
}

MoonPixbufLoaderSDL2::MoonPixbufLoaderSDL2() : MoonPixbufLoaderSDL2(nullptr) {
}

MoonPixbufLoaderSDL2::~MoonPixbufLoaderSDL2() {
    g_byte_array_free(data, true);
    data = NULL;
}

void MoonPixbufLoaderSDL2::Write(const guchar *buffer, int buflen, MoonError **error) {
    g_byte_array_append(data, buffer, buflen);
}

void MoonPixbufLoaderSDL2::Close(MoonError **error) {
    if (data->len == 0)
        return;

    SDL_RWops* rw = SDL_RWFromMem(data->data, (int)data->len);
    if (!rw) {
        if (error)
            *error = new MoonError(MoonError::EXCEPTION, 4001, SDL_GetError());
        return;
    }

    // freesrc=1: SDL_image closes rw when done
    SDL_Surface* surface = IMG_Load_RW(rw, 1);
    if (!surface) {
        if (error)
            *error = new MoonError(MoonError::EXCEPTION, 4001, IMG_GetError());
        return;
    }

    // SDL_PIXELFORMAT_RGBA32 is always RGBA bytes in memory regardless of endianness
    SDL_Surface* converted = SDL_ConvertSurfaceFormat(surface, SDL_PIXELFORMAT_RGBA32, 0);
    SDL_FreeSurface(surface);
    if (!converted) {
        if (error)
            *error = new MoonError(MoonError::EXCEPTION, 4001, SDL_GetError());
        return;
    }

    guint w = (guint)converted->w;
    guint h = (guint)converted->h;
    guint stride = (guint)converted->pitch;
    guchar* pixels = new guchar[h * stride];
    memcpy(pixels, converted->pixels, h * stride);
    SDL_FreeSurface(converted);

    pixbuf = new MoonPixbufSDL2(pixels, w, h, stride, crc_error);
}

MoonPixbuf *MoonPixbufLoaderSDL2::GetPixbuf() {
    if (pixbuf != NULL)
        return pixbuf;

    return NULL;
}

MoonPixbufSDL2::MoonPixbufSDL2(guchar *pixels, guint width, guint height, guint stride, bool crc_error) {
    this->pixels = pixels;
    this->width = width;
    this->height = height;
    this->stride = stride;
    this->crc_error = crc_error;
}

MoonPixbufSDL2::~MoonPixbufSDL2() {
    delete[] pixels;
}

gint MoonPixbufSDL2::GetWidth() {
    return crc_error ? 1 : (gint)width;
}

gint MoonPixbufSDL2::GetHeight() {
    return crc_error ? 1 : (gint)height;
}

gint MoonPixbufSDL2::GetRowStride() {
    return crc_error ? 4 : (gint)stride;
}

gint MoonPixbufSDL2::GetNumChannels() {
    return 4;
}

guchar *MoonPixbufSDL2::GetPixels() {
    if (crc_error)
        return (guchar *)g_malloc0(4);
    return pixels;
}

gboolean MoonPixbufSDL2::IsPremultiplied() {
    return FALSE;
}

gpointer MoonPixbufSDL2::GetPlatformPixbuf() {
    return NULL;
}
