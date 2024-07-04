/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * context-wgl.cpp
 *
 * Copyright 2024 Thomas May
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#include <config.h>

#if USE_WGL

#include <stdlib.h>
#include <string.h>

#include "context-wgl.h"
#include "projection.h"
#include "effect.h"
#include "region.h"

#define IS_TRANSLUCENT(x) (x * 255 < 254.5)

namespace Moonlight {

    MoonWGLContext::MoonWGLContext(MoonWGLSurface *surface) : GLContext(surface) {
        this->hWnd = surface->GetHWND();
        this->hDC = surface->GetHDC();
    }

    MoonWGLContext::~MoonWGLContext() {
        if (hGL)
            wglDeleteContext(hGL);
    }

    bool MoonWGLContext::Initialize() {

        PIXELFORMATDESCRIPTOR pfd;
        memset(&pfd, 0, sizeof(PIXELFORMATDESCRIPTOR));
        pfd.nSize = sizeof(PIXELFORMATDESCRIPTOR);
        pfd.nVersion = 1;
        pfd.dwFlags = PFD_DOUBLEBUFFER | PFD_SUPPORT_OPENGL | PFD_DRAW_TO_WINDOW;
        pfd.iPixelType = PFD_TYPE_RGBA;
        pfd.cColorBits = 32;
        pfd.cDepthBits = 32;
        pfd.iLayerType = PFD_MAIN_PLANE;

        int nPixelFormat = ChoosePixelFormat(hDC, &pfd);

        if (nPixelFormat == 0)
            return false;

        BOOL bResult = SetPixelFormat(hDC, nPixelFormat, &pfd);

        if (!bResult)
            return false;

        HGLRC tempContext = wglCreateContext(hDC);
        wglMakeCurrent(hDC, tempContext);

        GLenum err = glewInit();
        if (GLEW_OK != err) {
            g_warning("GLEW is not initialized!");
        }

        int attribs[] = {
            WGL_CONTEXT_MAJOR_VERSION_ARB, 2,
            WGL_CONTEXT_MINOR_VERSION_ARB, 1,
            WGL_CONTEXT_FLAGS_ARB, 0,
            0
        };

        if (wglewIsSupported("WGL_ARB_create_context") == 1) {
            hGL = wglCreateContextAttribsARB(hDC, 0, attribs);
            wglMakeCurrent(NULL, NULL);
            wglDeleteContext(tempContext);
            wglMakeCurrent(hDC, hGL);
        }
        else {
            hGL = tempContext;
        }

        glGetIntegerv(GL_MAX_TEXTURE_SIZE, &maxTextureSize);

        wglSwapIntervalEXT(0);

        if (maxTextureSize < 2048)
            g_warning("OpenGL max texture size: %d", maxTextureSize);

        g_warning("Moonlight: OpenGL vendor string: %s\n", glGetString(GL_VENDOR));
        g_warning("Moonlight: OpenGL renderer string: %s\n", glGetString(GL_RENDERER));
        g_warning("Moonlight: OpenGL version string: %s\n", glGetString(GL_VERSION));
        // g_warning("Moonlight: OpenGL extension string: %s\n", glGetString(GL_EXTENSIONS));

        return true;
    }

    void MoonWGLContext::MakeCurrent() {
        g_assert(hGL);
        wglMakeCurrent(hDC, hGL);
    }

    void MoonWGLContext::ForceCurrent() {
        if (wglGetCurrentContext() != hGL)
            MakeCurrent();
    }

    const char *MoonWGLContext::ProgramPrecisionString() {
        return "";
    }

    void MoonWGLContext::SetupVertexData(const double *matrix,
        double x,
        double y,
        double width,
        double height) {
        Target *target = Top()->GetTarget();
        MoonSurface *ms;
        Rect r = target->GetData(&ms);
        MoonWGLSurface *dst = (MoonWGLSurface *)ms;
        double m[16];

        if (matrix)
            Matrix3D::Init(m, matrix);
        else
            Matrix3D::Identity(m);

        GLContext::SetupVertexData(m, x, y, width, height);

        if (dst->GetHDC()) {
            int i;

            for (i = 0; i < 4; i++) {
                GLfloat v = vertices[i][1] + vertices[i][3];

                vertices[i][1] = vertices[i][3] - v;
            }
        }

        ms->unref();
    }


    gboolean MoonWGLContext::HasDrawable() {
        Target *target = Top()->GetTarget();
        MoonSurface *ms;
        Rect r = target->GetData(&ms);
        MoonWGLSurface *dst = (MoonWGLSurface *)ms;
        gboolean status = FALSE;

        if (dst->GetHDC() || dst->HasTexture())
            status = TRUE;

        ms->unref();

        return status;
    }

