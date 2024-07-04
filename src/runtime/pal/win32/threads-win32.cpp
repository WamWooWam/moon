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

#if PAL_THREADS_WINDOWS

#include "pal-threads.h"

namespace Moonlight {


    MoonTlsKey::MoonTlsKey() {
        tls_index = TlsAlloc();
    }

    MoonTlsKey::~MoonTlsKey() {
        TlsFree(tls_index);
    }

    bool
    MoonThread::Join() {
        /* FIXME */
        return false;
    }


    MoonThread::MoonThread(ThreadFunc func, gpointer func_arg)
        : func(func), func_arg(func_arg), thread(nullptr), threadId(0) {
    }

    MoonThread::MoonThread() {
        /* FIXME */
        thread = GetCurrentThread();
    }

    MoonThread::~MoonThread() {
    }

    bool
    MoonThread::IsThread(MoonThread* other) {
        /* FIXME */
        return GetCurrentThread() == other->thread;
    }

    gpointer
    MoonThread::GetSpecific(MoonTlsKey& key) {
        return TlsGetValue(key.tls_index);
    }

    void
    MoonThread::SetSpecific(MoonTlsKey& key, gpointer data) {
        TlsSetValue(key.tls_index, data);
    }

    DWORD WINAPI MoonThread::Main(LPVOID data) {
        MoonThread* moon_thread = (MoonThread*)data;

        TlsSetValue(self_tls, moon_thread);

        void* rv = moon_thread->func(moon_thread->func_arg);

        TlsSetValue(self_tls, NULL);

        return (DWORD)rv;
    }

    DWORD MoonThread::self_tls = 0;

    int MoonThread::Start(MoonThread** mt, MoonThread::ThreadFunc func, gpointer arg) {
        if (self_tls == 0)
            self_tls = TlsAlloc();

        MoonThread* moon_thread = new MoonThread(func, arg);

        moon_thread->thread = CreateThread(NULL, 0, MoonThread::Main, moon_thread, 0, &moon_thread->threadId);
        if (moon_thread->thread) {
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
            self_tls = TlsAlloc();

        MoonThread* mt = (MoonThread*)TlsGetValue(self_tls);
        if (mt == NULL) {
            // this happens in an unattached thread (the main thread, as well
            // as mono threads that call into moonlight)
            mt = new MoonThread();
            TlsSetValue(self_tls, mt);
        }

        return mt;
    }

    MoonMutex::MoonMutex(bool recursive) {
        InitializeCriticalSection(&mutex);
    }

    MoonMutex::~MoonMutex() {
        DeleteCriticalSection(&mutex);
    }

    void MoonMutex::Lock() {
        EnterCriticalSection(&mutex);
    }

    void MoonMutex::Unlock() {
        LeaveCriticalSection(&mutex);
    }

    MoonRWLock::MoonRWLock() {
        InitializeSRWLock(&lock);
    }

    MoonRWLock::~MoonRWLock() {
        // ReleaseSRWLockExclusive (&lock);
    }

    void MoonRWLock::ReadUnlock() {
        ReleaseSRWLockShared(&lock);
    }

    void MoonRWLock::ReadLock() {
        AcquireSRWLockShared(&lock);
    }

    void MoonRWLock::WriteUnlock() {
        ReleaseSRWLockExclusive(&lock);
    }

    void MoonRWLock::WriteLock() {
        AcquireSRWLockExclusive(&lock);
    }

    MoonCond::MoonCond() {
        InitializeConditionVariable(&cond);
    }

    MoonCond::~MoonCond() {
        // ConditionVariable (&cond);
    }

    void MoonCond::TimedWait(MoonMutex& mutex, timespec* ts) {
        SleepConditionVariableCS(&cond, &mutex.mutex, ts->tv_sec * 1000 + ts->tv_nsec / 100000);
    }
    void MoonCond::Wait(MoonMutex& mutex) {
        SleepConditionVariableCS(&cond, &mutex.mutex, INFINITE);
    }

    void MoonCond::Signal() {
        WakeConditionVariable(&cond);
    }

    void MoonCond::Broadcast() {
        WakeAllConditionVariable(&cond);
    }

};

#endif
