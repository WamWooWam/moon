//
// KeyEventArgs.cs
//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright 2008 Novell, Inc.
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
using System.Windows;
using Mono;

namespace System.Windows.Input
{

    public sealed class KeyEventArgs : RoutedEventArgs
    {
        internal KeyEventArgs(IntPtr raw) : base(raw, false)
        {
        }

        internal KeyEventArgs() : base(SafeNativeMethods.key_event_args_new(), true)
        {
        }

        // Don't change the declaration order of these properties as it'll affect the output
        // in drt 912.
        public int PlatformKeyCode => NativeMethods.key_event_args_get_platform_key_code(NativeHandle);

        public bool Handled
        {
            get => EventHandled;
            set => EventHandled = value;
        }

        public Key Key => (Key)NativeMethods.key_event_args_get_key(NativeHandle);
    }
}