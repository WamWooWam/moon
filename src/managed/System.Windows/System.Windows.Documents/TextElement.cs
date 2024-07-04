//
// System.Windows.Documents.TextElement.cs
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

using System;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using Mono;

namespace System.Windows.Documents
{
    public abstract partial class TextElement : DependencyObject
    {

        public new object FindName(string name)
        {
            return base.FindName(name);
        }

        public string Name => (string)GetValue(FrameworkElement.NameProperty);


        protected virtual AutomationPeer OnCreateAutomationPeer()
        {
            Console.WriteLine("NEIX: System.Windows.Documents.TextElement:.OnCreateAutomationPeer");
            throw new NotImplementedException(); ;
        }

        TextPointer contentStart;
        public TextPointer ContentStart
        {
            get
            {
                if (contentStart == null)
                {
                    IntPtr tp = NativeMethods.text_element_get_content_start(native);
                    if (tp != IntPtr.Zero)
                        contentStart = new TextPointer(tp);
                }
                return contentStart;
            }
        }

        TextPointer contentEnd;
        public TextPointer ContentEnd
        {
            get
            {
                if (contentEnd == null)
                {
                    IntPtr tp = NativeMethods.text_element_get_content_end(native);
                    if (tp != IntPtr.Zero)
                        contentEnd = new TextPointer(tp);
                }
                return contentEnd;
            }
        }

        TextPointer elementStart;
        public TextPointer ElementStart
        {
            get
            {
                if (elementStart == null)
                {
                    IntPtr tp = NativeMethods.text_element_get_element_start(native);
                    if (tp != IntPtr.Zero)
                        elementStart = new TextPointer(tp);
                }
                return elementStart;
            }
        }

        TextPointer elementEnd;
        public TextPointer ElementEnd
        {
            get
            {
                if (elementEnd == null)
                {
                    IntPtr tp = NativeMethods.text_element_get_element_end(native);
                    if (tp != IntPtr.Zero)
                        elementEnd = new TextPointer(tp);
                }
                return elementEnd;
            }
        }
    }
}

