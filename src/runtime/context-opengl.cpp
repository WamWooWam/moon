/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * context-opengl.cpp
 *
 * Copyright 2011 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#include <config.h>

#include <stdlib.h>
#include <string.h>

#define __MOON_OPENGL__

#include "context-opengl.h"
#include "projection.h"
#include "effect.h"
#include "region.h"
#include "debug.h"

#define IS_TRANSLUCENT(x) (x * 255 < 254.5)

namespace Moonlight {

OpenGLContext::OpenGLContext (OpenGLSurface *surface) : GLContext (surface)
{
}

OpenGLContext::~OpenGLContext ()
{
}

bool
OpenGLContext::Initialize ()
{
	const char *requiredExtension[] = {
		"GL_EXT_framebuffer_object",
		"GL_ARB_texture_non_power_of_two",
		"GL_ARB_shading_language_100"
	};
	const char *version = (const char *) glGetString (GL_VERSION);
        const char *extensions = (const char *) glGetString (GL_EXTENSIONS);

	if (!version || !extensions) {
		g_warning ("glGetString returned NULL");
		return false;
	}

	if (atof (version) < MIN_GL_VERSION) {
		moon_debug ("Moonlight: Insufficient OpenGL version: %s", version);
		return false;
	}

	for (guint i = 0; i < G_N_ELEMENTS (requiredExtension); i++) {
		if (!strstr (extensions, requiredExtension[i])) {
			g_warning ("%s missing", requiredExtension[i]);
			return false;
		}
	}

	glGetIntegerv (GL_MAX_TEXTURE_SIZE, &maxTextureSize);

	if (maxTextureSize < 2048)
		g_warning ("OpenGL max texture size: %d", maxTextureSize);

	printf ("Moonlight: OpenGL vendor string: %s\n",
		glGetString (GL_VENDOR));
	printf ("Moonlight: OpenGL renderer string: %s\n",
		glGetString (GL_RENDERER));
	printf ("Moonlight: OpenGL version string: %s\n",
		glGetString (GL_VERSION));

	return true;
}

void
OpenGLContext::SetupVertexData (double x,
                                double y,
                                double width,
                                double height)
{
	Context::Target *target = Top ()->GetTarget ();
	MoonSurface     *ms;
	Rect            r = target->GetData (&ms);
	OpenGLSurface   *dst = (OpenGLSurface *) ms;
	double          dx = 2.0 / dst->Width ();
	double          dy = 2.0 / dst->Height ();
	double          p[4][4];
	int             i;

	p[0][0] = x;
	p[0][1] = y;
	p[0][2] = 0.0;
	p[0][3] = 1.0;

	p[1][0] = x + width;
	p[1][1] = y;
	p[1][2] = 0.0;
	p[1][3] = 1.0;

	p[2][0] = x + width;
	p[2][1] = y + height;
	p[2][2] = 0.0;
	p[2][3] = 1.0;

	p[3][0] = x;
	p[3][1] = y + height;
	p[3][2] = 0.0;
	p[3][3] = 1.0;

	for (i = 0; i < 4; i++) {
		vertices[i][0] = p[i][0] * dx - p[i][3];
		vertices[i][1] = p[i][1] * dy - p[i][3];
		vertices[i][2] = -p[i][2];
		vertices[i][3] = p[i][3];
	}

	if (dst->HasDrawable ()) {
		int i;

		for (i = 0; i < 4; i++) {
			GLfloat v = vertices[i][1] + vertices[i][3];

			vertices[i][1] = vertices[i][3] - v;
		}
	}

	ms->unref ();
}

void
OpenGLContext::SetupVertexData (const double *matrix,
                                double       x,
                                double       y,
                                double       width,
                                double       height)
{
	Target      *target = Top ()->GetTarget ();
	MoonSurface *ms;
	Rect        r = target->GetData (&ms);
	OpenGLSurface  *dst = (OpenGLSurface *) ms;
	double      dx = 2.0 / dst->Width ();
	double      dy = 2.0 / dst->Height ();
	double      p[4][4];
	int         i;

	p[0][0] = 0.0;
	p[0][1] = 0.0;
	p[0][2] = 0.0;
	p[0][3] = 1.0;

	p[1][0] = r.width;
	p[1][1] = 0.0;
	p[1][2] = 0.0;
	p[1][3] = 1.0;

	p[2][0] = r.width;
	p[2][1] = r.height;
	p[2][2] = 0.0;
	p[2][3] = 1.0;

	p[3][0] = 0.0;
	p[3][1] = r.height;
	p[3][2] = 0.0;
	p[3][3] = 1.0;

	for (i = 0; i < 4; i++) {
		vertices[i][0] = p[i][0] * dx - p[i][3];
		vertices[i][1] = p[i][1] * dy - p[i][3];
		vertices[i][2] = -p[i][2];
		vertices[i][3] = p[i][3];
	}

	if (dst->HasDrawable ()) {
		int i;

		for (i = 0; i < 4; i++) {
			GLfloat v = vertices[i][1] + vertices[i][3];

			vertices[i][1] = vertices[i][3] - v;
		}
	}

	ms->unref ();
}

void
OpenGLContext::SetupTexCoordData (const double *matrix,
                                  double       du,
                                  double       dv)
{
	Context::Target *target = Top ()->GetTarget ();
	Rect            r = target->GetData (NULL);
	double          p[4][4];
	int             i;

	p[0][0] = 0.0;
	p[0][1] = 0.0;
	p[0][2] = 0.0;
	p[0][3] = 1.0;

	p[1][0] = r.width;
	p[1][1] = 0.0;
	p[1][2] = 0.0;
	p[1][3] = 1.0;

	p[2][0] = r.width;
	p[2][1] = r.height;
	p[2][2] = 0.0;
	p[2][3] = 1.0;

	p[3][0] = 0.0;
	p[3][1] = r.height;
	p[3][2] = 0.0;
	p[3][3] = 1.0;

	for (i = 0; i < 4; i++) {
		Matrix3D::TransformPoint (p[i], matrix, p[i]);

		texcoords[i][0] = p[i][0] * du;
		texcoords[i][1] = p[i][1] * dv;
		texcoords[i][2] = p[i][2];
		texcoords[i][3] = p[i][3];
	}
}

void
OpenGLContext::SetupTexUnit (GLenum target, GLint texture)
{
	glBindTexture (target, texture);
	glTexParameteri (target, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
	glTexParameteri (target, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri (target, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_BORDER);
	glTexParameteri (target, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_BORDER);
}

gboolean
OpenGLContext::HasDrawable ()
{
	Target        *target = Top ()->GetTarget ();
	MoonSurface   *ms;
	Rect          r = target->GetData (&ms);
	OpenGLSurface *dst = (OpenGLSurface *) ms;
	gboolean      status = FALSE;

	if (dst->HasDrawable () || dst->HasTexture ())
		status = TRUE;

	ms->unref ();

	return status;
}

void
OpenGLContext::SyncDrawable ()
{
	Target        *target = Top ()->GetTarget ();
	Target        *cairo = target->GetCairoTarget ();
	MoonSurface   *ms;
	Rect          r = target->GetData (&ms);
	OpenGLSurface *dst = (OpenGLSurface *) ms;

	// clear target contents
	if (!target->GetInit ()) {
		if (!dst->HasDrawable ())
			GLContext::SetFramebuffer ();

		glClearColor (0.0, 0.0, 0.0, 0.0);
		glClear (GL_COLOR_BUFFER_BIT);

		// mark target contents as initialized
		target->SetInit (ms);
	}

	// initialize target contents with surface
	if (target->GetInit () != ms) {
		OpenGLSurface *src = (OpenGLSurface  *) target->GetInit ();
		GLuint     texture0 = src->Texture ();
		GLuint     program = GetProjectProgram (1.0, 0);
		GLsizei    width0 = src->Width ();
		GLsizei    height0 = src->Height ();

		if (!dst->HasDrawable ())
			GLContext::SetFramebuffer ();

		SetViewport ();

		glUseProgram (program);

		SetupVertexData (0, 0, width0, height0);
		GLContext::SetupTexCoordData ();

		glVertexAttribPointer (0, 4,
				       GL_FLOAT, GL_FALSE, 0,
				       vertices);
		glVertexAttribPointer (1, 4,
				       GL_FLOAT, GL_FALSE, 0,
				       texcoords);

		glBindTexture (GL_TEXTURE_2D, texture0);
		glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER,
				 GL_NEAREST);
		glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER,
				 GL_NEAREST);
		glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_WRAP_S,
				 GL_CLAMP_TO_EDGE);
		glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_WRAP_T,
				 GL_CLAMP_TO_EDGE);
		glUniform1i (glGetUniformLocation (program, "sampler0"), 0);

