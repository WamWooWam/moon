//
// System.Windows.UIElement.cs
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
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Media;
using System.Windows.Input;
using Mono;

namespace System.Windows
{
    public abstract partial class UIElement : DependencyObject
    {

        static UIElement()
        {
            VisibilityProperty.AddPropertyChangeCallback(VisibilityPropertyChanged);
        }

        static void VisibilityPropertyChanged(DependencyObject sender,
                                               DependencyPropertyChangedEventArgs args)
        {
            ((UIElement)sender).RaiseUIAVisibilityChanged(args);
        }

        internal event EventHandler VisualParentChanged;

        public Transform RenderTransform
        {
            get
            {
                Transform t = (Transform)GetValue(RenderTransformProperty);
                return t == null ? new MatrixTransform() : t;
            }
            set
            {
                if (value == null)
                    ClearValue(RenderTransformProperty);
                else
                    SetValue(RenderTransformProperty, value);
            }
        }

        public bool CaptureMouse()
        {
            return NativeMethods.uielement_capture_mouse(native);
        }

        public void ReleaseMouseCapture()
        {
            NativeMethods.uielement_release_mouse_capture(native);
        }

        public void Arrange(Rect finalRect)
        {
            if (finalRect.IsEmpty)
                throw new InvalidOperationException("Empty Rect");

            if (Double.IsInfinity(finalRect.Width) || Double.IsInfinity(finalRect.Height) || Double.IsInfinity(finalRect.X) || Double.IsInfinity(finalRect.Y))
                throw new InvalidOperationException("Infinite Rect");
            if (Double.IsNaN(finalRect.Width) || Double.IsNaN(finalRect.Height) || Double.IsNaN(finalRect.X) || Double.IsNaN(finalRect.Y))
                throw new InvalidOperationException("NaN Rect");

            NativeMethods.uielement_arrange(native, finalRect);
        }

        public void InvalidateArrange()
        {
            NativeMethods.uielement_invalidate_arrange(native);
        }

        public void Measure(Size availableSize)
        {
            NativeMethods.uielement_measure(native, availableSize);
        }

        internal DependencyObject SubtreeObject
        {
            get; private set;
        }

        internal UIElement VisualParent
        {
            get; set;
        }

        public void InvalidateMeasure()
        {
            NativeMethods.uielement_invalidate_measure(native);
        }

        public void UpdateLayout()
        {
            NativeMethods.uielement_update_layout(native);
        }

        public GeneralTransform TransformToVisual(UIElement visual)
        {
            GeneralTransform result;

            IntPtr t = NativeMethods.uielement_get_transform_to_uielement(native, visual == null ? IntPtr.Zero : visual.native);

            result = (GeneralTransform)NativeDependencyObjectHelper.Lookup(t);

            NativeMethods.event_object_unref(t);

            return result;
        }

        protected virtual AutomationPeer OnCreateAutomationPeer()
        {
            // there's no automation object associated with UIElement so null is returned
            // it could have been abtract but that that would have forced everyone (without 
            // automation support) to override this default
            return null;
        }

        internal AutomationPeer AutomationPeer
        {
            get; set;
        }

        // needed by FrameworkElementAutomationPeer
        internal AutomationPeer CreateAutomationPeer()
        {
            return OnCreateAutomationPeer();
        }

        public Size DesiredSize => NativeMethods.uielement_get_desired_size(native);

        public Size RenderSize => NativeMethods.uielement_get_render_size(native);

        private void ValidateHandlerArguments(RoutedEvent routedEvent, Delegate handler)
        {
            if (routedEvent == null)
                throw new ArgumentNullException("routedEvent");

            if (handler == null)
                throw new ArgumentNullException("handler");

            if (IsManipulation(routedEvent))
            {
                EnsureDesignMode();
            }
            else if (routedEvent == KeyDownEvent || routedEvent == KeyUpEvent)
            {
                if (handler is not KeyEventHandler)
                    throw new ArgumentException("handler");
            }
            else if (routedEvent == MouseLeftButtonDownEvent || routedEvent == MouseLeftButtonUpEvent)
            {
                if (handler is not MouseButtonEventHandler)
                    throw new ArgumentException("handler");
            }
            else if (routedEvent == MouseWheelEvent)
            {
                if (handler is not MouseWheelEventHandler)
                    throw new ArgumentException("handler");
            }
            else if (routedEvent == TextInputStartEvent || routedEvent == TextInputUpdateEvent || routedEvent == TextInputEvent)
            {
                if (handler is not TextCompositionEventHandler)
                    throw new ArgumentException("handler");
            }
            else
            {
                throw new ArgumentException("routedEvent");
            }
        }