    void MoonWGLContext::SyncDrawable() {
        Target *target = Top()->GetTarget();
        Target *cairo = target->GetCairoTarget();
        MoonSurface *ms;
        Rect r = target->GetData(&ms);
        MoonWGLSurface *dst = (MoonWGLSurface *)ms;

        // clear target contents
        if (!target->GetInit()) {
            if (!dst->GetHDC())
                GLContext::SetFramebuffer();

            glClearColor(0.0, 0.0, 0.0, 0.0);
            glClear(GL_COLOR_BUFFER_BIT);

            // mark target contents as initialized
            target->SetInit(ms);
        }

        // initialize target contents with surface
        if (target->GetInit() != ms) {
            MoonWGLSurface *src = (MoonWGLSurface *)target->GetInit();
            GLuint texture0 = src->Texture();
            GLuint program = GetProjectProgram(1.0, 0);
            GLsizei width0 = src->Width();
            GLsizei height0 = src->Height();

            if (!dst->GetHDC())
                GLContext::SetFramebuffer();

            SetViewport();

            glUseProgram(program);

            SetupVertexData(NULL, 0, 0, width0, height0);
            SetupTexCoordData();

            glVertexAttribPointer(0, 4,
                GL_FLOAT, GL_FALSE, 0,
                vertices);
            glVertexAttribPointer(1, 4,
                GL_FLOAT, GL_FALSE, 0,
                texcoords);

            glBindTexture(GL_TEXTURE_2D, texture0);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER,
                GL_NEAREST);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER,
                GL_NEAREST);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S,
                GL_CLAMP_TO_EDGE);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T,
                GL_CLAMP_TO_EDGE);
            glUniform1i(glGetUniformLocation(program, "sampler0"), 0);

            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);

            glDrawArrays(GL_TRIANGLE_FAN, 0, 4);

            glDisableVertexAttribArray(1);
            glDisableVertexAttribArray(0);

            glBindTexture(GL_TEXTURE_2D, 0);

            glUseProgram(0);

            glBindFramebuffer(GL_FRAMEBUFFER, 0);

            // mark target contents as initialized
            target->SetInit(ms);
        }

        // render any cairo contents onto target
        if (cairo) {
            MoonSurface *mSrc;
            Rect rSrc = cairo->GetData(&mSrc);
            MoonWGLSurface *src = (MoonWGLSurface *)mSrc;
            GLuint texture0 = src->Texture();
            GLuint program = GetProjectProgram(1.0, 0);
            GLsizei width0 = src->Width();
            GLsizei height0 = src->Height();

            if (!dst->GetHDC())
                GLContext::SetFramebuffer();

            SetViewport();

            glUseProgram(program);

            SetupVertexData(NULL, rSrc.x - r.x, rSrc.y - r.y, width0, height0);
            SetupTexCoordData();

            glVertexAttribPointer(0, 4,
                GL_FLOAT, GL_FALSE, 0,
                vertices);
            glVertexAttribPointer(1, 4,
                GL_FLOAT, GL_FALSE, 0,
                texcoords);

            glBindTexture(GL_TEXTURE_2D, texture0);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER,
                GL_NEAREST);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER,
                GL_NEAREST);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S,
                GL_CLAMP_TO_EDGE);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T,
                GL_CLAMP_TO_EDGE);
            glUniform1i(glGetUniformLocation(program, "sampler0"), 0);

            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);

            glEnable(GL_BLEND);
            glBlendFunc(GL_ONE, GL_ONE_MINUS_SRC_ALPHA);

            glDrawArrays(GL_TRIANGLE_FAN, 0, 4);

            glDisable(GL_BLEND);

            glDisableVertexAttribArray(1);
            glDisableVertexAttribArray(0);

            glBindTexture(GL_TEXTURE_2D, 0);

            glUseProgram(0);

            glBindFramebuffer(GL_FRAMEBUFFER, 0);

            mSrc->unref();

            target->SetCairoTarget(NULL);
        }

        ms->unref();
    }

    Rect MoonWGLContext::GroupBounds(Group extents) {
        if (extents.r.width > maxTextureSize ||
            extents.r.height > maxTextureSize)
            return Rect();

        return extents.r;
    }

    void MoonWGLContext::Push(Group extents) {
        Rect r = GroupBounds(extents);

        if (!r.IsEmpty()) {
            cairo_matrix_t matrix;
            MoonWGLSurface *surface = new MoonWGLSurface(r.width, r.height);
            Target *target = new Target(surface, extents.r);

            Top()->GetMatrix(&matrix);

            // mark target contents as uninitialized
            target->SetInit(NULL);

            Stack::Push(new Context::Node(target, &matrix, &extents.r));

            target->unref();
            surface->unref();
        }
        else {
            Context::Push(Clip());
        }
    }

    cairo_t *MoonWGLContext::Push(Cairo extents) {
        Target *target = Top()->GetTarget();
        Target *cairo = target->GetCairoTarget();
        Rect box;

        Top()->GetClip(&box);

        box = box.Intersection(extents.r);

        if (box.IsEmpty())
            return Context::Push(extents);

        if (cairo) {
            Rect r = cairo->GetData(NULL);
            Region *region = new Region(r);

            if (region->RectIn(box) != CAIRO_REGION_OVERLAP_IN) {
                ForceCurrent();
                SyncDrawable();
            }

            delete region;
        }

        if (!target->GetCairoTarget()) {
            MoonSurface *ms;
            Rect r = target->GetData(&ms);

            if (HasDrawable()) {
                MoonWGLSurface *surface = new MoonWGLSurface(box.width,
                    box.height);
                Target *cairo = new Target(surface, box);

                target->SetCairoTarget(cairo);

                cairo->unref();
                surface->unref();
            }
            else {
                // mark target contents as initialized
                target->SetInit(ms);
            }

            ms->unref();
        }

        return Context::Push(extents);
    }

    Rect MoonWGLContext::Pop(MoonSurface **ref) {
        Context::Node *prev = (Context::Node *)Top()->prev;

        g_assert(prev);

        if (Top()->GetTarget() != prev->GetTarget()) {
            Target *target = Top()->GetTarget();
            MoonSurface *init = target->GetInit();
            MoonSurface *ms;
            Rect r = target->GetData(&ms);
            MoonSurface *data = init != ms ? init : NULL;

            ms->unref();
            ForceCurrent();

            // return reference to initial state surface instead
            // of the target surface itself
            if (data) {
                Node *node = (Node *)Stack::Pop();

                *ref = data->ref();
                delete node;
                return r;
            }
            else {
                SyncDrawable();
            }
        }

        return GLContext::Pop(ref);
    }

    void MoonWGLContext::SetFramebuffer() {
        Target *target = Top()->GetTarget();
        MoonSurface *ms;
        Rect r = target->GetData(&ms);
        MoonWGLSurface *dst = (MoonWGLSurface *)ms;

        SyncDrawable();

        if (!dst->GetHDC())
            GLContext::SetFramebuffer();

        ms->unref();
    }

    void MoonWGLContext::SetScissor() {
        Target *target = Top()->GetTarget();
        MoonSurface *ms;
        Rect r = target->GetData(&ms);
        MoonWGLSurface *dst = (MoonWGLSurface *)ms;
        Rect clip;

        Top()->GetClip(&clip);

        clip.x -= r.x;
        clip.y -= r.y;

        if (dst->GetHDC()) {
            glScissor(clip.x,
                dst->Height() - (clip.y + clip.height),
                clip.width,
                clip.height);
        }
        else {
            GLContext::SetScissor();
        }

        ms->unref();
    }

    void MoonWGLContext::Clear(Color *color) {
        ForceCurrent();

        GLContext::Clear(color);
    }

    void MoonWGLContext::Blit(unsigned char *data,
        int stride) {
        Target *target = Top()->GetTarget();
        MoonSurface *ms;
        Rect r = target->GetData(&ms);
        MoonWGLSurface *dst = (MoonWGLSurface *)ms;
        unsigned char *buffer = data;
        int buffer_stride = stride;

        ForceCurrent();

        // no support for clipping
        g_assert(GetClip() == r);

        // no support for blit to drawable at the moment
        g_assert(!dst->GetHDC());

        // mark target as initialized
        target->SetInit(ms);

        if (PixelRowLength(stride, dst->Width(), 4) != dst->Width()) {
            buffer_stride = dst->Width() * 4;
            buffer = (unsigned char *)
                g_malloc(buffer_stride * dst->Height());

            for (int y = 0; y < dst->Height(); y++)
                memcpy(buffer + (y * buffer_stride),
                    data + (y * stride),
                    buffer_stride);
        }

        glPixelStorei(GL_UNPACK_ALIGNMENT, PixelAlignment(buffer_stride));
        glBindTexture(GL_TEXTURE_2D, dst->Texture());
        glTexSubImage2D(GL_TEXTURE_2D,
            0,
            0,
            0,
            dst->Width(),
            dst->Height(),
            GL_BGRA,
            GL_UNSIGNED_BYTE,
            buffer);
        glBindTexture(GL_TEXTURE_2D, 0);
        glPixelStorei(GL_UNPACK_ALIGNMENT, 4);

        if (buffer != data)
            g_free(buffer);

        ms->unref();
    }

    void MoonWGLContext::BlitYV12(unsigned char *data[],
        int stride[]) {
        Target *target = Top()->GetTarget();
        MoonSurface *ms;
        Rect r = target->GetData(&ms);
        MoonWGLSurface *dst = (MoonWGLSurface *)ms;
        int size[] = { dst->Width(), dst->Height() };
        int width[] = { size[0], size[0] / 2, size[0] / 2 };
        int height[] = { size[1], size[1] / 2, size[1] / 2 };
        unsigned char *buffer[] = { data[0], data[1], data[2] };
        int buffer_stride[] = { stride[0], stride[1], stride[2] };

        ForceCurrent();

        // no support for blit to drawable at the moment
        g_assert(!dst->GetHDC());

        // mark target as initialized
        target->SetInit(ms);

        for (int i = 0; i < 3; i++) {
            if (PixelRowLength(stride[i], width[i], 1) != width[i]) {
                buffer_stride[i] = width[i];
                buffer[i] = (unsigned char *)
                    g_malloc(buffer_stride[i] * height[i]);

                for (int y = 0; y < height[i]; y++)
                    memcpy(buffer[i] + y * buffer_stride[i],
                        data[i] + y * stride[i],
                        buffer_stride[i]);
            }
        }

        GLContext::BlitYV12(buffer, buffer_stride);

        for (int i = 0; i < 3; i++)
            if (buffer[i] != data[i])
                g_free(buffer[i]);


        ms->unref();
    }

    void MoonWGLContext::Paint(Color *color) {
        Target *target = Top()->GetTarget();
        MoonSurface *ms;
        Rect r = target->GetData(&ms);
        Rect clip;

        // avoid GL rendering to target without previously
        // allocated hardware drawable
        if (!HasDrawable()) {
            Context::Paint(color);
            ms->unref();
            return;
        }

        Top()->GetClip(&clip);

        if (!target->GetInit() && r == clip) {
            // mark target as initialized
            target->SetInit(ms);
        }

        ForceCurrent();

        GLContext::Paint(color);

        ms->unref();
    }

    void MoonWGLContext::Paint(MoonSurface *src,
        double alpha,
        double x,
        double y) {
        MoonWGLContext::Project(src, NULL, alpha, x, y);
    }

    void MoonWGLContext::Project(MoonSurface *src,
        const double *matrix,
        double alpha,
        double x,
        double y) {
        MoonWGLSurface *surface = (MoonWGLSurface *)src;
        Target *target = Top()->GetTarget();
        Rect r = target->GetData(NULL);
        Rect clip = GetClip();
        double m[16];

        if (!target->GetInit() && !IS_TRANSLUCENT(alpha) && r == clip) {
            int x0, y0;

            GetMatrix(m);
            if (matrix)
                Matrix3D::Multiply(m, matrix, m);

            if (Matrix3D::IsIntegerTranslation(m, &x0, &y0)) {
                MoonWGLSurface *surface = (MoonWGLSurface *)src;
                Rect r = Rect(x + x0,
                    y + y0,
                    surface->Width(),
                    surface->Height());

                // matching dimensions and no transformation allow us
                // to set source as initial state of target surface when
                // it is not already initialized.

                if (r == clip) {
                    target->SetInit(src);
                    return;
                }
            }
        }

        if (!HasDrawable() && !surface->HasTexture()) {
            int x0, y0;

            GetMatrix(m);
            if (matrix)
                Matrix3D::Multiply(m, matrix, m);

            // avoid GL rendering to target without previously
            // allocated hardware drawable
            if (Matrix3D::IsIntegerTranslation(m, &x0, &y0)) {
                cairo_matrix_t m;

                cairo_matrix_init_translate(&m, x0, y0);

                Context::Push(Context::AbsoluteTransform(m));
                Context::Paint(src, alpha, x, y);
                Context::Pop();
                return;
            }
        }

        GetDeviceMatrix(m);
        if (matrix)
            Matrix3D::Multiply(m, matrix, m);

        ForceCurrent();

        GLContext::Paint(src, m, alpha, x, y);
    }

    void MoonWGLContext::Blur(MoonSurface *src,
        double radius,
        double x,
        double y) {
        ForceCurrent();

        GLContext::Blur(src, radius, x, y);
    }

    void MoonWGLContext::DropShadow(MoonSurface *src,
        double dx,
        double dy,
        double radius,
        Color *color,
        double x,
        double y) {
        ForceCurrent();

        GLContext::DropShadow(src, dx, dy, radius, color, x, y);
    }

    void MoonWGLContext::ShaderEffect(MoonSurface *src,
        PixelShader *shader,
        Brush **sampler,
        int *sampler_mode,
        int n_sampler,
        Color *constant,
        int n_constant,
        int *ddxUvDdyUvPtr,
        double x,
        double y) {
        ForceCurrent();

        GLContext::ShaderEffect(src,
            shader,
            sampler, sampler_mode, n_sampler,
            constant, n_constant,
            ddxUvDdyUvPtr,
            x, y);
    }

    void MoonWGLContext::Flush() {
        ForceCurrent();
        SyncDrawable();

        GLContext::Flush();
    }

};

#endif