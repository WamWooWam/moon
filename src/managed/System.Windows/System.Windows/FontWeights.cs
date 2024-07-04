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

    public static class FontWeights
    {
        public static FontWeight Black => new FontWeight(FontWeightKind.Black);

        public static FontWeight Bold => new FontWeight(FontWeightKind.Bold);

        public static FontWeight ExtraBlack => new FontWeight(FontWeightKind.ExtraBlack);

        public static FontWeight ExtraBold => new FontWeight(FontWeightKind.ExtraBold);

        public static FontWeight ExtraLight => new FontWeight(FontWeightKind.ExtraLight);

        public static FontWeight Light => new FontWeight(FontWeightKind.Light);

        public static FontWeight Medium => new FontWeight(FontWeightKind.Medium);

        public static FontWeight Normal => new FontWeight(FontWeightKind.Normal);

        public static FontWeight SemiBold => new FontWeight(FontWeightKind.SemiBold);

        public static FontWeight Thin => new FontWeight(FontWeightKind.Thin);
    }
}