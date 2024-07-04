/*
 * InputMethod.cs.
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2008-2009 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

using System.Windows.Controls;

namespace System.Windows.Input
{
    public sealed partial class InputMethod : DependencyObject
    {
        static void CheckTarget(DependencyObject target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (target is not TextBox && target is not RichTextBox)
                throw new ArgumentException("target is not a TextBox or a RichTextBox", "target");
        }

        public static bool GetIsInputMethodEnabled(DependencyObject target)
        {
            CheckTarget(target);
            return (bool)target.GetValue(IsInputMethodEnabledProperty);
        }

        public static void SetIsInputMethodEnabled(DependencyObject target, bool value)
        {
            CheckTarget(target);
            target.SetValue(IsInputMethodEnabledProperty, value);
        }

        public static ImeConversionModeValues GetPreferredImeConversionMode(DependencyObject target)
        {
            CheckTarget(target);
            return (ImeConversionModeValues)target.GetValue(PreferredImeConversionModeProperty);
        }

        public static void SetPreferredImeConversionMode(DependencyObject target, ImeConversionModeValues value)
        {
            CheckTarget(target);
            target.SetValue(PreferredImeConversionModeProperty, value);
        }

        public static InputMethodState GetPreferredImeState(DependencyObject target)
        {
            CheckTarget(target);
            return (InputMethodState)target.GetValue(PreferredImeStateProperty);
        }

        public static void SetPreferredImeState(DependencyObject target, InputMethodState value)
        {
            CheckTarget(target);
            target.SetValue(PreferredImeStateProperty, value);
        }
    }
}
