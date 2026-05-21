/* pal/gtk/config.h
 *
 * Static configuration header for the GTK PAL backend.
 * Picked up automatically because pal/gtk/ is on the include path when
 * MOON_PAL=gtk.  Feature flags that vary at build time (e.g. HAVE_CURL) are
 * injected as -D definitions by CMake so they do not need to live here.
 */

#ifndef PAL_CONFIG_H
#define PAL_CONFIG_H 1

/* Threading — GTK PAL uses POSIX threads via GLib. */
#define PAL_THREADS_PTHREADS 1

/* Windowing — GTK2. */
#define PAL_GTK_WINDOWING    1

/* Font service — GTK PAL uses fontconfig for font enumeration. */
#define PAL_GTK_FONTSERVICE         1
#define PAL_FONTCONFIG_FONTSERVICE  1

/* cairo_pattern_set_color_tolerance is a non-standard extension not present
 * in all cairo builds — leave undefined unless actually available. */

#include <math.h>
#include <stddef.h>
#include <stdint.h>

/* HUGE is an obsolete alias for HUGE_VAL; define it if missing. */
#ifndef HUGE
#define HUGE HUGE_VAL
#endif

/* Build identity strings used by openfile.cpp and similar UI code. */
#ifndef PACKAGE_STRING
#define PACKAGE_STRING "Moonlight"
#endif
#ifndef VERSION
#define VERSION "4.0"
#endif

#endif /* PAL_CONFIG_H */