        public void AddHandler(RoutedEvent routedEvent, Delegate handler, bool handledEventsToo)
        {
            ValidateHandlerArguments(routedEvent, handler);
            RegisterEvent(routedEvent.EventId, handler, Events.CreateDispatcherFromEventId(routedEvent.EventId, handler), handledEventsToo);
        }

        public void RemoveHandler(RoutedEvent routedEvent, Delegate handler)
        {
            ValidateHandlerArguments(routedEvent, handler);
            UnregisterEvent(routedEvent.EventId, handler);
        }

        // Manipulation* events are not supported, outside of design-mode, in SL4
        static bool IsManipulation(RoutedEvent routedEvent)
        {
            switch (routedEvent.EventId)
            {
                case EventIds.UIElement_ManipulationCompletedEvent:
                case EventIds.UIElement_ManipulationDeltaEvent:
                case EventIds.UIElement_ManipulationStartedEvent:
                    return true;
                default:
                    return false;
            }
        }

        static void EnsureDesignMode()
        {
            // NOTE:
            // If you ended up here from drt 539 you should know
            // it doesn't pass on Silverlight but MobileStubsTests
            // does so tread lightly

            if (//Int32.Parse (Deployment.Current.RuntimeVersion.Split('.')[0]) < 4 &&
                !DesignerProperties.GetIsInDesignMode(Application.Current.RootVisual))
                throw new NotImplementedException();
        }

        internal override void AddStrongRef(IntPtr id, object value)
        {
            if (id == (IntPtr)WeakRefs.UIElement_SubtreeObject)
            {
                SubtreeObject = (DependencyObject)value;
            }
            else if (id == (IntPtr)WeakRefs.UIElement_VisualParent)
            {
                VisualParent = (UIElement)value;
                VisualParentChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                base.AddStrongRef(id, value);
            }
        }

        internal override void ClearStrongRef(IntPtr id, object value)
        {
            if (id == (IntPtr)WeakRefs.UIElement_SubtreeObject)
            {
                SubtreeObject = null;
            }
            else if (id == (IntPtr)WeakRefs.UIElement_VisualParent)
            {
                VisualParent = null;
                VisualParentChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                base.ClearStrongRef(id, value);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler<ManipulationCompletedEventArgs> ManipulationCompleted
        {
            add
            {
                EnsureDesignMode();
                RegisterEvent(EventIds.UIElement_ManipulationCompletedEvent, value, Events.CreateManipulationCompletedEventArgsEventHandlerDispatcher(value));
            }
            remove
            {
                EnsureDesignMode();
                UnregisterEvent(EventIds.UIElement_ManipulationCompletedEvent, value);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly RoutedEvent ManipulationCompletedEvent = new RoutedEvent(EventIds.UIElement_ManipulationCompletedEvent);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler<ManipulationDeltaEventArgs> ManipulationDelta
        {
            add
            {
                EnsureDesignMode();
                RegisterEvent(EventIds.UIElement_ManipulationDeltaEvent, value, Events.CreateManipulationDeltaEventArgsEventHandlerDispatcher(value));
            }
            remove
            {
                EnsureDesignMode();
                UnregisterEvent(EventIds.UIElement_ManipulationDeltaEvent, value);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly RoutedEvent ManipulationDeltaEvent = new RoutedEvent(EventIds.UIElement_ManipulationDeltaEvent);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler<ManipulationStartedEventArgs> ManipulationStarted
        {
            add
            {
                EnsureDesignMode();
                RegisterEvent(EventIds.UIElement_ManipulationStartedEvent, value, Events.CreateManipulationStartedEventArgsEventHandlerDispatcher(value));
            }
            remove
            {
                EnsureDesignMode();
                UnregisterEvent(EventIds.UIElement_ManipulationStartedEvent, value);
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly RoutedEvent ManipulationStartedEvent = new RoutedEvent(EventIds.UIElement_ManipulationStartedEvent);

        #region UIA Events

        internal event DependencyPropertyChangedEventHandler UIAVisibilityChanged;

        internal void RaiseUIAVisibilityChanged(DependencyPropertyChangedEventArgs args)
        {
            UIAVisibilityChanged?.Invoke(this, args);
        }

        #endregion
    }
}
