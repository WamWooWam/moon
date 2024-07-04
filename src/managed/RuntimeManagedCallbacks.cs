using System;
using Mono;

namespace Moonlight
{
    internal unsafe struct RuntimeManagedCallbacks
    {
        public delegate* unmanaged[Cdecl]<IntPtr, int, IntPtr> mono_gchandle_new;
        public delegate* unmanaged[Cdecl]<IntPtr, int, IntPtr> mono_gchandle_new_weakref;
        public delegate* unmanaged[Cdecl]<IntPtr, IntPtr> mono_gchandle_get_target;
        public delegate* unmanaged[Cdecl]<IntPtr, void> mono_gchandle_free;

        public delegate* unmanaged[Cdecl]<IntPtr, Kind, void> moon_ensure_managed_peer;

        public delegate* unmanaged[Cdecl]<IntPtr, byte*, byte*, byte*, int> moon_initialize_deployment_xap;
        public delegate* unmanaged[Cdecl]<byte*, byte*, int> moon_initialize_deployment_xaml;
        public delegate* unmanaged[Cdecl]<IntPtr, void> moon_destroy_managed_application;

        public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, IntPtr, IntPtr> moon_create_xaml_loader;
        public delegate* unmanaged[Cdecl]<byte*, int, int, Value**, int> moon_xaml_parser_create_from_file;
        public delegate* unmanaged[Cdecl]<byte*, int, int, Value**, int> moon_xaml_parser_create_from_text;
        public delegate* unmanaged[Cdecl]<byte*, int, int, Value*, Value**, int> moon_xaml_parser_hydrate_from_text;

        public delegate* unmanaged[Cdecl]<byte*, int, byte**, int> moon_canonicalize_file_name;
        public delegate* unmanaged[Cdecl]<byte**, byte**, byte**, int> moon_create_media_log_xml;
        public delegate* unmanaged[Cdecl]<byte*, int> moon_check_file_integrity;

        public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, int> mono_register_icalls;
    }
}