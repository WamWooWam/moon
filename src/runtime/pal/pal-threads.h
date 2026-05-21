/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#ifndef MOON_PAL_THREADS_H
#define MOON_PAL_THREADS_H

#include "pal.h"

#include <glib.h>

#if PAL_THREADS_PTHREADS
#include <pthread.h>
#elif PAL_SDL2_THREADS
#include <SDL.h>
#elif PAL_THREADS_WINDOWS
#else
#error "config.h not included before #including pal-threads.h"
#endif

namespace Moonlight {

    class MoonMutex;
    class MoonCond;
    class MoonThread;
    class MoonTlsKey;

    class MoonTlsKey {
    public:
        MoonTlsKey();
        ~MoonTlsKey();

    private:
        friend class MoonThread;
#if PAL_THREADS_PTHREADS
        pthread_key_t tls_key;
#elif PAL_THREADS_WINDOWS
        DWORD tls_index;
#elif PAL_SDL2_THREADS
        SDL_TLSID tls_index;
#endif
    };

    class MoonThread {
    public:
        typedef gpointer (*ThreadFunc)(gpointer);

        bool Join();

        static int Start(MoonThread** thread, ThreadFunc func, gpointer arg = NULL);
        static int StartJoinable(MoonThread** thread, ThreadFunc func, gpointer arg = NULL);

        static bool IsThread(MoonThread* other);

        static MoonThread* Self();

        static gpointer GetSpecific(MoonTlsKey& key);
        static void SetSpecific(MoonTlsKey& key, gpointer data);

#if PAL_THREADS_WINDOWS
        // Initialize a COM apartment on the calling thread, tracked by this
        // thread's MoonThread so it is balanced with CoUninitialize on exit.
        // Idempotent per thread, and safe to call when COM is already
        // initialized (e.g. by the .NET runtime) — it defers to the existing
        // apartment rather than fighting it. `sta` selects single-threaded
        // (apartment) vs multi-threaded.
        static void InitComApartment(bool sta);
#endif

    private:
        // we want to outlaw copy ctors and operator= since the platform types might
        // not be copyable/refcounted/etc, and the dtor called on the other value will
        // destroy the platform type.
        MoonThread(const MoonThread&) {}
        // same with operator=
        MoonThread& operator=(const MoonThread& rhs) { return *this; }

        ThreadFunc func;
        gpointer func_arg;

        MoonThread(ThreadFunc func, gpointer func_arg);
        MoonThread();

        ~MoonThread();

#if PAL_THREADS_PTHREADS
        pthread_t pt;
        static pthread_key_t self_tls;
        static void* Main(void* data);
        static void Cleanup(void* data);
#elif PAL_THREADS_WINDOWS
        HANDLE thread;
        DWORD threadId;
        // true for objects we synthesize in Self() for unattached threads —
        // those are owned by the self_tls slot and freed on thread exit.
        // Threads created via Start() are owned by their creator.
        bool synthetic;
        // COM apartment ownership for this thread: 0 = not attempted,
        // 1 = we initialized it and must CoUninitialize on exit, -1 = COM was
        // already initialized in another apartment, so we leave it alone.
        int com_state;
        static DWORD self_tls; // FLS index
        static void EnsureSelfTls();
        static BOOL WINAPI InitSelfTls(PINIT_ONCE once, PVOID param, PVOID* ctx);
        static DWORD WINAPI Main(LPVOID data);
        static VOID WINAPI SelfCleanup(PVOID data);
#elif PAL_SDL2_THREADS
        SDL_threadID thread;
        static SDL_TLSID self_tls;
        static int Main(void* data);
#endif
    };

    class MoonMutex {
    public:
        MoonMutex(bool recursive = false);
        ~MoonMutex();

        void Lock();
        void Unlock();

    private:
        // we want to outlaw copy ctors and operator= since the platform types might
        // not be copyable/refcounted/etc, and the dtor called on the other value will
        // destroy the platform type.
        MoonMutex(const MoonMutex&) {}
        // same with operator=
        MoonMutex& operator=(const MoonMutex& rhs) { return *this; }

        friend class MoonCond;
#if PAL_THREADS_PTHREADS
        pthread_mutex_t mutex;
#elif PAL_THREADS_WINDOWS
        CRITICAL_SECTION mutex;
#elif PAL_SDL2_THREADS
        SDL_mutex* mutex;
#endif
    };

    class MoonRWLock {
    public:
        MoonRWLock();
        ~MoonRWLock();

        void ReadLock();
        void ReadUnlock();

        void WriteUnlock();
        void WriteLock();

    private:
        // we want to outlaw copy ctors and operator= since the platform types might
        // not be copyable/refcounted/etc, and the dtor called on the other value will
        // destroy the platform type.
        MoonRWLock(const MoonRWLock&) {}
        // same with operator=
        MoonRWLock& operator=(const MoonRWLock& rhs) { return *this; }

#if PAL_THREADS_PTHREADS
#if HAVE_PTHREAD_RWLOCK_RDLOCK
        pthread_rwlock_t lock;
#else
        pthread_mutex_t lock;
#endif
#elif PAL_THREADS_WINDOWS
        SRWLOCK lock;
#elif PAL_SDL2_THREADS
        SDL_mutex* lock;
#endif
    };

    class MoonCond {
    public:
        MoonCond();
        ~MoonCond();

        void TimedWait(MoonMutex& mutex, timespec* ts);
        void Wait(MoonMutex& mutex);
        void Signal();
        void Broadcast();

    private:
        // we want to outlaw copy ctors and operator= since the platform types might
        // not be copyable/refcounted/etc, and the dtor called on the other value will
        // destroy the platform type.
        MoonCond(const MoonCond&) {}
        // same with operator=
        MoonCond& operator=(const MoonCond& rhs) { return *this; }

#if PAL_THREADS_PTHREADS
        pthread_cond_t cond;
#elif PAL_THREADS_WINDOWS
        CONDITION_VARIABLE cond;
#elif PAL_SDL2_THREADS
        SDL_cond* cond;
#endif
    };


};

#endif
