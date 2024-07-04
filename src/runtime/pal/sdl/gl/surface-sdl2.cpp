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

#include <string.h>

#include "surface-sdl2.h"

namespace Moonlight {

    MoonSDLGLSurface::MoonSDLGLSurface(SDL_Window* window, SDL_Renderer* renderer) : GLSurface() {
        this->window = window;
        this->renderer = renderer;

        SDL_GetWindowSize(window, &size[0], &size[1]);
    }

    MoonSDLGLSurface::MoonSDLGLSurface(GLsizei w, GLsizei h) : GLSurface(w, h) {
        this->window = NULL;
        this->renderer = NULL;
    }

    void MoonSDLGLSurface::SwapBuffers() {
        SDL_GL_SwapWindow(window);
    }

    __GLFuncPtr MoonSDLGLSurface::GetProcAddress(const char* procname) {
        return (__GLFuncPtr)SDL_GL_GetProcAddress(procname);
    }

    void MoonSDLGLSurface::Reshape(int width, int height) {
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

    cairo_surface_t* MoonSDLGLSurface::Cairo() {
        g_assert(window == NULL);

        return GLSurface::Cairo();
    }

    GLuint MoonSDLGLSurface::Texture() {
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

    bool MoonSDLGLSurface::HasTexture() {
        return texture != 0;
    }

};
