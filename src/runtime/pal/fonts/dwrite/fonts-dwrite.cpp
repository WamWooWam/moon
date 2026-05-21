/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * fonts-dwrite.cpp: DirectWrite-backed font catalog for the FreeType text stack.
 *
 * Copyright 2011 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#include <config.h>

#include <glib.h>

#include <dwrite.h>

#include "fonts-dwrite.h"
#include "font-utils.h"

#pragma comment(lib, "dwrite")

using namespace Microsoft::WRL;

namespace Moonlight {

    MoonFontServiceDWrite::MoonFontServiceDWrite() {
        DWriteCreateFactory(DWRITE_FACTORY_TYPE_SHARED, __uuidof(IDWriteFactory),
            reinterpret_cast<IUnknown **>(factory.GetAddressOf()));
    }

    MoonFontServiceDWrite::~MoonFontServiceDWrite() {
    }

    bool MoonFontServiceDWrite::EnsureCollection() {
        if (collection != nullptr)
            return true;
        if (factory == nullptr)
            return false;

        HRESULT hr = factory->GetSystemFontCollection(collection.GetAddressOf(), FALSE);
        return SUCCEEDED(hr) && collection != nullptr;
    }

    bool MoonFontServiceDWrite::ResolveFontFile(IDWriteFont *font, char **out_path, int *out_index) {
        *out_path = nullptr;
        *out_index = 0;

        ComPtr<IDWriteFontFace> face;
        if (FAILED(font->CreateFontFace(face.GetAddressOf())))
            return false;

        UINT32 numberOfFiles = 0;
        if (FAILED(face->GetFiles(&numberOfFiles, nullptr)) || numberOfFiles == 0)
            return false;

        ComPtr<IDWriteFontFile> file;
        UINT32 wanted = 1;
        if (FAILED(face->GetFiles(&wanted, file.GetAddressOf())) || file == nullptr)
            return false;

        const void *referenceKey = nullptr;
        UINT32 referenceKeySize = 0;
        if (FAILED(file->GetReferenceKey(&referenceKey, &referenceKeySize)))
            return false;

        ComPtr<IDWriteFontFileLoader> loader;
        if (FAILED(file->GetLoader(loader.GetAddressOf())))
            return false;

        ComPtr<IDWriteLocalFontFileLoader> localLoader;
        if (FAILED(loader.As(&localLoader)))
            return false;

        UINT32 pathLen = 0;
        if (FAILED(localLoader->GetFilePathLengthFromKey(referenceKey, referenceKeySize, &pathLen)))
            return false;

        wchar_t *pathW = g_new(wchar_t, pathLen + 1);
        if (FAILED(localLoader->GetFilePathFromKey(referenceKey, referenceKeySize, pathW, pathLen + 1))) {
            g_free(pathW);
            return false;
        }

        char *path_utf8 = g_utf16_to_utf8(reinterpret_cast<gunichar2 *>(pathW), -1, NULL, NULL, NULL);
        g_free(pathW);
        if (path_utf8 == nullptr)
            return false;

        *out_path = path_utf8;
        *out_index = (int)face->GetIndex();
        return true;
    }

    MoonFont *MoonFontServiceDWrite::FindFont(const FontStyleInfo *desired) {
        if (!EnsureCollection())
            return NULL;

        const char *family_name = desired->family_name;
        if (family_name == nullptr || !g_ascii_strcasecmp(family_name, "Sans"))
            family_name = "Segoe UI";

        wchar_t *familyW = reinterpret_cast<wchar_t *>(g_utf8_to_utf16(family_name, -1, NULL, NULL, NULL));

        UINT32 familyIndex = 0;
        BOOL exists = FALSE;
        if (familyW != nullptr) {
            collection->FindFamilyName(familyW, &familyIndex, &exists);
            g_free(familyW);
        }

        if (!exists) {
            UINT32 idx = 0;
            BOOL fallbackExists = FALSE;
            collection->FindFamilyName(L"Segoe UI", &idx, &fallbackExists);
            if (!fallbackExists)
                collection->FindFamilyName(L"Arial", &idx, &fallbackExists);

            if (fallbackExists) {
                familyIndex = idx;
            } else if (collection->GetFontFamilyCount() > 0) {
                familyIndex = 0;
            } else {
                return NULL;
            }
        }

        ComPtr<IDWriteFontFamily> family;
        if (FAILED(collection->GetFontFamily(familyIndex, family.GetAddressOf())))
            return NULL;

        int weight = desired->weight;
        if (weight < 1)
            weight = DWRITE_FONT_WEIGHT_NORMAL;
        else if (weight > 999)
            weight = 999;

        int stretch = desired->stretch;
        if (stretch < DWRITE_FONT_STRETCH_ULTRA_CONDENSED || stretch > DWRITE_FONT_STRETCH_ULTRA_EXPANDED)
            stretch = DWRITE_FONT_STRETCH_NORMAL;

        int style = desired->style;
        if (style < DWRITE_FONT_STYLE_NORMAL || style > DWRITE_FONT_STYLE_ITALIC)
            style = DWRITE_FONT_STYLE_NORMAL;

        ComPtr<IDWriteFont> font;
        if (FAILED(family->GetFirstMatchingFont((DWRITE_FONT_WEIGHT)weight,
                (DWRITE_FONT_STRETCH)stretch, (DWRITE_FONT_STYLE)style, font.GetAddressOf())))
            return NULL;

        char *path = nullptr;
        int index = 0;
        if (!ResolveFontFile(font.Get(), &path, &index))
            return NULL;

        MoonFont *result = new MoonFont(path, index);
        g_free(path);
        return result;
    }

    void MoonFontServiceDWrite::ForeachFont(MoonForeachFontCallback foreach, gpointer user_data) {
        if (!EnsureCollection())
            return;

        UINT32 familyCount = collection->GetFontFamilyCount();
        for (UINT32 i = 0; i < familyCount; i++) {
            ComPtr<IDWriteFontFamily> family;
            if (FAILED(collection->GetFontFamily(i, family.GetAddressOf())))
                continue;

            UINT32 fontCount = family->GetFontCount();
            for (UINT32 j = 0; j < fontCount; j++) {
                ComPtr<IDWriteFont> font;
                if (FAILED(family->GetFont(j, font.GetAddressOf())))
                    continue;

                char *path = nullptr;
                int index = 0;
                if (!ResolveFontFile(font.Get(), &path, &index))
                    continue;

                bool keep_going = foreach(path, index, user_data);
                g_free(path);

                if (!keep_going)
                    return;
            }
        }
    }
}
