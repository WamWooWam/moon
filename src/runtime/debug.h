/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * debug.h: 
 *
 * Copyright 2007 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

#ifndef __MOONLIGHT_DEBUG_H__
#define __MOONLIGHT_DEBUG_H__

#if defined (DEBUG) || LOGGING

#include <glib.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#endif /* DEBUG || LOGGING */

#include "runtime.h"

#ifdef PLATFORM_ANDROID
#define moon_debug(...) g_debug(__VA_ARGS__)
#else
#define moon_debug(...) printf(__VA_ARGS__)
#endif

/*
 * Stacktrace (debug) stuff
 */


#if DEBUG

void print_reftrace (const char * type, const char * typname, int refcount, bool keep);
void dump_frames (void);
void store_reftrace (void *obj, const char *done, guint32 refcount);
void free_reftrace (void *obj);
void show_reftrace (void *obj);

G_BEGIN_DECLS

char* get_reftrace (void);
char* get_stack_trace (void);
void print_stack_trace (void);
void hexdump_addr (void *addr, size_t n);

G_END_DECLS

#else

#define print_stack_trace()
#define print_reftrace(type, typname, refcount, keep)
#define dump_frames()

#endif /* DEBUG */

/*
 * Logging stuff
 */

#define LOGGING 1

#if LOGGING

#define LOG_ALSA(...)				if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_ALSA)) moon_debug (__VA_ARGS__);
#define LOG_ALSA_EX(...)			if (G_UNLIKELY (debug_flags_ex & RUNTIME_DEBUG_ALSA_EX)) moon_debug (__VA_ARGS__);
#define LOG_AUDIO(...)				if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_AUDIO)) moon_debug (__VA_ARGS__);
#define LOG_AUDIO_EX(...)			if (G_UNLIKELY (debug_flags_ex & RUNTIME_DEBUG_AUDIO_EX)) moon_debug (__VA_ARGS__);
#define LOG_PULSE(...)				if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_PULSE)) moon_debug (__VA_ARGS__);
#define LOG_PULSE_EX(...)			if (G_UNLIKELY (debug_flags_ex & RUNTIME_DEBUG_PULSE_EX)) moon_debug (__VA_ARGS__);
#define LOG_CURL(...)				if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_CURL)) moon_debug (__VA_ARGS__);
#define LOG_MARKERS(...)			if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_MARKERS)) moon_debug (__VA_ARGS__);
#define LOG_MARKERS_EX(...)			if (G_UNLIKELY (debug_flags_ex & RUNTIME_DEBUG_MARKERS_EX)) moon_debug (__VA_ARGS__);
#define LOG_MMS(...)				if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_MMS)) moon_debug (__VA_ARGS__);
#define LOG_MMS_EX(...)             if (G_UNLIKELY (debug_flags_ex & RUNTIME_DEBUG_MMS_EX)) moon_debug (__VA_ARGS__);
#define LOG_MEDIAPLAYER(...)			if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_MEDIAPLAYER)) moon_debug (__VA_ARGS__);
#define LOG_MEDIAPLAYER_EX(...)			if (G_UNLIKELY (debug_flags_ex & RUNTIME_DEBUG_MEDIAPLAYER_EX)) moon_debug (__VA_ARGS__);
#define LOG_PIPELINE(...)			if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_PIPELINE)) moon_debug (__VA_ARGS__);
#define LOG_PIPELINE_EX(...)		if (G_UNLIKELY (debug_flags_ex & RUNTIME_DEBUG_PIPELINE_EX)) moon_debug (__VA_ARGS__);
#define LOG_PIPELINE_ERROR(...)			if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_PIPELINE_ERROR)) moon_debug (__VA_ARGS__);
#define LOG_PIPELINE_ERROR_CONDITIONAL(x, ...) if (G_UNLIKELY (x && debug_flags & PIPELINE_ERROR)) moon_debug (__VA_ARGS__);
#define LOG_FRAMEREADERLOOP(...)		if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_FRAMEREADERLOOP)) moon_debug (__VA_ARGS__);
#define LOG_FFMPEG(...)				if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_FFMPEG)) moon_debug(__VA_ARGS__);
#define LOG_CODECS(...)				if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_CODECS)) moon_debug (__VA_ARGS__);
#define LOG_DP(...)				if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_DP)) moon_debug (__VA_ARGS__);
#define LOG_DOWNLOADER(...)			if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_DOWNLOADER)) moon_debug (__VA_ARGS__);
#define LOG_FONT(...)				if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_FONT)) moon_debug (__VA_ARGS__);
#define LOG_LAYOUT(...)				if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_LAYOUT)) moon_debug (__VA_ARGS__);
#define LOG_MEDIA(...)				if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_MEDIA)) moon_debug (__VA_ARGS__);
#define LOG_MEDIAELEMENT(...)			if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_MEDIAELEMENT)) moon_debug (__VA_ARGS__);
#define LOG_MEDIAELEMENT_EX(...)		if (G_UNLIKELY (debug_flags_ex & RUNTIME_DEBUG_MEDIAELEMENT_EX)) moon_debug (__VA_ARGS__);
#define LOG_MSI(...)				if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_MSI)) moon_debug (__VA_ARGS__);
#define LOG_BUFFERING(...)			if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_BUFFERING)) moon_debug (__VA_ARGS__);
#define LOG_PLAYLIST(...)			if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_PLAYLIST)) moon_debug (__VA_ARGS__);
#define LOG_TEXT(...)				if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_TEXT)) moon_debug (__VA_ARGS__);
#define LOG_XAML(...)				if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_XAML)) moon_debug (__VA_ARGS__);
#define LOG_DEPLOYMENT(...)		if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_DEPLOYMENT)) moon_debug (__VA_ARGS__);
#define LOG_MP3(...)				if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_MP3)) moon_debug (__VA_ARGS__);
#define LOG_ASF(...)			if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_ASF)) moon_debug (__VA_ARGS__);
#define LOG_VALUE(...)			if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_VALUE)) moon_debug (__VA_ARGS__);
#define LOG_DEMUXERS(...)		if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_DEMUXERS)) moon_debug (__VA_ARGS__);
#define LOG_MP4(...)			if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_MP4)) moon_debug (__VA_ARGS__);
#define LOG_EFFECT(...)			if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_EFFECT)) moon_debug (__VA_ARGS__);
#define LOG_OOB(...)			if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_OOB)) moon_debug (__VA_ARGS__);
#define LOG_CUSTOM(x,...)		if (G_UNLIKELY (debug_flags & (x))) moon_debug (__VA_ARGS__);
#define LOG_CAPTURE(...)        if (G_UNLIKELY (debug_flags & RUNTIME_DEBUG_CAPTURE)) moon_debug (__VA_ARGS__);
#define LOG_NPSTREAM(...)       if (G_UNLIKELY (debug_flags_ex & RUNTIME_DEBUG_NPSTREAM)) moon_debug (__VA_ARGS__);

