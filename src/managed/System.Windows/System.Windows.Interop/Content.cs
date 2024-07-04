//
// Content.cs
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

using Mono;
using System;

namespace System.Windows.Interop
{

    public sealed class Content
    {

        public Content()
        {
        }

        public double ActualHeight
        {
            get
            {
                return 768;
            }
        }

        public double ActualWidth
        {
            get
            {
                return 1024;
            }
        }

        public bool IsFullScreen
        {
            get => NativeMethods.surface_get_full_screen(Deployment.Current.Surface.Native);
            set => NativeMethods.surface_set_full_screen(Deployment.Current.Surface.Native, value);
        }

        public FullScreenOptions FullScreenOptions
        {
            get => NativeMethods.surface_get_full_screen_options(Deployment.Current.Surface.Native);
            set => NativeMethods.surface_set_full_screen_options(Deployment.Current.Surface.Native, value);
        }

        public double ZoomFactor => NativeMethods.surface_get_zoom_factor(Deployment.Current.Surface.Native);

        public event EventHandler FullScreenChanged
        {
            add => Deployment.Current.Surface.EventList.RegisterEvent(Deployment.Current.Surface, EventIds.Surface_FullScreenChangeEvent, value, Events.CreateNullSenderEventHandlerDispatcher(value));
            remove => Deployment.Current.Surface.EventList.UnregisterEvent(Deployment.Current.Surface, EventIds.Surface_FullScreenChangeEvent, value);
        }

        public event EventHandler Resized
        {
            add => Deployment.Current.Surface.EventList.RegisterEvent(Deployment.Current.Surface, EventIds.Surface_ResizeEvent, value, Events.CreateNullSenderEventHandlerDispatcher(value));
            remove => Deployment.Current.Surface.EventList.UnregisterEvent(Deployment.Current.Surface, EventIds.Surface_ResizeEvent, value);
        }

        public event EventHandler Zoomed
        {
            add => Deployment.Current.Surface.EventList.RegisterEvent(Deployment.Current.Surface, EventIds.Surface_ZoomedEvent, value, Events.CreateNullSenderEventHandlerDispatcher(value));
            remove => Deployment.Current.Surface.EventList.UnregisterEvent(Deployment.Current.Surface, EventIds.Surface_ZoomedEvent, value);
        }
    }
}
