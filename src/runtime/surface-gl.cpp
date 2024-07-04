/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * surface-gl.cpp
 *
 * Copyright 2010 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#include <config.h>

#include "surface-gl.h"

namespace Moonlight {

GLSurface::GLSurface ()
{
	size[0]       = 0;
	size[1]       = 0;
	texture       = 0;
	textureYUV[0] = 0;
	textureYUV[1] = 0;
	textureYUV[2] = 0;
	data          = NULL;
}

GLSurface::GLSurface (GLsizei width, GLsizei height)
{
	size[0]       = width;
	size[1]       = height;
	texture       = 0;
	textureYUV[0] = 0;
	textureYUV[1] = 0;
	textureYUV[2] = 0;
	data          = NULL;
}

GLSurface::~GLSurface ()
{
	if (data)
		g_free (data);

	if (texture)
		glDeleteTextures (1, &texture);

	if (textureYUV[0])
		glDeleteTextures (3, textureYUV);
}

cairo_surface_t *
GLSurface::Cairo ()
{
	int stride = size[0] * 4;

	if (!data) {
		data = (unsigned char *) g_malloc0 (stride * size[1]);

		// derived class should implement read back of texture image
		g_assert (texture == 0 && !IsPlanar ());
	}

	return cairo_image_surface_create_for_data (data,
						    CAIRO_FORMAT_ARGB32,
						    size[0],
						    size[1],
						    stride);
}

GLuint
GLSurface::Texture ()
{
	if (!texture)
		glGenTextures (1, &texture);

	// derived class should implement texture image allocation
	g_assert (data == NULL);

	return texture;
}

bool
GLSurface::IsPlanar ()
{
	return (textureYUV[0] && textureYUV[1] && textureYUV[2]);
}

GLuint
GLSurface::TextureYUV (int i)
{
	if (!textureYUV[i]) {
		int width[] = { size[0], size[0] / 2, size[0] / 2 };
		int height[] = { size[1], size[1] / 2, size[1] / 2 };
		int j;

		glGenTextures (3, textureYUV);

		for (j = 0; j < 3; j++) {
			glBindTexture (GL_TEXTURE_2D, textureYUV[j]);
			glTexImage2D (GL_TEXTURE_2D,
				      0,
				      GL_LUMINANCE,
				      width[j],
				      height[j],
				      0,
				      GL_LUMINANCE,
				      GL_UNSIGNED_BYTE,
				      NULL);
		}
		glBindTexture (GL_TEXTURE_2D, 0);
	}

	return textureYUV[i];
}

GLsizei
GLSurface::Width ()
{
	return size[0];
}

GLsizei
GLSurface::Height ()
{
	return size[1];
}

__GLFuncPtr
GLSurface::GetProcAddress (const char *procname)
{
	g_warning ("GLSurface::GetProcAddress has been called. The derived class should have overridden it.");
	return NULL;
}

};
