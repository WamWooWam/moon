//
// FrameworkElement.cs
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
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Automation;
using System.Windows.Controls.Primitives;

namespace System.Windows
{
    public abstract partial class FrameworkElement : UIElement
    {

        static UnmanagedEventHandlerInvoker template_applied = (_sender, _event_id, _token, _calldata, _closure) =>
            Events.SafeDispatcher(
                (IntPtr target, IntPtr calldata, IntPtr closure) =>
                    ((FrameworkElement)NativeDependencyObjectHelper.FromIntPtr(closure)).InvokeOnApplyTemplate())(_sender, _calldata, _closure);

        static UnmanagedEventHandler invoke_loaded_cb = (IntPtr sender, IntPtr eventargs, IntPtr calldata) =>
        {
            try
            {
                ((FrameworkElement)NativeDependencyObjectHelper.Lookup(sender)).InvokeLoaded();
            }
            catch
            {

            }
            finally
            {
                NativeMethods.event_object_do_emit_current_context(sender, EventIds.UIElement_LoadedEvent, IntPtr.Zero);
            }
        };

        public event EventHandler LayoutUpdated
        {
            add
            {
                if (layoutUpdatedListeners == null)
                    layoutUpdatedListeners = new List<KeyValuePair<EventHandler, WeakLayoutUpdatedListener>>();

                // Store each delegate and weak listener here so that the delegate lives as long as
                // the FE its attached to and also so that we can easily disable the weak listeners later.
                var listener = new WeakLayoutUpdatedListener(Deployment.Current, value);
                layoutUpdatedListeners.Add(new KeyValuePair<EventHandler, WeakLayoutUpdatedListener>(value, listener));
            }
            remove
            {
                if (layoutUpdatedListeners == null)
                    return;

                for (int i = 0; i < layoutUpdatedListeners.Count; i++)
                {
                    if (layoutUpdatedListeners[i].Key == value)
                    {
                        layoutUpdatedListeners[i].Value.Detach();
                        layoutUpdatedListeners.RemoveAt(i);
                    }
                }
            }
        }

        static MeasureOverrideCallback measure_cb = InvokeMeasureOverride;
        static ArrangeOverrideCallback arrange_cb = InvokeArrangeOverride;
        static GetDefaultTemplateCallback get_default_template_cb = InvokeGetDefaultTemplate;
        static StyleResourceChangedCallback style_resource_changed_cb = InvokeStyleResourceChanged;
        List<KeyValuePair<EventHandler, WeakLayoutUpdatedListener>> layoutUpdatedListeners;

        private static bool UseNativeLayoutMethod(Type type)
        {
            return type == typeof(FrameworkElement)
                || type == typeof(Canvas)
                || type == typeof(Grid);
        }

        bool OverridesGetDefaultTemplate()
        {
            return this is ContentPresenter
                || this is ItemsPresenter
                || this is ItemsControl
                || this is ContentControl
                || this is Viewbox;
        }

        private bool OverridesLayoutMethod(string name)
        {
            var method = GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (method == null)
                return false;

            if (!method.IsVirtual || !method.IsFamily)
                return false;

            if (method.ReturnType != typeof(Size))
                return false;

            if (UseNativeLayoutMethod(method.DeclaringType))
                return false;

            var parameters = method.GetParameters();
            if (parameters.Length != 1 || parameters[0].ParameterType != typeof(Size))
                return false;

            return true;
        }

        private new void Initialize()
        {
            // FIXME this should not be handled using Events.AddHandler, since those handlers are removable via the plugin

            // hook up the TemplateApplied callback so we
            // can notify controls when their template has
            // been instantiated as a visual tree.
            Events.AddHandler(this, EventIds.FrameworkElement_TemplateAppliedEvent, template_applied);
            Events.AddOnEventHandler(this, EventIds.UIElement_LoadedEvent, invoke_loaded_cb);

            MeasureOverrideCallback measure = null;
            ArrangeOverrideCallback arrange = null;
            GetDefaultTemplateCallback getTemplate = null;
            StyleResourceChangedCallback styleResourceChanged = null;

            if (OverridesLayoutMethod("MeasureOverride"))
                measure = measure_cb;
            if (OverridesLayoutMethod("ArrangeOverride"))
                arrange = arrange_cb;
            if (OverridesGetDefaultTemplate())
                getTemplate = get_default_template_cb;
            styleResourceChanged = style_resource_changed_cb;

            NativeMethods.framework_element_register_managed_overrides(native, measure, arrange, getTemplate, styleResourceChanged);
        }

