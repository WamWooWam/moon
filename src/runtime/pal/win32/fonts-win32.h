/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#pragma once
#include "pal.h"

struct IFileDialog;

namespace Moonlight {
    class MoonFontServiceWin32 : public MoonFontService {
        GPtrArray *system_fonts;

    public:
        MoonFontServiceWin32();
        ~MoonFontServiceWin32();

        virtual void ForeachFont(MoonForeachFontCallback foreach, gpointer user_data);
        virtual MoonFont *FindFont(const FontStyleInfo *pattern);
    };
};
