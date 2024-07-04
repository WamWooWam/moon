using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Moonlight
{
    internal static class MarshalEx
    {
        public static string PtrToStringUtf8(IntPtr ptr)
        {
#if NETCOREAPP1_1_OR_GREATER
            return Marshal.PtrToStringUTF8(ptr);
#else
            //#error Implement this
            return "";
#endif
        }
    }
}
