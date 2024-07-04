// (c) Copyright Microsoft Corporation. 
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved. 


using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Automation.Peers;

namespace System.Windows.Controls
{
    /// <summary> 
    /// Represents a control that a user can select and clear. 
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
    [TemplateVisualState(Name = "InvalidFocused", GroupName = "ValidationStates")]
    [TemplateVisualState(Name = "InvalidUnfocused", GroupName = "ValidationStates")]
    [TemplateVisualState(Name = "Valid", GroupName = "ValidationStates")]
    public partial class CheckBox : ToggleButton
    {
        /// <summary>
        /// Initializes a new instance of the CheckBox class. 
        /// </summary>
        public CheckBox()
        {
            DefaultStyleKey = typeof(CheckBox);
        }

        /*
                /// <summary>
                /// Handles the KeyDown event for CheckBox.
                /// </summary> 
                /// <param name="key">
                /// The keyboard key associated with the event.
                /// </param> 
                /// <returns>True if the event was handled, false otherwise.</returns> 
                private void OnKeyDown(object sender, KeyEventArgs e)
                { 
                    if (!IsThreeState && IsEnabled)
                    {
                        if (e.Key == Key.Add) 
                        {
                            e.Handled = true;
                            IsPressed = false; 
                            IsChecked = true; 
                        }
                        else if (e.Key == Key.Subtract) 
                        {
                            e.Handled = true;
                            IsPressed = false; 
                            IsChecked = false;
                        }
                    } 
                } 
        */

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new CheckBoxAutomationPeer(this);
        }
    }
}
