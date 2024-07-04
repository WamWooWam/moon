//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright (C) 2010 Novell, Inc (http://www.novell.com)
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

using System.Windows.Browser;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Interop;
using System;
using System.Globalization;

namespace Mono
{

    internal static class ScriptObjectHelper
    {

        static ScriptObject comparer;
        static bool isChrome;
        static ScriptObjectHelper()
        {
            throw new NotImplementedException();
        }

        public static bool TryChangeType(object value, Type type, CultureInfo culture, out object ret)
        {
            throw new NotImplementedException();

        }

        public static object FromValue(Value v)
        {
            throw new NotImplementedException();
        }

        public static void ToValue(ref Value v, object o)
        {
            throw new NotImplementedException();
        }

        public static T CreateInstance<T>(IntPtr ptr)
        {
            ConstructorInfo i = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                        null, new Type[] { typeof(IntPtr) }, null);
            object o = i.Invoke(new object[] { ptr });
            return (T)o;
        }

        public static object CreateInstance(Type type, IntPtr ptr)
        {
            ConstructorInfo i = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                        null, new Type[] { typeof(IntPtr) }, null);
            object o = i.Invoke(new object[] { ptr });
            return o;
        }

    }

}
