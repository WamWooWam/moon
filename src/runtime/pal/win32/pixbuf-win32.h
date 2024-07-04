/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#ifndef MOON_PIXBUF_Win32_H
#define MOON_PIXBUF_Win32_H

#include "pal.h"
#include <wrl.h>

struct IWICImagingFactory;
struct IWICBitmapSource;

namespace Moonlight {

class MoonPixbufWin32 : public MoonPixbuf {
public:
    MoonPixbufWin32(IWICBitmapSource *image, bool crc_error);
	virtual ~MoonPixbufWin32 ();

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

class MoonPixbufLoaderWin32 : public MoonPixbufLoader {
public:
	MoonPixbufLoaderWin32 (const char *imageType);
	MoonPixbufLoaderWin32 ();
	virtual ~MoonPixbufLoaderWin32 ();

	virtual void Write (const guchar *buffer, int buflen, MoonError **error);
	virtual void Close (MoonError **error);
	virtual MoonPixbuf *GetPixbuf ();

private:
	bool crc_error;
	GByteArray *data;
	const char *image_type;
	guint offset;
	MoonPixbufWin32 *pixbuf;
    Microsoft::WRL::ComPtr<IWICImagingFactory> pWICFactory;
};

};
#endif /* MOON_PIXBUF_Win32_H */
