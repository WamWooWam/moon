/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#pragma once

#include "pal.h"

namespace Moonlight {

class MoonFontServiceFontconfig : public MoonFontService {
	double dpi;
public:
	MoonFontServiceFontconfig();
	~MoonFontServiceFontconfig();

	virtual void ForeachFont(MoonForeachFontCallback foreach, gpointer user_data);
	virtual MoonFont *FindFont(const FontStyleInfo *pattern);

	virtual guint32 GetCharIndex(FT_Face face, gunichar unichar);
};

}
