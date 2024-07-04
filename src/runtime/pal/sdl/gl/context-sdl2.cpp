/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * context-sdlgl.cpp
 *
 * Copyright 2024 Thomas May
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#include <config.h>

#include <stdlib.h>
#include <string.h>


#include "context-sdl2.h"
#include "projection.h"
#include "effect.h"
#include "region.h"

#define IS_TRANSLUCENT(x) (x * 255 < 254.5)

namespace Moonlight {

    MoonSDLGLContext::MoonSDLGLContext(MoonSDLGLSurface *surface) : GLContext(surface) {
        this->window = surface->Window();
        this->renderer = surface->Renderer();
    }

    MoonSDLGLContext::~MoonSDLGLContext() {
    }

    bool MoonSDLGLContext::Initialize() {
        SDL_GL_SetAttribute(SDL_GL_CONTEXT_MAJOR_VERSION, 3);
        SDL_GL_SetAttribute(SDL_GL_CONTEXT_MINOR_VERSION, 1);
        SDL_GL_SetAttribute(
            SDL_GL_CONTEXT_PROFILE_MASK,
            SDL_GL_CONTEXT_PROFILE_CORE);
        SDL_GL_SetAttribute(SDL_GL_DOUBLEBUFFER, 1);

        context = SDL_GL_CreateContext(window);
        if (!context) {
            fprintf(stderr, "SDL_GL_CreateContext: %s\n", SDL_GetError());
            return false;
        }

        SDL_GL_SetSwapInterval(1);
        SDL_GL_MakeCurrent(window, context);

        glewExperimental = GL_TRUE;
        GLenum glewError = glewInit();
        if (glewError != GLEW_OK) {
            fprintf(stderr, "glewInit: %s\n", glewGetErrorString(glewError));
            return false;
        }

        glGetIntegerv(GL_MAX_TEXTURE_SIZE, &maxTextureSize);
        if (maxTextureSize < 2048)
            g_warning("OpenGL max texture size: %d", maxTextureSize);

        g_warning("Moonlight: OpenGL vendor string: %s\n", glGetString(GL_VENDOR));
        g_warning("Moonlight: OpenGL renderer string: %s\n", glGetString(GL_RENDERER));
        g_warning("Moonlight: OpenGL version string: %s\n", glGetString(GL_VERSION));

        return true;
    }

    void MoonSDLGLContext::MakeCurrent() {
        SDL_GL_MakeCurrent(window, context);
    }

    void MoonSDLGLContext::ForceCurrent() {
        if (SDL_GL_GetCurrentContext() != context)
            MakeCurrent();
    }

    const char *MoonSDLGLContext::ProgramPrecisionString() {
        return "";
    }

    void MoonSDLGLContext::SetupVertexData(const double *matrix,
        double x,
        double y,
        double width,
        double height) {
        Target *target = Top()->GetTarget();
        MoonSurface *ms;
        Rect r = target->GetData(&ms);
        MoonSDLGLSurface *dst = (MoonSDLGLSurface *)ms;
        double m[16];

        if (matrix)
            Matrix3D::Init(m, matrix);
        else
            Matrix3D::Identity(m);

        GLContext::SetupVertexData(m, x, y, width, height);

        if (dst->Window()) {
            int i;

            for (i = 0; i < 4; i++) {
                GLfloat v = vertices[i][1] + vertices[i][3];

                vertices[i][1] = vertices[i][3] - v;
            }
        }

        ms->unref();
    }


    gboolean MoonSDLGLContext::HasDrawable() {
        Target *target = Top()->GetTarget();
        MoonSurface *ms;
        Rect r = target->GetData(&ms);
        MoonSDLGLSurface *dst = (MoonSDLGLSurface *)ms;
        gboolean status = FALSE;

        if (dst->Window() || dst->HasTexture())
            status = TRUE;

        ms->unref();

        return status;
    }

