#ifndef PAL_CONFIG_H
#define PAL_CONFIG_H 1

#define PAL_SDL2_THREADS 1
#define PAL_SDL2_WINDOWING 1
#define USE_SDL2_GL 1

#ifdef _WIN32
#define PAL_DWRITE_FONTSERVICE 1
#else
#define PAL_FONTCONFIG_FONTSERVICE 1
#endif

#define USE_GLEW 1
#define HAVE_CURL 1

#ifndef _WIN32
/* posix_memalign is declared in <stdlib.h> on POSIX; suppress pal.h's extern "C" re-declaration. */
#define HAVE_POSIX_MEMALIGN 1
#endif

#include <math.h>
#include <stddef.h>
#include <stdint.h>

/* HUGE is from the deprecated <values.h>; on Linux it may not be defined
   after <math.h>. Use HUGE_VAL (C99) as the fallback. */
#ifndef HUGE
#define HUGE HUGE_VAL
#endif

#ifdef _WIN32
#include <intrin.h>


/* ---- POSIX compat ---- */

/* gmtime_r(timep, result) — reverse of gmtime_s on Windows */
#include <time.h>
static __forceinline struct tm *
gmtime_r(const time_t *timep, struct tm *result) {
    gmtime_s(result, timep);
    return result;
}

/* S_ISLNK — Windows has no symlinks in the traditional POSIX sense */
#ifndef S_ISLNK
#define S_ISLNK(m) (0)
#endif

/* pipe(fds) — POSIX wrapper around _pipe */
#include <io.h>
#include <fcntl.h>
static __forceinline int pipe(int fds[2]) {
    return _pipe(fds, 4096, _O_BINARY);
}

/* PATH_MAX */
#include <stdlib.h>
#ifndef PATH_MAX
#define PATH_MAX _MAX_PATH
#endif

/* PACKAGE_STRING / VERSION */
#ifndef PACKAGE_STRING
#define PACKAGE_STRING "Moonlight"
#endif
#ifndef VERSION
#define VERSION "4.0"
#endif

/* HUGE (obsolete alias for HUGE_VAL) */
#ifndef HUGE
#define HUGE HUGE_VAL
#endif

#define strtok_r strtok_s

static __forceinline int ffsl(long x) {
    unsigned long i;

    if (_BitScanForward(&i, x))
        return (i + 1);
    return (0);
}

static __forceinline int ffs(int x) {
    return (ffsl(x));
}

#endif

#endif /* PAL_CONFIG_H */