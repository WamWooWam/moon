/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * fonts-gtk.cpp: different types of collections
 *
 * Copyright 2011 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#include <config.h>

#include <glib.h>

#include <shobjidl.h>
#include <shlwapi.h>
#include <shlobj.h>

#include "pal-sdl2.h"
#include "fonts-sdl2.h"
#include "font-utils.h"

namespace Moonlight {

    struct SDL2Font {
        FontStyleInfo style;
        char *path;
        int index;

        SDL2Font(FT_Face face, const char *path, int index) {
            font_style_info_init(&style, NULL);

            // extract whatever little style info we can from the family name
            font_style_info_parse(&style, face->family_name, true);

            // style info parsed from style_name overrides anything we got from family_name
            font_style_info_parse(&style, face->style_name, false);

            this->path = g_strdup(path);
            this->index = index;
        }

        ~SDL2Font() {
            g_free(style.family_name);
            g_free(path);
        }
    };

    MoonFontServiceSDL2::MoonFontServiceSDL2() {
        SDL2Font *font;
        const char *name;
        GString *path;
        FT_Long i, n;
        FT_Face face;
        size_t len;
        GDir *dir;

        wchar_t *font_dir;
        if (FAILED(SHGetKnownFolderPath(FOLDERID_Fonts, 0, NULL, &font_dir))) {
            g_assert_not_reached();
            return;
        }

        char *font_dir_utf8 = g_utf16_to_utf8(reinterpret_cast<gunichar2 *>(font_dir), -1, NULL, NULL, NULL);
        system_fonts = g_ptr_array_new();

        if (!(dir = g_dir_open(font_dir_utf8, 0, NULL)))
            return;

        // append a trailing backslash to the path
        path = g_string_new(font_dir_utf8);
        g_string_append_c(path, '\\');
        len = path->len;

        // scan all system fonts
        while ((name = g_dir_read_name(dir))) {
            if (!strcmp(name, ".") || !strcmp(name, ".."))
                continue;

            g_string_truncate(path, len);
            g_string_append(path, name);

            if (FT_New_Face(libft2, path->str, 0, &face) != 0)
                continue;

            font = new SDL2Font(face, path->str, 0);
            g_ptr_array_add(system_fonts, font);

            n = face->num_faces;
            FT_Done_Face(face);

            for (i = 1; i < n; i++) {
                if (FT_New_Face(libft2, path->str, i, &face) != 0)
                    continue;

                font = new SDL2Font(face, path->str, 0);
                g_ptr_array_add(system_fonts, font);

                FT_Done_Face(face);
            }
        }

        g_string_free(path, true);
        g_dir_close(dir);
        g_free(font_dir_utf8);

        CoTaskMemFree(font_dir);
    }

    MoonFontServiceSDL2::~MoonFontServiceSDL2() {
    }

    MoonFont *MoonFontServiceSDL2::FindFont(const FontStyleInfo *desired) {
        SDL2Font *font, *best = NULL;
        int diff, closest = G_MAXINT;
        const char *family_name;

        if (g_ascii_strcasecmp(desired->family_name, "Sans") != 0)
            family_name = desired->family_name;
        else
            family_name = "Arial Black";

        for (guint i = 0; i < system_fonts->len; i++) {
            font = (SDL2Font *)system_fonts->pdata[i];

            if (!g_ascii_strcasecmp(font->style.family_name, family_name)) {
                diff = font_style_info_diff(&font->style, desired);
                if (diff < closest) {
                    closest = diff;
                    best = font;
                }
            }
        }

        if (best != NULL)
            return new MoonFont(best->path, best->index);

        return NULL;
    }

    void MoonFontServiceSDL2::ForeachFont(MoonForeachFontCallback foreach, gpointer user_data) {
        SDL2Font *font;

        for (guint i = 0; i < system_fonts->len; i++) {
            font = (SDL2Font *)system_fonts->pdata[i];

            if (!foreach (font->path, font->index, user_data))
                break;
        }
    }
};
