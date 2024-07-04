/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#include <config.h>
#include <string.h>

#include "pal-sdl2.h"
#include "runtime.h"
#include "pixbuf-sdl2.h"

#include <Shlwapi.h>
#include <wincodec.h>
#include <wrl.h>

using namespace Moonlight;
using namespace Microsoft::WRL;

MoonPixbufLoaderSDL2::MoonPixbufLoaderSDL2(const char *imageType) {
    crc_error = false;
    data = g_byte_array_new();
    image_type = imageType;
    offset = 0;
    pixbuf = NULL;
    pWICFactory.Reset();

    HRESULT hr = CoCreateInstance(CLSID_WICImagingFactory, NULL, CLSCTX_INPROC_SERVER, IID_PPV_ARGS(&pWICFactory));
    if (FAILED(hr))
        g_warning("CoCreateInstance CLSID_WICImagingFactory failed!");
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
    // g_warning ("MPLA::Close ()");
}

MoonPixbuf *MoonPixbufLoaderSDL2::GetPixbuf() {
    // g_warning ("MPLA::GetPixbuf ()");
    if (pixbuf != NULL)
        return pixbuf;

    if (pWICFactory == NULL)
        return NULL;

    ComPtr<IWICBitmapSource> source;
    ComPtr<IWICBitmapDecoder> decoder;
    ComPtr<IWICBitmapFrameDecode> frame;
    ComPtr<IStream> stream = SHCreateMemStream(data->data, data->len);
    HRESULT hr = pWICFactory->CreateDecoderFromStream(stream.Get(), NULL, WICDecodeMetadataCacheOnDemand, decoder.GetAddressOf());
    if (FAILED(hr))
        return NULL;

    hr = decoder->GetFrame(0, frame.GetAddressOf());
    if (FAILED(hr))
        return NULL;

    hr = WICConvertBitmapSource(GUID_WICPixelFormat32bppPBGRA, frame.Get(), source.GetAddressOf());
    if (FAILED(hr))
        return NULL;

    pixbuf = new MoonPixbufSDL2(source.Get(), false);
    return pixbuf;
}

MoonPixbufSDL2::MoonPixbufSDL2(IWICBitmapSource *source, bool crc_error) {
    pBitmapSource = source;
    pBitmapSource->GetSize(&width, &height);

    UINT size = width * height * 4;
    stride = width * 4;
    data = new BYTE[size];

    pBitmapSource->CopyPixels(NULL, stride, size, data);
}

MoonPixbufSDL2::~MoonPixbufSDL2() {
    delete[] data;

    if (hBitmap != NULL)
        DeleteObject(hBitmap);
}

gint MoonPixbufSDL2::GetWidth() {
    return (gint)width;
}

gint MoonPixbufSDL2::GetHeight() {
    return (gint)height;
}

gint MoonPixbufSDL2::GetRowStride() {
    return (gint)stride;
}

gint MoonPixbufSDL2::GetNumChannels() {
    return 4;
}

guchar *MoonPixbufSDL2::GetPixels() {
    return (guchar *)data;
}

gboolean MoonPixbufSDL2::IsPremultiplied() {
    return TRUE;
}

// Returns a HBITMAP
gpointer MoonPixbufSDL2::GetPlatformPixbuf() {
    if (pBitmapSource == NULL)
        return NULL;

    if (this->hBitmap != NULL)
        return this->hBitmap;

    return this->hBitmap = CreateBitmap(width, height, 1, 32, data);
}
