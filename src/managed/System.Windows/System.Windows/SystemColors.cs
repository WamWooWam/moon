//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright 2009 Novell, Inc.
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
using System.Windows;
using System.Windows.Media;

namespace System.Windows
{

    enum SystemColor
    {
        ActiveBorderColor,
        ActiveCaptionColor,
        ActiveCaptionTextColor,
        AppWorkspaceColor,
        ControlColor,
        ControlDarkColor,
        ControlDarkDarkColor,
        ControlLightColor,
        ControlLightLightColor,
        ControlTextColor,
        DesktopColor,
        GrayTextColor,
        HighlightColor,
        HighlightTextColor,
        InactiveBorderColor,
        InactiveCaptionColor,
        InactiveCaptionTextColor,
        InfoColor,
        InfoTextColor,
        MenuColor,
        MenuTextColor,
        ScrollBarColor,
        WindowColor,
        WindowFrameColor,
        WindowTextColor
    }

    public static class SystemColors
    {

        static Color GetSystemColor(SystemColor id)
        {
            IntPtr clr = NativeMethods.moon_windowing_system_get_system_color(NativeMethods.runtime_get_windowing_system(), (int)id);

            if (clr == IntPtr.Zero)
                return new Color();

            unsafe
            {
                return ((UnmanagedColor*)clr)->ToColor();
            }
        }

        public static Color ActiveBorderColor => GetSystemColor(SystemColor.ActiveBorderColor);

        public static Color ActiveCaptionColor => GetSystemColor(SystemColor.ActiveCaptionColor);

        public static Color ActiveCaptionTextColor => GetSystemColor(SystemColor.ActiveCaptionTextColor);

        public static Color AppWorkspaceColor => GetSystemColor(SystemColor.AppWorkspaceColor);

        public static Color ControlColor => GetSystemColor(SystemColor.ControlColor);

        public static Color ControlDarkColor => GetSystemColor(SystemColor.ControlDarkColor);

        public static Color ControlDarkDarkColor => GetSystemColor(SystemColor.ControlDarkDarkColor);

        public static Color ControlLightColor => GetSystemColor(SystemColor.ControlLightColor);

        public static Color ControlLightLightColor => GetSystemColor(SystemColor.ControlLightLightColor);

        public static Color ControlTextColor => GetSystemColor(SystemColor.ControlTextColor);

        public static Color DesktopColor => GetSystemColor(SystemColor.DesktopColor);

        public static Color GrayTextColor => GetSystemColor(SystemColor.GrayTextColor);

        public static Color HighlightColor => GetSystemColor(SystemColor.HighlightColor);

        public static Color HighlightTextColor => GetSystemColor(SystemColor.HighlightTextColor);

        public static Color InactiveBorderColor => GetSystemColor(SystemColor.InactiveBorderColor);

        public static Color InactiveCaptionColor => GetSystemColor(SystemColor.InactiveCaptionColor);

        public static Color InactiveCaptionTextColor => GetSystemColor(SystemColor.InactiveCaptionTextColor);

        public static Color InfoColor => GetSystemColor(SystemColor.InfoColor);

        public static Color InfoTextColor => GetSystemColor(SystemColor.InfoTextColor);

        public static Color MenuColor => GetSystemColor(SystemColor.MenuColor);

        public static Color MenuTextColor => GetSystemColor(SystemColor.MenuTextColor);

        public static Color ScrollBarColor => GetSystemColor(SystemColor.ScrollBarColor);

        public static Color WindowColor => GetSystemColor(SystemColor.WindowColor);

        public static Color WindowFrameColor => GetSystemColor(SystemColor.WindowFrameColor);

        public static Color WindowTextColor => GetSystemColor(SystemColor.WindowTextColor);
    }
}
