// (c) Copyright Microsoft Corporation. 
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved. 


using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Media.Animation;
using System.Windows.Automation.Peers;
using System.Windows;

namespace System.Windows.Controls.Primitives
{
    /// <summary> 
    /// Base class for controls that can switch states, such as CheckBox.
    /// </summary> 
    [TemplateVisualState(Name = StateNormal, GroupName = GroupCommon)]
    [TemplateVisualState(Name = StateMouseOver, GroupName = GroupCommon)]
    [TemplateVisualState(Name = StatePressed, GroupName = GroupCommon)]
    [TemplateVisualState(Name = StateDisabled, GroupName = GroupCommon)]
    [TemplateVisualState(Name = StateUnfocused, GroupName = GroupFocus)]
    [TemplateVisualState(Name = StateFocused, GroupName = GroupFocus)]
    [TemplateVisualState(Name = StateChecked, GroupName = GroupCheck)]
    [TemplateVisualState(Name = StateUnchecked, GroupName = GroupCheck)]
    [TemplateVisualState(Name = StateIndeterminate, GroupName = GroupCheck)]
    public partial class ToggleButton : ButtonBase
    {
        #region IsChecked 
        /// <summary>
        /// Gets or sets whether the ToggleButton is checked.
        /// </summary> 
        [TypeConverter(typeof(NullableBoolConverter))]
        public bool? IsChecked
        {
            get => GetValue(IsCheckedProperty) as bool?;
            set => SetValue(IsCheckedProperty, value);
        }

        /// <summary>
        /// Identifies the IsChecked dependency property. 
        /// </summary> 
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.RegisterCore(
                "IsChecked",
                typeof(bool?),
                typeof(ToggleButton),
                new PropertyMetadata(false, OnIsCheckedPropertyChanged));

        /// <summary> 
        /// IsCheckedProperty property changed handler. 
        /// </summary>
        /// <param name="d">ToggleButton that changed its IsChecked.</param> 
        /// <param name="e">DependencyPropertyChangedEventArgs.</param>
        private static void OnIsCheckedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToggleButton source = d as ToggleButton;
            Debug.Assert(source != null,
                "The source is not an instance of ToggleButton!");

            Debug.Assert(typeof(bool?).IsInstanceOfType(e.NewValue) || (e.NewValue == null),
                "The value is not an instance of bool?!");
            bool? value = (bool?)e.NewValue;

            // Raise the appropriate changed event 
            RoutedEventArgs args = new RoutedEventArgs();
            args.OriginalSource = source; // like beta1 (and unit test confirmed)
            if (value == true)
            {
                source.OnChecked(args);
            }
            else if (value == false)
            {
                source.OnUnchecked(args);
            }
            else
            {
                source.OnIndeterminate(args);
            }

            // Raising UIA Automation Event
            ToggleButtonAutomationPeer peer = source.AutomationPeer as ToggleButtonAutomationPeer;
            if (peer != null)
                peer.RaiseToggleStateChanged();
        }
        #endregion IsChecked

        #region IsThreeState 
        /// <summary>
        /// Determines whether the control supports two or three states.
        /// </summary> 
        public bool IsThreeState
        {
            get => (bool)GetValue(IsThreeStateProperty);
            set => SetValue(IsThreeStateProperty, value);
        }

        /// <summary>
        /// Identifies the IsThreeState dependency property.
        /// </summary> 
        public static readonly DependencyProperty IsThreeStateProperty =
            DependencyProperty.RegisterCore(
                "IsThreeState",
                typeof(bool),
                typeof(ToggleButton),
                null);
        #endregion IsThreeState

        #region Template Parts 
        /// <summary> 
        /// Checked state of the ToggleButton.
        /// </summary> 
        internal const string StateChecked = "Checked";

        /// <summary> 
        /// Unchecked state of the ToggleButton.
        /// </summary>
        internal const string StateUnchecked = "Unchecked";

        /// <summary>
        /// Indeterminate state of the ToggleButton. 
        /// </summary>
        internal const string StateIndeterminate = "Indeterminate";

        /// <summary>
        /// Check state group of the ToggleButton.
        /// </summary> 
        internal const string GroupCheck = "CheckStates";
        #endregion Template Parts

        /// <summary>
        /// Occurs when a ToggleButton is checked.
        /// </summary> 
        public event RoutedEventHandler Checked;

        /// <summary> 
        /// Occurs when the state of a ToggleButton is neither on nor off. 
        /// </summary>
        public event RoutedEventHandler Indeterminate;

