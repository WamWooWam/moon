/* pal/compat/atomic.h
 *
 * Portable GCC/Clang replacements for the Windows Interlocked* atomic
 * operations that were previously supplied by mono/io-layer/atomic.h.
 * Included by the #else branch of every #ifdef MONO block that pulled
 * in that header.
 */

#ifndef MOON_COMPAT_ATOMIC_H
#define MOON_COMPAT_ATOMIC_H

#ifdef _WIN32

#else
#include <stdint.h>

/* InterlockedExchangeAdd: returns old value, adds val to *p. */
static inline int
InterlockedExchangeAdd (volatile int *p, int val)
{
    return __atomic_fetch_add (p, val, __ATOMIC_SEQ_CST);
}

static inline unsigned int
InterlockedExchangeAdd (volatile unsigned int *p, int val)
{
    return __atomic_fetch_add (p, (unsigned int)val, __ATOMIC_SEQ_CST);
}

/* InterlockedIncrement: increments *p, return value unused in this codebase. */
static inline void
InterlockedIncrement (volatile int *p)
{
    __atomic_fetch_add (p, 1, __ATOMIC_SEQ_CST);
}

static inline void
InterlockedIncrement (volatile unsigned int *p)
{
    __atomic_fetch_add (p, 1u, __ATOMIC_SEQ_CST);
}

/* InterlockedDecrement: returns decremented (new) value. */
static inline int
InterlockedDecrement (volatile int *p)
{
    return (int)__atomic_fetch_sub (p, 1, __ATOMIC_SEQ_CST) - 1;
}

/* InterlockedExchangePointer: atomically swaps *ptr with val, returns old.
 * void* overload handles gpointer fields (e.g. pending_unrefs in deployment.cpp). */
static inline void *
InterlockedExchangePointer (void * volatile *ptr, void *val)
{
    void *old;
    __atomic_exchange (ptr, &val, &old, __ATOMIC_SEQ_CST);
    return old;
}

static inline void *
InterlockedCompareExchangePointer (void * volatile *ptr, void *new_val, void *expected)
{
    __atomic_compare_exchange (ptr, &expected, &new_val,
                               /*weak=*/0,
                               __ATOMIC_SEQ_CST, __ATOMIC_SEQ_CST);
    return expected;
}

template<typename T>
static inline T *
InterlockedExchangePointer (T * volatile *ptr, T *val)
{
    T *old;
    __atomic_exchange (ptr, &val, &old, __ATOMIC_SEQ_CST);
    return old;
}

/* Allow NULL / nullptr as the incoming value. */
template<typename T>
static inline T *
InterlockedExchangePointer (T * volatile *ptr, decltype(nullptr))
{
    T *nval = nullptr;
    return InterlockedExchangePointer (ptr, nval);
}

/*
 * InterlockedCompareExchangePointer:
 *   If *ptr == expected, write new_val and return expected.
 *   Otherwise leave *ptr unchanged and return the actual current value.
 * Mirrors Windows semantics (returns old value in both cases).
 */
template<typename T>
static inline T *
InterlockedCompareExchangePointer (T * volatile *ptr, T *new_val, T *expected)
{
    __atomic_compare_exchange (ptr, &expected, &new_val,
                               /*weak=*/0,
                               __ATOMIC_SEQ_CST, __ATOMIC_SEQ_CST);
    return expected;
}

#endif

#endif /* MOON_COMPAT_ATOMIC_H */
