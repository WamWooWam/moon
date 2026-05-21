/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#pragma once
#include "pal.h"
#include <wrl.h>

struct IDWriteFactory;
struct IDWriteFontCollection;
struct IDWriteFont;

namespace Moonlight {
    // Uses DirectWrite purely as a font catalog/matcher: it enumerates and
    // matches system + per-user fonts (with proper metadata), then resolves
    // each match to a file path + face index that the FreeType-based text
    // stack loads. DirectWrite does no rendering here.
    class MoonFontServiceDWrite : public MoonFontService {
        Microsoft::WRL::ComPtr<IDWriteFactory> factory;
        Microsoft::WRL::ComPtr<IDWriteFontCollection> collection;

        bool EnsureCollection();
        bool ResolveFontFile(IDWriteFont *font, char **out_path, int *out_index);

    public:
        MoonFontServiceDWrite();
        ~MoonFontServiceDWrite();

        virtual void ForeachFont(MoonForeachFontCallback foreach, gpointer user_data);
        virtual MoonFont *FindFont(const FontStyleInfo *pattern);
    };
};
