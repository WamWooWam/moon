//
// TimelineMarkerRoutedEventArgs.cs
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
//

using System;
using Mono;

namespace System.Windows.Media
{
    public sealed class TimelineMarkerRoutedEventArgs : RoutedEventArgs
    {
        TimelineMarker marker;

        public TimelineMarker Marker
        {
            get => marker;
            set => NativeMethods.timeline_marker_routed_event_args_set_marker(NativeHandle, value == null ? IntPtr.Zero : value.native);
        }

        public TimelineMarkerRoutedEventArgs() : base(SafeNativeMethods.timeline_marker_routed_event_args_new(), true)
        {

        }

        internal TimelineMarkerRoutedEventArgs(IntPtr raw, bool dropref) : base(raw, dropref)
        {

        }

        internal override void AddStrongRef(IntPtr id, object value)
        {
            if (id == (IntPtr)WeakRefs.TimelineMarkerRoutedEventArgs_Marker)
                marker = (TimelineMarker)value;
            else
                base.AddStrongRef(id, value);
        }

        public void ClearStrongRef(IntPtr id, object value)
        {
            throw new NotImplementedException();
        }
    }
}