        /// <summary>
        /// Occurs when a ToggleButton is unchecked. 
        /// </summary>
        public event RoutedEventHandler Unchecked;

        /// <summary> 
        /// Initializes a new instance of the ToggleButton class.
        /// </summary> 
        public ToggleButton()
        {
            DefaultStyleKey = typeof(ToggleButton);
        }

        /// <summary> 
        /// Apply a template to the ToggleButton. 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Sync the logical and visual states of the control
            UpdateVisualState(false);
        }

        /// <summary>
        /// Change to the correct visual state for the ToggleButton. 
        /// </summary>
        /// <param name="useTransitions">
        /// true to use transitions when updating the visual state, false to 
        /// snap directly to the new visual state.
        /// </param>
        internal override void ChangeVisualState(bool useTransitions)
        {
            // Cache dependency properties we'll check more than once
            bool? isChecked = IsChecked;
            bool isEnabled = IsEnabled;

            // Update the Interaction state group 
            if (!isEnabled)
            {
                GoToState(useTransitions, StateDisabled);
            }
            else if (IsPressed)
            {
                GoToState(useTransitions, StatePressed);
            }
            else if (IsMouseOver)
            {
                GoToState(useTransitions, StateMouseOver);
            }
            else
            {
                GoToState(useTransitions, StateNormal);
            }

            // Update the validation state group
            if (Validation.GetErrors(this).Count > 0)
            {
                GoToState(useTransitions, IsFocused ? "InvalidFocused" : "InvalidUnfocused");
            }
            else
            {
                GoToState(useTransitions, "Valid");
            }

            // Update the Check state group 
            if (isChecked == true)
            {
                GoToState(useTransitions, StateChecked);
            }
            else if (isChecked == false)
            {
                GoToState(useTransitions, StateUnchecked);
            }
            else
            {
                // isChecked is null
                if (!GoToState(useTransitions, StateIndeterminate))
                {
                    GoToState(useTransitions, StateUnchecked);
                }
            }

            if (IsFocused && isEnabled)
            {
                GoToState(useTransitions, StateFocused);
            }
            else
            {
                GoToState(useTransitions, StateUnfocused);
            }
        }

        /// <summary>
        /// Called when a ToggleButton raises a Checked event.
        /// </summary> 
        /// <param name="e">The event data for the Checked event.</param> 
        internal virtual void OnChecked(RoutedEventArgs e)
        {
            UpdateVisualState();

            Checked?.Invoke(this, e);
        }

        /// <summary>
        /// Called when a ToggleButton raises an Indeterminate event.
        /// </summary> 
        /// <param name="e">The event data for the Indeterminate event.</param>
        internal virtual void OnIndeterminate(RoutedEventArgs e)
        {
            UpdateVisualState();

            Indeterminate?.Invoke(this, e);
        }

        /// <summary> 
        /// Called when a ToggleButton raises an Unchecked event.
        /// </summary> 
        /// <param name="e">The event data for the Unchecked event.</param>
        internal virtual void OnUnchecked(RoutedEventArgs e)
        {
            UpdateVisualState();

            Unchecked?.Invoke(this, e);
        }

        /// <summary>
        /// Move the button to its next IsChecked value.
        /// </summary> 
        protected internal virtual void OnToggle()
        {
            bool? isChecked = IsChecked;
            if (isChecked == true)
            {
                IsChecked = IsThreeState ? null : ((bool?)false);
            }
            else
            {
                IsChecked = (bool?)isChecked.HasValue;
            }
        }

        /// <summary>
        /// Called when the Content property changes. 
        /// </summary>
        /// <param name="oldContent">
        /// The old value of the Content property. 
        /// </param> 
        /// <param name="newContent">
        /// The new value of the Content property. 
        /// </param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            UpdateVisualState();
        }

        /// <summary>
        /// Raises the Click routed event. 
        /// </summary>
        protected override void OnClick()
        {
            OnToggle();
            base.OnClick();
        }

        // Moonlight: This method wasn't included in the control drop. 
        public override string ToString()
        {
            string content = Content == null ? string.Empty : Content.ToString();
            string ischecked = IsChecked.HasValue ? IsChecked.Value.ToString() : "null";
            return string.Format("System.Windows.Controls.Primitives.ToggleButton Content:{0} IsChecked:{1}", content, ischecked);
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new ToggleButtonAutomationPeer(this);
        }
    }
}
