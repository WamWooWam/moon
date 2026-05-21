/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/* Font service for the SDL2 PAL — delegates to the platform font backend. */

#pragma once

#ifdef _WIN32
#include "pal/fonts/dwrite/fonts-dwrite.h"
#else
#include "pal/fonts/fontconfig/fonts-fontconfig.h"
#endif
