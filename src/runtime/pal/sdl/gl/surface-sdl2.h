/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * surface-glx.h
 *
 * Copyright 2010 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#ifndef __MOON_SURFACE_EGL_H__
#define __MOON_SURFACE_EGL_H__

#include <SDL.h>

#include "surface-gl.h"
#include <GL/GL.h>
#include <GL/glew.h>

namespace Moonlight {

    class MOON_API MoonSDLGLSurface : public GLSurface {
    public:
        MoonSDLGLSurface(SDL_Window* window, SDL_Renderer* renderer);
        MoonSDLGLSurface(GLsizei w, GLsizei h);

        inline SDL_Window* Window() { return window; }
        inline SDL_Renderer* Renderer() { return renderer; }

        void SwapBuffers();
        void Reshape(int width, int height);
        cairo_surface_t* Cairo();
        GLuint Texture();
        bool HasTexture();

        __GLFuncPtr GetProcAddress(const char* procname);

    private:
        SDL_Window* window;
        SDL_Renderer* renderer;
    };

};

#endif /* __MOON_SURFACE_EGL_H__ */
