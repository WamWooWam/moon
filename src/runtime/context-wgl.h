/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * context-glx.h
 *
 * Copyright 2010 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#ifndef __MOON_CONTEXT_EGL_H__
#define __MOON_CONTEXT_EGL_H__

#ifdef USE_WGL

#include "context-gl.h"
#include "surface-wgl.h"

namespace Moonlight {

    class MOON_API MoonWGLContext : public GLContext {
    public:
        MoonWGLContext(MoonWGLSurface *surface);
        virtual ~MoonWGLContext();

        bool Initialize();

        void MakeCurrent();

        void Push(Group extents);
        cairo_t *Push(Cairo extents);
        Rect Pop(MoonSurface **ref);

        void Clear(Color *color);

        void Blit(unsigned char *data,
            int stride);

        void BlitYV12(unsigned char *data[],
            int stride[]);

        void Paint(Color *color);

        void Paint(MoonSurface *src,
            double alpha,
            double x,
            double y);

        void Project(MoonSurface *src,
            const double *matrix,
            double alpha,
            double x,
            double y);

        void Blur(MoonSurface *src,
            double radius,
            double x,
            double y);

        void DropShadow(MoonSurface *src,
            double dx,
            double dy,
            double radius,
            Color *color,
            double x,
            double y);

        void ShaderEffect(MoonSurface *src,
            PixelShader *shader,
            Brush **sampler,
            int *sampler_mode,
            int n_sampler,
            Color *constant,
            int n_constant,
            int *ddxUvDdyUvPtr,
            double x,
            double y);

        void Flush();

    protected:
        void SetupVertexData(const double *matrix,
            double x,
            double y,
            double width,
            double height);
        const char *ProgramPrecisionString();
        void SetFramebuffer();
        void SetScissor();
        void ForceCurrent();
        gboolean HasDrawable();
        void SyncDrawable();
        Rect GroupBounds(Group extents);

    private:
        HWND hWnd;
        HDC hDC;
        HGLRC hGL;
        GLint maxTextureSize;
    };

};

#endif /* USE_WGL */
#endif /* __MOON_CONTEXT_EGL_H__ */
