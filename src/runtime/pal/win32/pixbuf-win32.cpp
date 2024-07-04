/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#include <config.h>
#include <string.h>

#include "pal-win32.h"
#include "runtime.h"
#include "pixbuf-win32.h"

#include <Shlwapi.h>
#include <wincodec.h>
#include <wrl.h>

using namespace Moonlight;
using namespace Microsoft::WRL;

MoonPixbufLoaderWin32::MoonPixbufLoaderWin32(const char *imageType) {
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

MoonPixbufLoaderWin32::MoonPixbufLoaderWin32() : MoonPixbufLoaderWin32(nullptr) {
}

MoonPixbufLoaderWin32::~MoonPixbufLoaderWin32() {
    g_byte_array_free(data, true);
    data = NULL;
}

void MoonPixbufLoaderWin32::Write(const guchar *buffer, int buflen, MoonError **error) {
    g_byte_array_append(data, buffer, buflen);
}

void MoonPixbufLoaderWin32::Close(MoonError **error) {
    // g_warning ("MPLA::Close ()");
}

MoonPixbuf *MoonPixbufLoaderWin32::GetPixbuf() {
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

    pixbuf = new MoonPixbufWin32(source.Get(), false);
    return pixbuf;
}

MoonPixbufWin32::MoonPixbufWin32(IWICBitmapSource *source, bool crc_error) {
    pBitmapSource = source;
    pBitmapSource->GetSize(&width, &height);

    UINT size = width * height * 4;
    stride = width * 4;
    data = new BYTE[size];

    pBitmapSource->CopyPixels(NULL, stride, size, data);
}

MoonPixbufWin32::~MoonPixbufWin32() {
    delete[] data;

    if (hBitmap != NULL)
        DeleteObject(hBitmap);
}

gint MoonPixbufWin32::GetWidth() {
    return (gint)width;
}

gint MoonPixbufWin32::GetHeight() {
    return (gint)height;
}

gint MoonPixbufWin32::GetRowStride() {
    return (gint)stride;
}

gint MoonPixbufWin32::GetNumChannels() {
    return 4;
}

guchar *MoonPixbufWin32::GetPixels() {
    return (guchar *)data;
}

gboolean MoonPixbufWin32::IsPremultiplied() {
    return TRUE;
}

// Returns a HBITMAP
gpointer MoonPixbufWin32::GetPlatformPixbuf() {
    if (pBitmapSource == NULL)
        return NULL;

    if (this->hBitmap != NULL)
        return this->hBitmap;

    return this->hBitmap = CreateBitmap(width, height, 1, 32, data);
}
