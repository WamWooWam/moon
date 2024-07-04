using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Mono;

namespace Moonlight
{
    internal class Initializer
    {
        private static RuntimeManagedCallbacks callbacks;
        private static bool ininitialized;

#pragma warning disable CA2255 // The 'ModuleInitializer' attribute should not be used in libraries
        [ModuleInitializer]
#pragma warning restore CA2255 // The 'ModuleInitializer' attribute should not be used in libraries
        internal static void InitializeMoonlight()
        {
            if(ininitialized) return;
            ininitialized = true;

            callbacks = new RuntimeManagedCallbacks()
            {
                mono_gchandle_new = &mono_gchandle_new,
                mono_gchandle_new_weakref = &mono_gchandle_new_weakref,
                mono_gchandle_get_target = &mono_gchandle_get_target,
                mono_gchandle_free = &mono_gchandle_free,
                mono_register_icalls = &mono_register_icalls,

                moon_ensure_managed_peer = &moon_ensure_managed_peer,
                moon_create_xaml_loader = &moon_create_xaml_loader
            };


            IntPtr pnt = Value.Allocate(Marshal.SizeOf(callbacks));
            Marshal.StructureToPtr(callbacks, pnt, false);

            NativeMethods.runtime_initialize_callbacks(pnt);
        }

        //
        // The handling of native<->managed object handles works a little differently here vs. using the actual mono runtime.
        //
        // We don't have MonoObject and can't pass around actual pointers to managed objects, so everything is always a GCHandle 
        // of some kind. That said, mono_gchandle_new/mono_gchandle_new_weakref should always return a *new* GCHandle instance
        // as this effectively acts like a refcount.
        //

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static IntPtr mono_gchandle_new(IntPtr obj, int pinned)
        {
            var handle = GCHandle.FromIntPtr(obj);
            Debug.Assert(handle.IsAllocated);
            Debug.Assert(handle.Target != null);

            return (IntPtr)GCHandle.Alloc(handle.Target, pinned != 0 ? GCHandleType.Pinned : GCHandleType.Normal);
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static IntPtr mono_gchandle_new_weakref(IntPtr obj, int track)
        {
            var handle = GCHandle.FromIntPtr(obj);
            Debug.Assert(handle.IsAllocated);
            Debug.Assert(handle.Target != null);

            return (IntPtr)GCHandle.Alloc(handle.Target, track != 0 ? GCHandleType.WeakTrackResurrection : GCHandleType.Weak);
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static IntPtr mono_gchandle_get_target(IntPtr obj)
        {
            // on mono this would return the GCHandle's target object as a pointer.
            // We can't do this, so just return the handle itself
            // 
            // This callback is no longer used.

            Debug.Assert(false);
            return obj;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void mono_gchandle_free(IntPtr obj)
        {
            GCHandle.FromIntPtr(obj).Free();
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int mono_register_icalls(IntPtr obj, IntPtr obj2)
        {
            NativeMethods.__event_object_get_managed_object
                = Marshal.GetDelegateForFunctionPointer<NativeMethods.event_object_get_managed_object_t>(obj);
            NativeMethods.__dependency_object_get_value_with_error
                = Marshal.GetDelegateForFunctionPointer<NativeMethods.dependency_object_get_value_with_error_t>(obj2);

            return 1;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void moon_ensure_managed_peer(IntPtr obj, Kind kind)
        {
            ApplicationLauncher.EnsureManagedPeer(obj, kind);
        }


        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static IntPtr moon_create_xaml_loader(IntPtr nativeLoader, IntPtr pluginInstnace, IntPtr surface, IntPtr resourceBase)
        {
            var loader = ApplicationLauncher.CreateXamlLoader(nativeLoader, pluginInstnace, surface, resourceBase);
            return (IntPtr)GCHandle.Alloc(loader);
        }
    }
}