		glEnableVertexAttribArray (0);
		glEnableVertexAttribArray (1);

		glDrawArrays (GL_TRIANGLE_FAN, 0, 4);

		glDisableVertexAttribArray (1);
		glDisableVertexAttribArray (0);

		glBindTexture (GL_TEXTURE_2D, 0);

		glUseProgram (0);

		glBindFramebuffer (GL_FRAMEBUFFER, 0);

		// mark target contents as initialized
		target->SetInit (ms);
	}

	// render any cairo contents onto target
	if (cairo) {
		MoonSurface   *mSrc;
		Rect          rSrc = cairo->GetData (&mSrc);
		OpenGLSurface *src = (OpenGLSurface  *) mSrc;
		GLuint        texture0 = src->Texture ();
		GLuint        program = GetProjectProgram (1.0, 0);
		GLsizei       width0 = src->Width ();
		GLsizei       height0 = src->Height ();

		if (!dst->HasDrawable ())
			GLContext::SetFramebuffer ();

		SetViewport ();

		glUseProgram (program);

		SetupVertexData (rSrc.x - r.x, rSrc.y - r.y, width0, height0);
		GLContext::SetupTexCoordData ();

		glVertexAttribPointer (0, 4,
				       GL_FLOAT, GL_FALSE, 0,
				       vertices);
		glVertexAttribPointer (1, 4,
				       GL_FLOAT, GL_FALSE, 0,
				       texcoords);

		glBindTexture (GL_TEXTURE_2D, texture0);
		glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER,
				 GL_NEAREST);
		glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER,
				 GL_NEAREST);
		glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_WRAP_S,
				 GL_CLAMP_TO_EDGE);
		glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_WRAP_T,
				 GL_CLAMP_TO_EDGE);
		glUniform1i (glGetUniformLocation (program, "sampler0"), 0);

		glEnableVertexAttribArray (0);
		glEnableVertexAttribArray (1);

		glEnable (GL_BLEND);
		glBlendFunc (GL_ONE, GL_ONE_MINUS_SRC_ALPHA);

		glDrawArrays (GL_TRIANGLE_FAN, 0, 4);

		glDisable (GL_BLEND);

		glDisableVertexAttribArray (1);
		glDisableVertexAttribArray (0);

		glBindTexture (GL_TEXTURE_2D, 0);

		glUseProgram (0);

		glBindFramebuffer (GL_FRAMEBUFFER, 0);

		mSrc->unref ();

		target->SetCairoTarget (NULL);
	}

	ms->unref ();
}

