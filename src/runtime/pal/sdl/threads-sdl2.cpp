/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

/*
 * pal-threads-windows.cpp
 *
 * Copyright 2011 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#include "config.h"

#if PAL_SDL2_THREADS

#include "pal-threads.h"

namespace Moonlight {

    SDL_TLSID MoonThread::self_tls = 0;

    MoonTlsKey::MoonTlsKey() {
        tls_index = SDL_TLSCreate();
    }

    MoonTlsKey::~MoonTlsKey() {
    }

    bool
    MoonThread::Join() {
        return false;
    }

    MoonThread::MoonThread(ThreadFunc func, gpointer func_arg)
        : func(func), func_arg(func_arg), thread(0) {
    }

    MoonThread::MoonThread() {
        /* FIXME */
        thread = SDL_ThreadID();
    }

    MoonThread::~MoonThread() {
    }

    bool
    MoonThread::IsThread(MoonThread* other) {
        /* FIXME */
        return SDL_ThreadID() == other->thread;
    }

    gpointer
    MoonThread::GetSpecific(MoonTlsKey& key) {
        return SDL_TLSGet(key.tls_index);
    }

    void
    MoonThread::SetSpecific(MoonTlsKey& key, gpointer data) {
        SDL_TLSSet(key.tls_index, data, NULL);
    }

    int MoonThread::Main(void* data) {
        MoonThread* moon_thread = (MoonThread*)data;

        SDL_TLSSet(self_tls, moon_thread, NULL);
        moon_thread->thread = SDL_ThreadID();

        void* rv = moon_thread->func(moon_thread->func_arg);

        SDL_TLSSet(self_tls, NULL, NULL);

        return (int)rv;
    }

    int MoonThread::Start(MoonThread** mt, MoonThread::ThreadFunc func, gpointer arg) {
        if (self_tls == 0)
            self_tls = SDL_TLSCreate();

        printf("Starting thread %p %p\n", func, arg);

        MoonThread* moon_thread = new MoonThread(func, arg);

        SDL_Thread* thread = SDL_CreateThread(MoonThread::Main, "MoonThread", moon_thread);
        if (thread) {
            *mt = moon_thread;
            
            return 1;
        }
        else {
            delete moon_thread;
            *mt = NULL;
            return 0;
        }
    }

    int MoonThread::StartJoinable(MoonThread** mt, MoonThread::ThreadFunc func, gpointer arg) {
        return Start(mt, func, arg);
    }

    MoonThread* MoonThread::Self() {
        // FIXME we race here
        if (self_tls == 0)
            self_tls = SDL_TLSCreate();

        MoonThread* mt = (MoonThread*)SDL_TLSGet(self_tls);
        if (mt == NULL) {
            // this happens in an unattached thread (the main thread, as well
            // as mono threads that call into moonlight)
            mt = new MoonThread();
            SDL_TLSSet(self_tls, mt, NULL);
        }

        return mt;
    }

    MoonMutex::MoonMutex(bool recursive) {
        mutex = SDL_CreateMutex();
    }

    MoonMutex::~MoonMutex() {
        SDL_DestroyMutex(mutex);
    }

    void MoonMutex::Lock() {
        SDL_LockMutex(mutex);
    }

    void MoonMutex::Unlock() {
        SDL_UnlockMutex(mutex);
    }

    MoonRWLock::MoonRWLock() {
        lock = SDL_CreateMutex();
    }

    MoonRWLock::~MoonRWLock() {
        SDL_DestroyMutex(lock);
    }

    void MoonRWLock::ReadUnlock() {
        SDL_UnlockMutex(lock);
    }

    void MoonRWLock::ReadLock() {
        SDL_LockMutex(lock);
    }

    void MoonRWLock::WriteUnlock() {
        SDL_UnlockMutex(lock);
    }

    void MoonRWLock::WriteLock() {
        SDL_LockMutex(lock);
    }

    MoonCond::MoonCond() {
        cond = SDL_CreateCond();
    }

    MoonCond::~MoonCond() {
        SDL_DestroyCond(cond);
    }

    void MoonCond::TimedWait(MoonMutex& mutex, timespec* ts) {
        // SleepConditionVariableCS(&cond, &mutex.mutex, ts->tv_sec * 1000 + ts->tv_nsec / 100000);
        SDL_CondWaitTimeout(cond, mutex.mutex, ts->tv_sec * 1000 + ts->tv_nsec / 100000);
    }
    void MoonCond::Wait(MoonMutex& mutex) {
        SDL_CondWait(cond, mutex.mutex);
    }

    void MoonCond::Signal() {
        SDL_CondSignal(cond);
    }

    void MoonCond::Broadcast() {
        SDL_CondBroadcast(cond);
    }

};

#endif