        internal void SetImplicitStyles()
        {
            // FIXME: do we need to limit this to generic.xaml (or possibly appresources + generic.xaml)?

            // see the code in xaml.cpp which does the
            // same thing (search for
            // ->SetImplicitStyles) and make sure to
            // keep them in sync.
            NativeMethods.framework_element_set_implicit_styles(native, ImplicitStyleMask.All, IntPtr.Zero);
        }

        void SetImplicitStyles(ImplicitStyleMask style_mask, Style[] styles)
        {
            IntPtr styles_array = Value.Allocate(IntPtr.Size * styles.Length);
            for (int i = 0; i < styles.Length; i++)
                Marshal.WriteIntPtr(styles_array, i * IntPtr.Size, styles[i] == null ? IntPtr.Zero : styles[i].native);

            NativeMethods.framework_element_set_implicit_styles(native, style_mask, styles_array);
        }

        void ClearImplicitStyles(ImplicitStyleMask style_mask)
        {
            NativeMethods.framework_element_clear_implicit_styles(native, style_mask);
        }

        internal bool ApplyTemplate()
        {
            return NativeMethods.framework_element_apply_template(native);
        }

        public new object FindName(string name)
        {
            return base.FindName(name);
        }

        internal void SetTemplateBinding(DependencyProperty dp, TemplateBindingExpression tb)
        {
            try
            {
                SetValue(dp, tb);
            }
            catch
            {
                // Do nothing here - The DP should still have its default value
            }
        }

        public BindingExpressionBase SetBinding(DependencyProperty dp, Binding binding)
        {
            return BindingOperations.SetBinding(this, dp, binding);
        }

        protected virtual Size MeasureOverride(Size availableSize)
        {
            return NativeMethods.framework_element_measure_override(native, availableSize);
        }

        protected virtual Size ArrangeOverride(Size finalSize)
        {
            return NativeMethods.framework_element_arrange_override(native, finalSize);
        }

        public new DependencyObject Parent
        {
            get; private set;
        }

        internal override void AddStrongRef(IntPtr id, object value)
        {
            if (id == (IntPtr)WeakRefs.FrameworkElement_LogicalParent)
            {
                Parent = (DependencyObject)value;
            }
            else
            {
                base.AddStrongRef(id, value);
            }
        }

        internal override void ClearStrongRef(IntPtr id, object value)
        {
            if (id == (IntPtr)WeakRefs.FrameworkElement_LogicalParent)
            {
                Parent = null;
            }
            else
            {
                base.ClearStrongRef(id, value);
            }
        }

        public event EventHandler<ValidationErrorEventArgs> BindingValidationError;

        internal virtual void InvokeOnApplyTemplate()
        {
            OnApplyTemplate();
        }

        internal virtual void InvokeLoaded()
        {

        }

        class DeepVisualTreeEnumerator : IEnumerator<FrameworkElement>
        {
            FrameworkElement root;
            Stack<FrameworkElement> stack;
            bool started;
            bool skip_subtree;

            public DeepVisualTreeEnumerator(FrameworkElement root)
            {
                this.root = root;

                stack = new Stack<FrameworkElement>();
            }

            public bool MoveNext()
            {
                if (!started)
                {
                    started = true;
                    stack.Push(root);
                    return true;
                }

                if (stack.Count == 0)
                    return false;

                FrameworkElement fwe = stack.Pop();
                if (skip_subtree)
                {
                    skip_subtree = false;
                }
                else
                {
                    int count = VisualTreeHelper.GetChildrenCount(fwe);
                    for (int i = 0; i < count; i++)
                        stack.Push((FrameworkElement)VisualTreeHelper.GetChild(fwe, i));
                }

                return stack.Count != 0;
            }

