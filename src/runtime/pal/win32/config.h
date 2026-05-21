#ifndef PAL_CONFIG_H
#define PAL_CONFIG_H 1

#define PAL_THREADS_WINDOWS 1
#define PAL_WIN32_WINDOWING 1
#define PAL_DWRITE_FONTSERVICE 1
#define HAVE_CURL 1

#define USE_GLEW 1
#define USE_WGL 1

#define GLIB_IS_EGLIB 1

#include <math.h>
#include <stddef.h>
#include <stdint.h>
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

#endif /* PAL_CONFIG_H */