#else

#define LOG_ALSA(...)
#define LOG_ALSA_EX(...)
#define LOG_AUDIO(...)
#define LOG_AUDIO_EX(...)
#define LOG_PULSE(...)
#define LOG_PULSE_EX(...)
#define LOG_CURL(...)
#define LOG_MARKERS(...)
#define LOG_MARKERS_EX(...)
#define LOG_MMS(...)
#define LOG_MMS_EX(...)
#define LOG_MEDIAPLAYER(...)
#define LOG_MEDIAPLAYER_EX(...)
#define LOG_PIPELINE(...)
#define LOG_PIPELINE_EX(...)
#define LOG_PIPELINE_ERROR(...)
#define LOG_PIPELINE_ERROR_CONDITIONAL(x, ...)
#define LOG_FRAMEREADERLOOP(...)
#define LOG_FFMPEG(...)
#define LOG_CODECS(...)
#define LOG_DP(...)
#define LOG_DOWNLOADER(...)
#define LOG_FONT(...)
#define LOG_LAYOUT(...)
#define LOG_MEDIA(...)
#define LOG_MEDIAELEMENT(...)
#define LOG_MEDIAELEMENT_EX(...)
#define LOG_MSI(...)
#define LOG_BUFFERING(...)
#define LOG_PLAYLIST(...)
#define LOG_TEXT(...)
#define LOG_XAML(...)
#define LOG_DEPLOYMENT(...)
#define LOG_MP3(...)
#define LOG_ASF(...)
#define LOG_VALUE(...)
#define LOG_DEMUXERS(...)
#define LOG_MP4(...)
#define LOG_EFFECT(...)
#define LOG_OOB(...)
#define LOG_CUSTOM(...)
#define LOG_CAPTURE(...)
#define LOG_NPSTREAM(...)

#endif /* LOGGING */

#if SANITY && defined (DEBUG)
G_BEGIN_DECLS
void moonlight_install_signal_handlers ();
G_END_DECLS
#else
#define moonlight_install_signal_handlers() 
#endif /* SANITY */

#endif /* __MOONLIGHT_DEBUG_H */

