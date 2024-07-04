//
// NativeMethods.cs
//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright 2007 Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Messaging;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace Mono
{

    // XXX keep this in sync with the enum in provider.h
    internal enum ImplicitStyleIndex
    {
        VisualTree,
        ApplicationResources,
        GenericXaml,

        Count
    };

    // XXX keep this in sync with the enum in provider.h
    [Flags]
    internal enum ImplicitStyleMask
    {
        VisualTree = 1 << ImplicitStyleIndex.VisualTree,
        ApplicationResources = 1 << ImplicitStyleIndex.ApplicationResources,
        GenericXaml = 1 << ImplicitStyleIndex.GenericXaml,

        All = (GenericXaml | VisualTree | ApplicationResources)
    };

    internal enum HttpRequestOptions
    {
        OptionsNone = 0,
        CustomHeaders = 1,
        DisableCache = 2,
        DisableFileStorage = 4, /* Data will not be written to disk. User must listen to the Write event */
        DisableAsyncSend = 8,
        ForceHttp_1_0 = 16,
    }

    internal enum DownloaderAccessPolicy
    {
        DownloaderPolicy,
        MediaPolicy,
        XamlPolicy,
        FontPolicy,
        StreamingPolicy,
        MsiPolicy,
        NoPolicy
    }

    internal enum MoonWindowType
    {
        FullScreen,
        Desktop,
        Plugin
    };

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void MentorChangedCallback(IntPtr doptr, IntPtr mentor_ptr);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void EnsureManagedPeerCallback(IntPtr doptr, Kind kind);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void ManagedRefCallback(IntPtr referer, IntPtr referent, IntPtr id);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate Size MeasureOverrideCallback(IntPtr fwe_ptr, Size availableSize, ref MoonError error);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate Size ArrangeOverrideCallback(IntPtr fwe_ptr, Size finalSize, ref MoonError error);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void StyleResourceChangedCallback(IntPtr fwe_ptr, [MarshalAs(UnmanagedType.LPUTF8Str)] string resourceKey, IntPtr style_ptr);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void FlattenTimelinesCallback(IntPtr timeline, IntPtr dep_ob, IntPtr dp);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void GetImplicitStylesCallback(IntPtr fwe_ptr, ImplicitStyleMask style_mask, out IntPtr styles_array);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr GetDefaultTemplateCallback(IntPtr fwe_ptr);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void ConvertSetterValuesCallback(IntPtr style_ptr);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void ConvertKeyframeValueCallback(Mono.Kind kind, IntPtr property, IntPtr original, out Value converted);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate ManagedStreamCallbacks GetResourceCallback(IntPtr resourceBase, IntPtr name);

    // Used in databinding to interact with managed objects
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate Value GetValueCallback();
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void SetValueCallback(IntPtr value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr CreateCustomXamlElementCallback([MarshalAs(UnmanagedType.LPUTF8Str)] string xmlns, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void SetCustomXamlAttributeCallback(IntPtr target, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void XamlHookupEventCallback(IntPtr target, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void UnmanagedEventHandler(IntPtr sender, IntPtr calldata, IntPtr closure);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void UnmanagedEventHandlerInvoker(IntPtr sender, int event_id, int token, IntPtr calldata, IntPtr closure);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void PlainEvent(IntPtr target);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void DestroyUnmanagedEvent(IntPtr target, int event_id, int token);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void AsyncResponseAvailableHandler(IntPtr response, IntPtr context);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void UnmanagedPropertyChangeHandler(IntPtr dependency_object, IntPtr propertyChangedArgs, ref MoonError error, IntPtr closure);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void UnmanagedPropertyChangeHandlerInvoker(int token, IntPtr dependency_object, IntPtr propertyChangedArgs, ref MoonError error, IntPtr closure);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void TickCallHandler(IntPtr handle);


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool InvalidateHandleDelegate(IntPtr obj_handle);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool HasMemberDelegate(IntPtr obj_handle,
                          [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool InvokeDelegate(IntPtr obj_handle,
                           [MarshalAs(UnmanagedType.LPUTF8Str)] string name,
                           [MarshalAs (UnmanagedType.LPArray, SizeParamIndex = 3)]
                           IntPtr[] args,
                           int arg_count,
                           ref Value return_value,
                           out string exc_string);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool SetPropertyDelegate(IntPtr obj_handle,
                            [MarshalAs(UnmanagedType.LPUTF8Str)] string name,
                            [MarshalAs (UnmanagedType.LPArray, SizeParamIndex = 3)]
                            IntPtr[] args,
                            int arg_count,
                            ref Value return_value,
                            out string exc_string);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool GetPropertyDelegate(IntPtr obj_handle,
                            [MarshalAs(UnmanagedType.LPUTF8Str)] string name,
                            [MarshalAs (UnmanagedType.LPArray, SizeParamIndex = 3)]
                            IntPtr[] args,
                            int arg_count,
                            ref Value return_value,
                            out string exc_string);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void HttpHeaderVisitor(IntPtr context, IntPtr name, IntPtr val);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void DomEventCallback(IntPtr context, string name, int client_x, int client_y, int offset_x, int offset_y,
        [MarshalAs(UnmanagedType.Bool)] bool alt_key,   // glib gboolean is a gint (i.e. 4 bytes just like the FX bool)
        [MarshalAs(UnmanagedType.Bool)] bool ctrl_key,
        [MarshalAs(UnmanagedType.Bool)] bool shift_key,
        int /* System.Windows.Browser.MouseButtons */ mouse_button,
        int key_code,
        int char_code,
        IntPtr domEvent);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool ImageUriFunc(IntPtr msts, int level, int posx, int posy, ref IntPtr uri);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr System_Uri_Ctor_1([MarshalAs(UnmanagedType.LPUTF8Str)] string uri_string);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr System_Uri_Ctor_2([MarshalAs(UnmanagedType.LPUTF8Str)] string uri_string, UriKind uri_kind);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr System_Uri_Ctor_3(IntPtr base_uri, [MarshalAs(UnmanagedType.LPUTF8Str)] string relative_uri);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr System_Uri_Ctor_4(IntPtr base_uri, IntPtr relative_uri);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr System_Uri_GetStringProperty(IntPtr instance);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int System_Uri_GetInt32Property(IntPtr instance);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool System_Uri_GetBooleanProperty(IntPtr instance);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr System_Uri_ToString(IntPtr instance);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool System_Uri_Equals(IntPtr a, IntPtr b);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr System_Uri_Clone(IntPtr instance);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr System_Uri_CloneWithScheme(IntPtr instance, [MarshalAs(UnmanagedType.LPUTF8Str)] string scheme);

    internal struct UriFunctions
    {
        public System_Uri_Ctor_1 ctor_1;
        public System_Uri_Ctor_2 ctor_2;
        public System_Uri_Ctor_3 ctor_3;
        public System_Uri_Ctor_4 ctor_4;
        public System_Uri_GetStringProperty get_scheme;
        public System_Uri_GetStringProperty get_host;
        public System_Uri_GetInt32Property get_port;
        public System_Uri_GetStringProperty get_fragment;
        public System_Uri_GetStringProperty get_path;
        public System_Uri_GetStringProperty get_unescaped_path;
        public System_Uri_GetStringProperty get_query;
        public System_Uri_GetStringProperty get_original_string;
        public System_Uri_GetBooleanProperty get_is_absolute;
        public System_Uri_ToString tostring;
        public System_Uri_Equals equals;
        public System_Uri_CloneWithScheme clone_with_scheme;
        public System_Uri_ToString toescapedstring;
    };

    internal static partial class NativeMethods
    {
        /*
* 
        * Don't add any P/Invokes here.
        * 
        * Add annotations (@GeneratePInvoke) to your C/C++ methods to generate the P/Invokes.
        * If the generator gets parameters wrong, you can add a @MarshalAs=<whatever> to override.
        * 
        */
        internal static Exception CreateManagedException(MoonError err)
        {
            string msg = err.Message;
            Exception ex = null;

            if (err.GCHandle.IsAllocated)
            {
                // We need to get this before calling Dispose.
                ex = err.GCHandle.Target as Exception;
            }

            err.Dispose();

            switch (err.Number)
            {
                case 1:
                default:
                    return new Exception(msg);
                case 2:
                    return new ArgumentException(msg);
                case 3:
                    return new ArgumentNullException(msg);
                case 4:
                    return new ArgumentOutOfRangeException(msg);
                case 5:
                    return new InvalidOperationException(msg);
                case 6:
                    return new XamlParseException(err.LineNumber, err.CharPosition, msg);
                case 7:
                    return new UnauthorizedAccessException(msg);
                case 8:
                    return new ExecutionEngineException(msg);
                case 9:
                    if (ex != null)
                        return ex;
                    return new Exception(msg);
                case 10:
                    return new ListenFailedException(msg);
                case 11:
                    return new SendFailedException(msg);
                case 12:
                    return new NotImplementedException(msg);
                case 13:
                    return new SecurityException(msg);
                case 14:
                    return new NotSupportedException(msg);
            }
        }

        public delegate IntPtr event_object_get_managed_object_t(IntPtr handle);
        public delegate IntPtr dependency_object_get_value_with_error_t(IntPtr dep_ob, IntPtr dp, out MoonError error);

        public static event_object_get_managed_object_t __event_object_get_managed_object;
        public static dependency_object_get_value_with_error_t __dependency_object_get_value_with_error;
        public static object event_object_get_managed_object(IntPtr handle)
        {
            Debug.Assert(__event_object_get_managed_object != null);
            IntPtr num = __event_object_get_managed_object(handle);
            return num == IntPtr.Zero ? (object)null : GCHandle.FromIntPtr(num).Target;
        }

        public static IntPtr dependency_object_get_value(IntPtr dep_ob, IntPtr dp)
        {
            MoonError error;
            var ret = dependency_object_get_value_with_error(dep_ob, dp, out error);
            if (error.Number != 0)
                throw CreateManagedException(error);
            return ret;
        }

        public static IntPtr dependency_object_get_value_with_error(IntPtr dep_ob, IntPtr dp, out MoonError error)
        {
            Debug.Assert(__dependency_object_get_value_with_error != null);
            return __dependency_object_get_value_with_error(dep_ob, dp, out error);
        }
    }
}