Rect
OpenGLContext::GroupBounds (Group extents)
{
	if (extents.r.width  > maxTextureSize ||
	    extents.r.height > maxTextureSize)
		return Rect ();

	return extents.r;
}

void
OpenGLContext::Push (Group extents)
{
	Rect r = GroupBounds (extents);

	if (!r.IsEmpty ()) {
		cairo_matrix_t matrix;
		OpenGLSurface  *surface = new OpenGLSurface (r.width, r.height);
		Target         *target = new Target (surface, extents.r);

		Top ()->GetMatrix (&matrix);

		// mark target contents as uninitialized
		target->SetInit (NULL);

		Stack::Push (new Context::Node (target, &matrix, &extents.r));

		target->unref ();
		surface->unref ();
	}
	else {
		Context::Push (Clip ());
	}
}

cairo_t *
OpenGLContext::Push (Cairo extents)
{
	Target *target = Top ()->GetTarget ();
	Target *cairo = target->GetCairoTarget ();
	Rect   box;

	Top ()->GetClip (&box);

	box = box.Intersection (extents.r);

	if (box.IsEmpty ())
		return Context::Push (extents);

	if (cairo) {
		Rect   r = cairo->GetData (NULL);
		Region *region = new Region (r);

		if (region->RectIn (box) != CAIRO_REGION_OVERLAP_IN)
			SyncDrawable ();

		delete region;
	}

	if (!target->GetCairoTarget ()) {
		MoonSurface *ms;
		Rect        r = target->GetData (&ms);

		if (HasDrawable ()) {
			OpenGLSurface *surface = new OpenGLSurface (box.width,
                                                                    box.height);
			Target        *cairo = new Target (surface, box);

			target->SetCairoTarget (cairo);

			cairo->unref ();
			surface->unref ();
		}
		else {
			// mark target contents as initialized
			target->SetInit (ms);
		}

		ms->unref ();
	}

	return Context::Push (extents);
}