            public void Reset()
            {
                stack = new Stack<FrameworkElement>();
                started = false;
            }

            public void SkipSubtree()
            {
                skip_subtree = true;
            }

            public FrameworkElement Current => stack.Peek();

            object System.Collections.IEnumerator.Current => Current;

            void IDisposable.Dispose()
            {
                stack.Clear();
                started = false;
            }
        }

        static void InvokeStyleResourceChanged(IntPtr fwe_ptr, string styleResource, IntPtr style_ptr)
        {
            try
            {
                FrameworkElement fe = (FrameworkElement)NativeDependencyObjectHelper.Lookup(fwe_ptr);
                Style style = (Style)NativeDependencyObjectHelper.Lookup(style_ptr);
                if (fe == null)
                    return;

                Type t = Type.GetType(styleResource.Substring(ResourceDictionary.INTERNAL_TYPE_KEY_MAGIC_COOKIE.Length));
                if (t == null)
                {
                    // Console.WriteLine ("InvokeStyleResourceChanged: this shouldn't happen...");
                    return;
                }

                // Console.WriteLine ("InvokeStyleResourceChanged (fe={0}/{1}, styleResource={2})", fe.GetType(), fe.Name, styleResource);

                DependencyObject templateOwner = fe.TemplateOwner;

                DeepVisualTreeEnumerator walker = new DeepVisualTreeEnumerator(fe);

                while (walker.MoveNext())
                {
                    FrameworkElement child = walker.Current;

                    if (child != fe)
                    {
                        if (child.Resources.Contains(styleResource) || child.Resources.Contains(t) || child.Resources.Contains(t.ToString()))
                        {
                            // Console.WriteLine ("child {0}/{1} contains a resource with the same key, skipping subtree", child.GetType(), child.Name);
                            walker.SkipSubtree();
                            continue;
                        }
                    }
                    if (child.TemplateOwner != templateOwner)
                    {
                        // Console.WriteLine ("skipping child {0}/{1} subtree since there are different template owners", child.GetType(), child.Name);
                        walker.SkipSubtree();
                        continue;
                    }


                    if (child.GetType() == t)
                    {
                        // Console.WriteLine ("child {0}/{1} gets the new style!", child.GetType(), child.Name);
                        child.SetImplicitStyles(ImplicitStyleMask.VisualTree, new Style[] { style, null, null });
                    }
                    // else {
                    // 	Console.WriteLine ("child {0}/{1} type doesn't match", child.GetType(), child.Name);
                    // }
                }
            }
            catch (Exception e)
            {
                // Console.WriteLine (e);
            }
        }

        static Size InvokeMeasureOverride(IntPtr fwe_ptr, Size availableSize, ref MoonError error)
        {
            FrameworkElement fe = null;
            try
            {
                fe = (FrameworkElement)NativeDependencyObjectHelper.Lookup(fwe_ptr);
                return fe.MeasureOverride(availableSize);
            }
            catch (Exception ex)
            {
                try
                {
                    if (fe != null)
                        LayoutInformation.SetLayoutExceptionElement(Deployment.Current.Dispatcher, fe);
                    error = new MoonError(ex);
                }
                catch (Exception ex2)
                {
                    try
                    {
                        Console.WriteLine("Leaked exception: {0}", ex2);
                    }
                    catch
                    {
                        // Ignore
                    }
                }
            }
            return new Size();
        }

        static Size InvokeArrangeOverride(IntPtr fwe_ptr, Size finalSize, ref MoonError error)
        {
            FrameworkElement fe = null;
            try
            {
                fe = (FrameworkElement)NativeDependencyObjectHelper.Lookup(fwe_ptr);
                return fe.ArrangeOverride(finalSize);
            }
            catch (Exception ex)
            {
                try
                {
                    if (fe != null)
                        LayoutInformation.SetLayoutExceptionElement(Deployment.Current.Dispatcher, fe);
                    error = new MoonError(ex);
                }
                catch (Exception ex2)
                {
                    try
                    {
                        Console.WriteLine("Leaked exception: {0}", ex2);
                    }
                    catch
                    {
                        // Ignore
                    }
                }
            }
            return new Size();
        }

