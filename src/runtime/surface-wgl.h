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

#ifdef USE_WGL

#include "surface-gl.h"
#if defined(USE_GLEW)
#include <GL/glew.h>
#include <GL/wglew.h>
#endif

namespace Moonlight {

class MOON_API MoonWGLSurface : public GLSurface {
public:
	MoonWGLSurface (HWND hWnd, HDC hDC);
	MoonWGLSurface (GLsizei w, GLsizei h);

    inline HWND GetHWND() { return hWnd; }
    inline HDC GetHDC() { return hDC; }

	void SwapBuffers ();
	void Reshape (int width, int height);
	cairo_surface_t *Cairo ();
	GLuint Texture ();
	bool HasTexture ();

private:
    HWND hWnd;
    HDC hDC;
};

};

#endif 
#endif /* __MOON_SURFACE_EGL_H__ */