Rect
OpenGLContext::Pop (MoonSurface **ref)
{
	Context::Node *prev = (Context::Node *) Top ()->prev;

	g_assert (prev);

	if (Top ()->GetTarget () != prev->GetTarget ()) {
		Target      *target = Top ()->GetTarget ();
		MoonSurface *init = target->GetInit ();
		MoonSurface *ms;
		Rect        r = target->GetData (&ms);
		MoonSurface *data = init != ms ? init : NULL;

		ms->unref ();

		// return reference to initial state surface instead
		// of the target surface itself
		if (data) {
			Node *node = (Node *) Stack::Pop ();

			*ref = data->ref ();
			delete node;
			return r;
		}
		else {
			SyncDrawable ();
		}
	}

	return GLContext::Pop (ref);
}

void
OpenGLContext::SetFramebuffer ()
{
	Target        *target = Top ()->GetTarget ();
	MoonSurface   *ms;
	Rect          r = target->GetData (&ms);
	OpenGLSurface *dst = (OpenGLSurface *) ms;

	SyncDrawable ();

	if (!dst->HasDrawable ())
		GLContext::SetFramebuffer ();

	ms->unref ();
}

void
OpenGLContext::SetScissor ()
{
	Target        *target = Top ()->GetTarget ();
	MoonSurface   *ms;
	Rect          r = target->GetData (&ms);
	OpenGLSurface *dst = (OpenGLSurface *) ms;
	Rect          clip;

	Top ()->GetClip (&clip);

	clip.x -= r.x;
	clip.y -= r.y;

	if (dst->HasDrawable ()) {
		glScissor (clip.x,
			   dst->Height () - (clip.y + clip.height),
			   clip.width,
			   clip.height);
	}
	else {
		GLContext::SetScissor ();
	}

	ms->unref ();
}

void
OpenGLContext::Blit (unsigned char *data,
                     int           stride)
{
	Target        *target = Top ()->GetTarget ();
	MoonSurface   *ms;
	Rect          r = target->GetData (&ms);
	OpenGLSurface *dst = (OpenGLSurface *) ms;
	Rect          clip;

	// no support for clipping
	g_assert (GetClip () == r);

	// no support for blit to drawable at the moment
	g_assert (!dst->HasDrawable ());

	// mark target as initialized
	target->SetInit (ms);

	glPixelStorei (GL_UNPACK_ROW_LENGTH,
		       PixelRowLength (stride, dst->Width (), 4));
	glPixelStorei (GL_UNPACK_ALIGNMENT, PixelAlignment (stride));
	glBindTexture (GL_TEXTURE_2D, dst->Texture ());
	glTexSubImage2D (GL_TEXTURE_2D,
			 0,
			 0,
			 0,
			 dst->Width (),
			 dst->Height (),
			 GL_BGRA,
			 GL_UNSIGNED_BYTE,
			 data);
	glBindTexture (GL_TEXTURE_2D, 0);
	glPixelStorei (GL_UNPACK_ALIGNMENT, 4);
	glPixelStorei (GL_UNPACK_ROW_LENGTH, 0);

	ms->unref ();
}

