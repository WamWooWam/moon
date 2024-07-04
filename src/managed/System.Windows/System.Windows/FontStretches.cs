//
// FontStetches.cs
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
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;

namespace System.Windows
{

    public static class FontStretches
    {
        public static FontStretch Condensed => new FontStretch(FontStretchKind.Condensed);

        public static FontStretch Expanded => new FontStretch(FontStretchKind.Expanded);

        public static FontStretch ExtraCondensed => new FontStretch(FontStretchKind.ExtraCondensed);

        public static FontStretch ExtraExpanded => new FontStretch(FontStretchKind.ExtraExpanded);

        public static FontStretch Normal => new FontStretch(FontStretchKind.Normal);

        public static FontStretch SemiCondensed => new FontStretch(FontStretchKind.SemiCondensed);

        public static FontStretch SemiExpanded => new FontStretch(FontStretchKind.SemiExpanded);

        public static FontStretch UltraCondensed => new FontStretch(FontStretchKind.UltraCondensed);

        public static FontStretch UltraExpanded => new FontStretch(FontStretchKind.UltraExpanded);
    }
}