    void MoonSDLGLContext::SyncDrawable() {
        Target *target = Top()->GetTarget();
        Target *cairo = target->GetCairoTarget();
        MoonSurface *ms;
        Rect r = target->GetData(&ms);
        MoonSDLGLSurface *dst = (MoonSDLGLSurface *)ms;

        // clear target contents
        if (!target->GetInit()) {
            if (!dst->Window())
                GLContext::SetFramebuffer();

            glClearColor(0.0, 0.0, 0.0, 0.0);
            glClear(GL_COLOR_BUFFER_BIT);

            // mark target contents as initialized
            target->SetInit(ms);
        }

        // initialize target contents with surface
        if (target->GetInit() != ms) {
            MoonSDLGLSurface *src = (MoonSDLGLSurface *)target->GetInit();
            GLuint texture0 = src->Texture();
            GLuint program = GetProjectProgram(1.0, 0);
            GLsizei width0 = src->Width();
            GLsizei height0 = src->Height();

            if (!dst->Window())
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
            MoonSDLGLSurface *src = (MoonSDLGLSurface *)mSrc;
            GLuint texture0 = src->Texture();
            GLuint program = GetProjectProgram(1.0, 0);
            GLsizei width0 = src->Width();
            GLsizei height0 = src->Height();

            if (!dst->Window())
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

    Rect MoonSDLGLContext::GroupBounds(Group extents) {
        if (extents.r.width > maxTextureSize ||
            extents.r.height > maxTextureSize)
            return Rect();

        return extents.r;
    }

    void MoonSDLGLContext::Push(Group extents) {
        Rect r = GroupBounds(extents);

        if (!r.IsEmpty()) {
            cairo_matrix_t matrix;
            MoonSDLGLSurface *surface = new MoonSDLGLSurface(r.width, r.height);
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

    cairo_t *MoonSDLGLContext::Push(Cairo extents) {
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
                MoonSDLGLSurface *surface = new MoonSDLGLSurface(box.width,
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

    Rect MoonSDLGLContext::Pop(MoonSurface **ref) {
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

    void MoonSDLGLContext::SetFramebuffer() {
        Target *target = Top()->GetTarget();
        MoonSurface *ms;
        Rect r = target->GetData(&ms);
        MoonSDLGLSurface *dst = (MoonSDLGLSurface *)ms;

        SyncDrawable();

        if (!dst->Window())
            GLContext::SetFramebuffer();

        ms->unref();
    }

    void MoonSDLGLContext::SetScissor() {
        Target *target = Top()->GetTarget();
        MoonSurface *ms;
        Rect r = target->GetData(&ms);
        MoonSDLGLSurface *dst = (MoonSDLGLSurface *)ms;
        Rect clip;

        Top()->GetClip(&clip);

        clip.x -= r.x;
        clip.y -= r.y;

        if (dst->Window()) {
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

    void MoonSDLGLContext::Clear(Color *color) {
        ForceCurrent();

        GLContext::Clear(color);
    }

    void MoonSDLGLContext::Blit(unsigned char *data,
        int stride) {
        Target *target = Top()->GetTarget();
        MoonSurface *ms;
        Rect r = target->GetData(&ms);
        MoonSDLGLSurface *dst = (MoonSDLGLSurface *)ms;
        unsigned char *buffer = data;
        int buffer_stride = stride;

        ForceCurrent();

        // no support for clipping
        g_assert(GetClip() == r);

        // no support for blit to drawable at the moment
        g_assert(!dst->Window());

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

    void MoonSDLGLContext::BlitYV12(unsigned char *data[],
        int stride[]) {
        Target *target = Top()->GetTarget();
        MoonSurface *ms;
        Rect r = target->GetData(&ms);
        MoonSDLGLSurface *dst = (MoonSDLGLSurface *)ms;
        int size[] = { dst->Width(), dst->Height() };
        int width[] = { size[0], size[0] / 2, size[0] / 2 };
        int height[] = { size[1], size[1] / 2, size[1] / 2 };
        unsigned char *buffer[] = { data[0], data[1], data[2] };
        int buffer_stride[] = { stride[0], stride[1], stride[2] };

        ForceCurrent();

        // no support for blit to drawable at the moment
        g_assert(!dst->Window());

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

    void MoonSDLGLContext::Paint(Color *color) {
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

    void MoonSDLGLContext::Paint(MoonSurface *src,
        double alpha,
        double x,
        double y) {
        MoonSDLGLContext::Project(src, NULL, alpha, x, y);
    }

    void MoonSDLGLContext::Project(MoonSurface *src,
        const double *matrix,
        double alpha,
        double x,
        double y) {
        MoonSDLGLSurface *surface = (MoonSDLGLSurface *)src;
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
                MoonSDLGLSurface *surface = (MoonSDLGLSurface *)src;
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

    void MoonSDLGLContext::Blur(MoonSurface *src,
        double radius,
        double x,
        double y) {
        ForceCurrent();

        GLContext::Blur(src, radius, x, y);
    }

    void MoonSDLGLContext::DropShadow(MoonSurface *src,
        double dx,
        double dy,
        double radius,
        Color *color,
        double x,
        double y) {
        ForceCurrent();

        GLContext::DropShadow(src, dx, dy, radius, color, x, y);
    }

    void MoonSDLGLContext::ShaderEffect(MoonSurface *src,
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

    void MoonSDLGLContext::Flush() {
        ForceCurrent();
        SyncDrawable();

        GLContext::Flush();
    }

};
