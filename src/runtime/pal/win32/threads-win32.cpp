/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

/*
 * threads-win32.cpp
 *
 * Copyright 2011 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#include "config.h"

#if PAL_THREADS_WINDOWS

#include "pal-threads.h"

// for the shared MoonEnsureCOM helper (and the COM declarations it needs)
#include "pal-win32.h"

namespace Moonlight {


    MoonTlsKey::MoonTlsKey() {
        tls_index = TlsAlloc();
    }

    MoonTlsKey::~MoonTlsKey() {
        TlsFree(tls_index);
    }

    MoonThread::MoonThread(ThreadFunc func, gpointer func_arg)
        : func(func), func_arg(func_arg), thread(nullptr), threadId(0), synthetic(false), com_state(0) {
    }

    MoonThread::MoonThread()
        : func(nullptr), func_arg(nullptr), thread(nullptr), threadId(GetCurrentThreadId()), synthetic(true), com_state(0) {
    }

    MoonThread::~MoonThread() {
        // Synthetic objects never own a real handle (just the caller's
        // pseudo-handle, which must not be closed).
        if (thread != nullptr)
            CloseHandle(thread);
    }

    bool
    MoonThread::Join() {
        if (thread == nullptr)
            return false;
        return WaitForSingleObject(thread, INFINITE) == WAIT_OBJECT_0;
    }

    bool
    MoonThread::IsThread(MoonThread* other) {
        return other != nullptr && other->threadId == GetCurrentThreadId();
    }

    gpointer
    MoonThread::GetSpecific(MoonTlsKey& key) {
        return TlsGetValue(key.tls_index);
    }

    void
    MoonThread::SetSpecific(MoonTlsKey& key, gpointer data) {
        TlsSetValue(key.tls_index, data);
    }

    DWORD MoonThread::self_tls = FLS_OUT_OF_INDEXES;

    static INIT_ONCE g_self_tls_once = INIT_ONCE_STATIC_INIT;

    // Runs on thread exit for every thread that stored a value in self_tls.
    // Only frees objects we synthesized in Self(); threads started via
    // Start() are owned by their creator and may outlive the thread (e.g.
    // a later Join()).
    VOID WINAPI MoonThread::SelfCleanup(PVOID data) {
        MoonThread* mt = (MoonThread*)data;
        if (mt == nullptr)
            return;

        // Balance our CoInitializeEx on the same thread that made it.
        if (mt->com_state == 1)
            CoUninitialize();

        if (mt->synthetic)
            delete mt;
    }

    void MoonThread::InitComApartment(bool sta) {
        MoonThread* self = Self();
        if (self->com_state != 0)
            return; // already attempted on this thread

        // Reuse the shared helper so the apartment/ownership logic lives in
        // one place. owned == true means we took a ref to balance on exit.
        bool owned = false;
        MoonEnsureCOM(sta ? COINIT_APARTMENTTHREADED : COINIT_MULTITHREADED, &owned);
        self->com_state = owned ? 1 : -1;
    }

    BOOL WINAPI MoonThread::InitSelfTls(PINIT_ONCE, PVOID, PVOID*) {
        // FLS (unlike TLS) invokes a destructor callback on thread exit,
        // which is how we avoid leaking synthesized objects.
        self_tls = FlsAlloc(&MoonThread::SelfCleanup);
        return TRUE;
    }

    void MoonThread::EnsureSelfTls() {
        InitOnceExecuteOnce(&g_self_tls_once, &MoonThread::InitSelfTls, nullptr, nullptr);
    }

    DWORD WINAPI MoonThread::Main(LPVOID data) {
        MoonThread* moon_thread = (MoonThread*)data;

        FlsSetValue(self_tls, moon_thread);

        // Worker threads we spawn join the multi-threaded apartment: it needs
        // no message pump and suits background COM use (WIC decoding, etc.).
        // SelfCleanup balances this on thread exit.
        InitComApartment(false);

        void* rv = moon_thread->func(moon_thread->func_arg);

        // Leave the slot set; SelfCleanup will see synthetic == false and
        // skip the delete, leaving ownership with the creator.
        return (DWORD)(DWORD_PTR)rv;
    }

    int MoonThread::Start(MoonThread** mt, MoonThread::ThreadFunc func, gpointer arg) {
        EnsureSelfTls();

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
        // Win32 threads are always joinable through their handle.
        return Start(mt, func, arg);
    }

    MoonThread* MoonThread::Self() {
        EnsureSelfTls();

        MoonThread* mt = (MoonThread*)FlsGetValue(self_tls);
        if (mt == NULL) {
            // this happens in an unattached thread (the main thread, as well
            // as mono threads that call into moonlight)
            mt = new MoonThread();
            FlsSetValue(self_tls, mt);
        }

        return mt;
    }

    MoonMutex::MoonMutex(bool recursive) {
        // A Win32 CRITICAL_SECTION is always recursive, so the non-recursive
        // request can't be honored. No in-tree caller relies on non-recursive
        // deadlock semantics, so this is intentional.
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
        // SRWLOCK requires no explicit destruction.
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
        // CONDITION_VARIABLE requires no explicit destruction.
    }

    void MoonCond::TimedWait(MoonMutex& mutex, timespec* ts) {
        // ts is treated as a relative timeout (the contract the sole in-tree
        // caller relies on), unlike pthread_cond_timedwait's absolute time.
        DWORD ms = (DWORD)(ts->tv_sec * 1000 + ts->tv_nsec / 1000000);
        SleepConditionVariableCS(&cond, &mutex.mutex, ms);
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
