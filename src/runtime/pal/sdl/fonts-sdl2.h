/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#pragma once
#include "pal.h"

struct IFileDialog;

namespace Moonlight {
    class MoonFontServiceSDL2 : public MoonFontService {
        GPtrArray *system_fonts;

    public:
        MoonFontServiceSDL2();
        ~MoonFontServiceSDL2();

        virtual void ForeachFont(MoonForeachFontCallback foreach, gpointer user_data);
        virtual MoonFont *FindFont(const FontStyleInfo *pattern);
    };
};
