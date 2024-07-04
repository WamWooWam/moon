/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#ifndef MOON_PIXBUF_SDL2_H
#define MOON_PIXBUF_SDL2_H

#include "pal.h"
#include <wrl.h>

struct IWICImagingFactory;
struct IWICBitmapSource;

namespace Moonlight {

class MoonPixbufSDL2 : public MoonPixbuf {
public:
    MoonPixbufSDL2(IWICBitmapSource *image, bool crc_error);
	virtual ~MoonPixbufSDL2 ();

	virtual gint GetWidth ();
	virtual gint GetHeight ();
	virtual gint GetRowStride ();
	virtual gint GetNumChannels ();
	virtual guchar *GetPixels ();
	virtual gboolean IsPremultiplied ();

	virtual gpointer GetPlatformPixbuf ();
	
private:
    BYTE *data;
    UINT width, height, stride;
    Microsoft::WRL::ComPtr<IWICBitmapSource> pBitmapSource;
	HBITMAP hBitmap;
};

class MoonPixbufLoaderSDL2 : public MoonPixbufLoader {
public:
	MoonPixbufLoaderSDL2 (const char *imageType);
	MoonPixbufLoaderSDL2 ();
	virtual ~MoonPixbufLoaderSDL2 ();

	virtual void Write (const guchar *buffer, int buflen, MoonError **error);
	virtual void Close (MoonError **error);
	virtual MoonPixbuf *GetPixbuf ();

private:
	bool crc_error;
	GByteArray *data;
	const char *image_type;
	guint offset;
	MoonPixbufSDL2 *pixbuf;
    Microsoft::WRL::ComPtr<IWICImagingFactory> pWICFactory;
};

};
#endif /* MOON_PIXBUF_SDL2_H */