        static IntPtr InvokeGetDefaultTemplate(IntPtr fwe_ptr)
        {
            IntPtr result = IntPtr.Zero;
            UIElement root = null;
            try
            {
                FrameworkElement element = (FrameworkElement)NativeDependencyObjectHelper.FromIntPtr(fwe_ptr);
                root = element.GetDefaultTemplate();
                if (root != null)
                    result = root.native;
            }
            catch (Exception ex)
            {
                try
                {
                    Console.WriteLine("Moonlight: Unhandled exception in FrameworkElement.InvokeGetDefaultTemplate: {0}", ex);
                }
                catch
                {
                    // Ignore
                }
            }
            return result;
        }

        internal virtual UIElement GetDefaultTemplate()
        {
            return null;
        }

        public virtual void OnApplyTemplate()
        {
            // according to doc this is not fully implemented since SL templates applies
            // to Control/ContentPresenter and is defined here for WPF compatibility
        }

        internal void RaiseBindingValidationError(ValidationErrorEventArgs e)
        {
            FrameworkElement element = this;
            e.OriginalSource = this;

            while (element != null)
            {
                element.BindingValidationError?.Invoke(element, e);
                element = VisualTreeHelper.GetParent(element) as FrameworkElement;
            }
        }

        public BindingExpression GetBindingExpression(DependencyProperty dp)
        {
            if (expressions == null)
                return null;
            Expression expression;
            if (expressions.TryGetValue(dp, out expression))
                return expression as BindingExpression;

            return null;
        }

        #region UIA Events and Methods

        // All events are 1-1 to each attached property defined in:
        // System.Windows.Automation.AutomationProperties

        internal event DependencyPropertyChangedEventHandler AcceleratorKeyChanged;
        internal event DependencyPropertyChangedEventHandler AccessKeyChanged;
        internal event DependencyPropertyChangedEventHandler AutomationIdChanged;
        internal event DependencyPropertyChangedEventHandler HelpTextChanged;
        internal event DependencyPropertyChangedEventHandler IsRequiredForFormChanged;
        internal event DependencyPropertyChangedEventHandler ItemStatusChanged;
        internal event DependencyPropertyChangedEventHandler ItemTypeChanged;
        internal event DependencyPropertyChangedEventHandler LabeledByChanged;
        internal event DependencyPropertyChangedEventHandler NameChanged;

        internal void RaiseAcceleratorKeyChanged(DependencyPropertyChangedEventArgs args)
        {
            AcceleratorKeyChanged?.Invoke(this, args);
        }

        internal void RaiseAccessKeyChanged(DependencyPropertyChangedEventArgs args)
        {
            AccessKeyChanged?.Invoke(this, args);
        }

        internal void RaiseAutomationIdChanged(DependencyPropertyChangedEventArgs args)
        {
            AutomationIdChanged?.Invoke(this, args);
        }

        internal void RaiseHelpTextChanged(DependencyPropertyChangedEventArgs args)
        {
            HelpTextChanged?.Invoke(this, args);
        }

        internal void RaiseIsRequiredForFormChanged(DependencyPropertyChangedEventArgs args)
        {
            IsRequiredForFormChanged?.Invoke(this, args);
        }

        internal void RaiseItemStatusChanged(DependencyPropertyChangedEventArgs args)
        {
            ItemStatusChanged?.Invoke(this, args);
        }

        internal void RaiseItemTypeChanged(DependencyPropertyChangedEventArgs args)
        {
            ItemTypeChanged?.Invoke(this, args);
        }

        internal void RaiseLabeledByChanged(DependencyPropertyChangedEventArgs args)
        {
            LabeledByChanged?.Invoke(this, args);
        }

        internal void RaiseNameChanged(DependencyPropertyChangedEventArgs args)
        {
            NameChanged?.Invoke(this, args);
        }

        #endregion
    }
}
