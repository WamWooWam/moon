#ifndef PAL_CONFIG_H
#define PAL_CONFIG_H 1

#define PAL_THREADS_WINDOWS 1
#define PAL_WIN32_WINDOWING 1
#define PAL_WIN32_FONTSERVICE 1
#define HAVE_CURL 1

#define USE_GLEW 1
#define USE_WGL 1

#include <math.h>
#include <stddef.h>
#include <stdint.h>
#include <intrin.h>

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