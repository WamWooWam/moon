/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * surface-glx.cpp
 *
 * Copyright 2010 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#include <config.h>

#if USE_WGL

#include <string.h>

#include "surface-wgl.h"

namespace Moonlight {

    MoonWGLSurface::MoonWGLSurface(HWND hWnd, HDC hDC) : GLSurface() {
        this->hWnd = hWnd;
        this->hDC = hDC;

        RECT rc;
        GetClientRect(hWnd, &rc);
        size[0] = rc.right - rc.left;
        size[1] = rc.bottom - rc.top;
    }

    MoonWGLSurface::MoonWGLSurface(GLsizei w, GLsizei h) : GLSurface(w, h) {
        this->hWnd = NULL;
        this->hDC = NULL;
    }

    void MoonWGLSurface::SwapBuffers() {
        g_assert(hDC);
        wglSwapLayerBuffers(hDC, WGL_SWAP_MAIN_PLANE);
    }

    void MoonWGLSurface::Reshape(int width, int height) {
        size[0] = width;
        size[1] = height;

        glViewport(0, 0, width, height);

        if (texture) {
            glDeleteTextures(1, &texture);
            texture = 0;
        }

        if (data) {
            g_free(data);
            data = NULL;
        }
    }

    cairo_surface_t* MoonWGLSurface::Cairo() {
        g_assert(hWnd == NULL);

        return GLSurface::Cairo();
    }

    GLuint MoonWGLSurface::Texture() {
        GLuint name = texture;

        if (!texture)
            glGenTextures(1, &texture);

        if (name != texture || data) {
            glBindTexture(GL_TEXTURE_2D, texture);
            glTexImage2D(GL_TEXTURE_2D,
                0,
                GL_RGBA,
                size[0],
                size[1],
                0,
                GL_RGBA,
                GL_UNSIGNED_BYTE,
                data);
            glBindTexture(GL_TEXTURE_2D, 0);
        }

        if (data) {
            g_free(data);
            data = NULL;
        }

        return texture;
    }

    bool MoonWGLSurface::HasTexture() {
        return texture != 0;
    }

};

#endif /* USE_WGL */