void
OpenGLContext::BlitYV12 (unsigned char *data[],
                         int           stride[])
{
	Target        *target = Top ()->GetTarget ();
	MoonSurface   *ms;
	Rect          r = target->GetData (&ms);
	OpenGLSurface *dst = (OpenGLSurface *) ms;
	int           size[] = { dst->Width (), dst->Height () };
	int           width[] = { size[0], size[0] / 2, size[0] / 2 };
	int           height[] = { size[1], size[1] / 2, size[1] / 2 };
	int           i;

	// no support for clipping
	g_assert (GetClip () == r);

	// no support for blit to drawable at the moment
	g_assert (!dst->HasDrawable ());

	// mark target as initialized
	target->SetInit (ms);

	for (i = 0; i < 3; i++) {
		glPixelStorei (GL_UNPACK_ROW_LENGTH,
			       PixelRowLength (stride[i], width[i], 1));
		glPixelStorei (GL_UNPACK_ALIGNMENT, PixelAlignment (stride[i]));
		glBindTexture (GL_TEXTURE_2D, dst->TextureYUV (i));
		glTexSubImage2D (GL_TEXTURE_2D,
				 0,
				 0,
				 0,
				 width[i],
				 height[i],
				 GL_LUMINANCE,
				 GL_UNSIGNED_BYTE,
				 data[i]);
	}
	glBindTexture (GL_TEXTURE_2D, 0);
	glPixelStorei (GL_UNPACK_ALIGNMENT, 4);
	glPixelStorei (GL_UNPACK_ROW_LENGTH, 0);

	ms->unref ();
}

void
OpenGLContext::Paint (Color *color)
{
	Target      *target = Top ()->GetTarget ();
	MoonSurface *ms;
	Rect        r = target->GetData (&ms);
	Rect        clip;

	// avoid GL rendering to target without previously
	// allocated hardware drawable
	if (!HasDrawable ()) {
		Context::Paint (color);
		ms->unref ();
		return;
	}

	Top ()->GetClip (&clip);

	if (!target->GetInit () && r == clip) {
		// mark target as initialized
		target->SetInit (ms);
	}

	GLContext::Paint (color);

	ms->unref ();
}

void
OpenGLContext::Paint (MoonSurface *src,
                      double      alpha,
                      double      x,
                      double      y)
{
	OpenGLContext::Project (src, NULL, alpha, x, y);
}

void
OpenGLContext::Project (MoonSurface  *src,
                        const double *matrix,
                        double       alpha,
                        double       x,
                        double       y)
{
	OpenGLSurface *surface = (OpenGLSurface *) src;
	Target        *target = Top ()->GetTarget ();
	Rect          r = target->GetData (NULL);
	Rect          clip = GetClip ();
	double        m[16];

	if (!target->GetInit () && !IS_TRANSLUCENT (alpha) && r == clip) {
		int x0, y0;

		GetMatrix (m);
		if (matrix)
			Matrix3D::Multiply (m, matrix, m);

		if (Matrix3D::IsIntegerTranslation (m, &x0, &y0)) {
			OpenGLSurface *surface = (OpenGLSurface *) src;
			Rect          r = Rect (x + x0,
                                                y + y0,
                                                surface->Width (),
                                                surface->Height ());

			// matching dimensions and no transformation allow us
			// to set source as initial state of target surface when
			// it is not already initialized.
			if (r == clip) {
				target->SetInit (src);
				return;
			}
		}
	}

	if (!HasDrawable () && !surface->HasTexture ()) {
		int x0, y0;

		GetMatrix (m);
		if (matrix)
			Matrix3D::Multiply (m, matrix, m);

		// avoid GL rendering to target without previously
		// allocated hardware drawable
		if (Matrix3D::IsIntegerTranslation (m, &x0, &y0)) {
			cairo_matrix_t m;

			cairo_matrix_init_translate (&m, x0, y0);

			Context::Push (Context::AbsoluteTransform (m));
			Context::Paint (src, alpha, x, y);
			Context::Pop ();
			return;
		}
	}

	GetDeviceMatrix (m);
	if (matrix)
		Matrix3D::Multiply (m, matrix, m);

	GLContext::Paint (src, m, alpha, x, y);
}

void
OpenGLContext::Flush ()
{
	SyncDrawable ();
	GLContext::Flush ();
}

};
