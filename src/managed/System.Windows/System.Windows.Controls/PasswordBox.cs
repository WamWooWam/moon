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
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Automation.Peers;
using System.Collections.Specialized;
using Mono;

namespace System.Windows.Controls
{
    [TemplatePartAttribute(Name = "ContentElement", Type = typeof(FrameworkElement))]
    [TemplateVisualStateAttribute(Name = "InvalidFocused", GroupName = "ValidationStates")]
    [TemplateVisualStateAttribute(Name = "Normal", GroupName = "CommonStates")]
    [TemplateVisualStateAttribute(Name = "Disabled", GroupName = "CommonStates")]
    [TemplateVisualStateAttribute(Name = "MouseOver", GroupName = "CommonStates")]
    [TemplateVisualStateAttribute(Name = "Unfocused", GroupName = "FocusStates")]
    [TemplateVisualStateAttribute(Name = "Focused", GroupName = "FocusStates")]
    [TemplateVisualStateAttribute(Name = "Valid", GroupName = "ValidationStates")]
    [TemplateVisualStateAttribute(Name = "InvalidUnfocused", GroupName = "ValidationStates")]
    public sealed partial class PasswordBox : Control
    {
        object contentElement;

        bool IsFocused
        {
            get; set;
        }

        bool IsMouseOver
        {
            get; set;
        }

        private new void Initialize()
        {
            // FIXME: Should use Events.AddOnEventHandler or something similar.
            CursorPositionChanged += OnCursorPositionChanged;
        }

        internal override void InvokeIsEnabledPropertyChanged()
        {
            base.InvokeIsEnabledPropertyChanged();
            ChangeVisualState(false);
        }

        internal override void InvokeOnApplyTemplate()
        {
            base.InvokeOnApplyTemplate();
            ChangeVisualState(false);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Chain up to our parent first, so that TabNavigation
            // works as well as allowing developers to filter our
            // input.
            base.OnKeyDown(e);

            if (!e.Handled)
                NativeMethods.text_box_base_on_key_down(native, e.NativeHandle);
        }

        internal override void PostOnKeyDown(KeyEventArgs k)
        {
            base.PostOnKeyDown(k);

            if (!k.Handled)
                NativeMethods.text_box_base_post_on_key_down(native, k.NativeHandle);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (!e.Handled)
                NativeMethods.text_box_base_on_key_up(native, e.NativeHandle);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (!e.Handled)
                NativeMethods.text_box_base_on_mouse_left_button_down(native, e.NativeHandle);

            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (!e.Handled)
                NativeMethods.text_box_base_on_mouse_left_button_up(native, e.NativeHandle);

            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            NativeMethods.text_box_base_on_mouse_move(native, e.NativeHandle);
            base.OnMouseMove(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            IsMouseOver = true;
            base.OnMouseEnter(e);
            ChangeVisualState();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            IsMouseOver = false;
            base.OnMouseLeave(e);
            ChangeVisualState();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            ChangeVisualState();
            NativeMethods.text_box_base_on_got_focus(native, e.NativeHandle);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            IsFocused = false;
            base.OnLostFocus(e);
            NativeMethods.text_box_base_on_lost_focus(native, e.NativeHandle);
            ChangeVisualState();
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            base.OnTextInput(e);
        }

        protected override void OnTextInputStart(TextCompositionEventArgs e)
        {
            base.OnTextInputStart(e);
        }

        protected override void OnTextInputUpdate(TextCompositionEventArgs e)
        {
            base.OnTextInputUpdate(e);
        }

        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            base.OnLostMouseCapture(e);
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new PasswordBoxAutomationPeer(this);
        }

        public void SelectAll()
        {
            NativeMethods.text_box_base_select_all(native);
        }

        void OnCursorPositionChanged(object sender, CursorPositionChangedEventArgs args)
        {
            if (contentElement == null)
                contentElement = GetTemplateChild("ContentElement");

            if (contentElement != null && contentElement is ScrollViewer)
            {
                ScrollViewer scrollview = contentElement as ScrollViewer;
                double offset = scrollview.HorizontalOffset;

                // Note: for horizontal scrollage, we offset by 1.0 pixel for the width of the cursor ibeam

                if (args.CursorX < offset)
                {
                    // need to scroll to the left a bit
                    scrollview.ScrollToHorizontalOffset(Math.Max(args.CursorX - 1.0, 0.0));
                }
                else if (args.CursorX > offset + scrollview.ViewportWidth)
                {
                    // need to scroll to the right
                    offset = (args.CursorX + 1.0) - scrollview.ViewportWidth;
                    scrollview.ScrollToHorizontalOffset(offset);
                }

                offset = scrollview.VerticalOffset;
                if (args.CursorY < offset)
                {
                    // need to scroll up a bit
                    scrollview.ScrollToVerticalOffset(args.CursorY);
                }
                else if (args.CursorY + args.CursorHeight > offset + scrollview.ViewportHeight)
                {
                    // need to scroll down a bit
                    offset = (args.CursorY + args.CursorHeight) - Math.Max(args.CursorHeight, scrollview.ViewportHeight);
                    scrollview.ScrollToVerticalOffset(offset);
                }
            }
        }

        event CursorPositionChangedEventHandler CursorPositionChanged
        {
            add => RegisterEvent(EventIds.TextBoxBase_CursorPositionChangedEvent, value,
                           Events.CreateCursorPositionChangedEventHandlerDispatcher(value));
            remove => UnregisterEvent(EventIds.TextBoxBase_CursorPositionChangedEvent, value);
        }

        void ChangeVisualState()
        {
            ChangeVisualState(true);
        }

        void ChangeVisualState(bool useTransitions)
        {
            if (!IsEnabled)
            {
                VisualStateManager.GoToState(this, "Disabled", useTransitions);
            }
            else if (IsMouseOver)
            {
                VisualStateManager.GoToState(this, "MouseOver", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", useTransitions);
            }

            if (IsFocused)
            {
                VisualStateManager.GoToState(this, "Focused", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Unfocused", useTransitions);
            }
        }
    }
}
