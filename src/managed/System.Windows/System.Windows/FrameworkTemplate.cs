//
// FrameworkTemplate.cs
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
using Mono.Xaml;

namespace System.Windows
{

    public abstract partial class FrameworkTemplate : DependencyObject
    {

        //
        // This is used by the parser when actually parsing the templates.
        // It uses this property to store the actual contents of the template.
        // so in <DataTemplate><FooBar /></DataTemplate> we would set
        // the Content property to FooBar. Once parsing is finished,
        // we get the FooBar value from the Content property and return
        // it from get_visual_tree.
        //
        internal DependencyObject Content
        {
            get;
            set;
        }

        object XamlContext
        {
            get; set;
        }

        internal override void AddStrongRef(IntPtr id, object value)
        {
            if (id == (IntPtr)WeakRefs.FrameworkTemplate_XamlContext)
                XamlContext = value;
            else
                base.AddStrongRef(id, value);
        }

        internal override void ClearStrongRef(IntPtr id, object value)
        {
            if (id == (IntPtr)WeakRefs.FrameworkTemplate_XamlContext)
                XamlContext = null;
            else
                base.ClearStrongRef(id, value);
        }

        internal DependencyObject GetVisualTree(DependencyObject bindingSource)
        {
            IntPtr src = bindingSource == null ? IntPtr.Zero : bindingSource.native;
            IntPtr visual_tree = NativeMethods.framework_template_get_visual_tree(native, src);
            /* GetVisualTree always returns a reffed object */
            try
            {
                return NativeDependencyObjectHelper.FromIntPtr(visual_tree) as DependencyObject;
            }
            finally
            {
                NativeMethods.event_object_unref(visual_tree);
            }
        }

        private XamlContext _context;
        private ParseTemplateFunc _func;
        internal void SetXamlBuffer(ParseTemplateFunc func, XamlContext context)
        {
            _func = func;
            _context = context;
            Value v = Value.FromObject(context);
            using (Value vv = v)
                NativeMethods.framework_template_set_xaml_buffer(native, _func, ref v, null, true);
        }
    }
}
