#ifndef PAL_CONFIG_H
#define PAL_CONFIG_H 1

#define PAL_SDL2_THREADS 1
#define PAL_SDL2_WINDOWING 1
#define PAL_SDL2_FONTSERVICE 1
#define USE_SDL2_GL 1

#define USE_GLEW 1
#define HAVE_CURL 1

#include <math.h>
#include <stddef.h>
#include <stdint.h>
#include <intrin.h>

#ifdef _WIN32
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

#define InterlockedExchangeAdd _InterlockedExchangeAdd
#define InterlockedIncrement _InterlockedIncrement
#define CreateSymbolicLinkA _CreateSymbolicLinkA

unsigned int __stdcall InterlockedExchangeAdd(unsigned int* Addend, unsigned int Value);
unsigned int __stdcall InterlockedIncrement(unsigned int* Addend);
int __stdcall CreateSymbolicLinkA(
    const char* lpSymlinkFileName,
    const char* lpTargetFileName,
    unsigned int dwFlags);

#define MAX_PATH 260

#endif

#endif /* PAL_CONFIG_H */