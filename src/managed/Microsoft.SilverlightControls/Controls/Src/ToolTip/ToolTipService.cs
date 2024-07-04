// Copyright © Microsoft Corporation. 
// This source is subject to the Microsoft Source License for Silverlight Controls (March 2008 Release).
// Please see http://go.microsoft.com/fwlink/?LinkID=111693 for details.
// All other rights reserved. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;

namespace System.Windows.Controls
{
    /// <summary> 
    ///     Service class that provides the system implementation for displaying ToolTips.
    /// </summary>
    public static class ToolTipService
    {
        #region Constants

        private const int TOOLTIPSERVICE_betweenShowDelay = 100; // 100 milliseconds
        private const int TOOLTIPSERVICE_initialShowDelay = 400; // 400 milliseconds
        private const int TOOLTIPSERVICE_showDuration = 5000;    // 5000 milliseconds 

        #endregion Constants

        #region Data 

        private static ToolTip _currentToolTip;
        private static DispatcherTimer _closeTimer;
        private static object _lastEnterSource;
        private static DateTime _lastToolTipOpenedTime = DateTime.MinValue;
        private static DispatcherTimer _openTimer;
        private static UIElement _owner;
        private static FrameworkElement _rootVisual;

        #endregion Data 

        static readonly DependencyProperty AssignedToolTipProperty =
            DependencyProperty.Register("AssignedToolTip", typeof(ToolTip), typeof(UIElement), null);

        #region Placement Property
        public static readonly DependencyProperty PlacementProperty =
                        DependencyProperty.RegisterAttached(
                        "Placement",            // Name 
                        typeof(PlacementMode),  // Type
                        typeof(ToolTipService), // Owner 
                        new PropertyMetadata(PlacementMode.Mouse, OnPlacementPropertyChanged));

        public static PlacementMode GetPlacement(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return (PlacementMode)element.GetValue(PlacementProperty);
        }

        public static void SetPlacement(DependencyObject element, PlacementMode value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(PlacementProperty, value);
        }

        private static void OnPlacementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion Placement Property

        #region Placement Target Property
        public static readonly DependencyProperty PlacementTargetProperty =
                        DependencyProperty.RegisterAttached(
                        "PlacementTarget",      // Name 
                        typeof(UIElement),      // Type
                        typeof(ToolTipService), // Owner 
                        new PropertyMetadata(OnPlacementTargetPropertyChanged));

        public static UIElement GetPlacementTarget(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return (UIElement)element.GetValue(PlacementTargetProperty);
        }

        public static void SetPlacementTarget(DependencyObject element, UIElement value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(PlacementTargetProperty, value);
        }

        private static void OnPlacementTargetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion Placement Target Property

        #region ToolTip Property
        /// <summary>
        ///     The DependencyProperty for the ToolTip property. 
        /// </summary>
        public static readonly DependencyProperty ToolTipProperty =
                        DependencyProperty.RegisterAttached(
                        "ToolTip",              // Name 
                        typeof(object),         // Type
                        typeof(ToolTipService), // Owner 
                        new PropertyMetadata(OnToolTipPropertyChanged));

        /// <summary> 
        ///     Gets the value of the ToolTip property on the specified object.
        /// </summary>
        /// <param name="element">The object on which to query the ToolTip property.</param> 
        /// <returns>The value of the ToolTip property.</returns> 
        public static object GetToolTip(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return element.GetValue(ToolTipProperty);
        }

        /// <summary>
        ///     Sets the ToolTip property on the specified object. 
        /// </summary>
        /// <param name="element">The object on which to set the ToolTip property.</param>
        /// <param name="value"> 
        ///     The value of the ToolTip property. If the value is of type ToolTip, then
        ///     that is the ToolTip that will be used (without any modification). If the value
        ///     is of any other type, then that value will be used as the content for a ToolTip 
        ///     provided by this service, and the other attached properties of this service 
        ///     will be used to configure the ToolTip.
        /// </param> 
        public static void SetToolTip(DependencyObject element, object value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(ToolTipProperty, value);
        }

        private static void OnToolTipPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement owner = (UIElement)d;

            object toolTip = e.NewValue;
            if (e.OldValue != null)
            {
                UnregisterToolTip(owner);
            }

            if (toolTip == null)
            {
                return;
            }

            RegisterToolTip(owner, toolTip);

            SetRootVisual();
        }

        #endregion ToolTip Property

        #region Internal Properties 

        /// <summary>
        /// Place the ToolTip relative to this point 
        /// </summary>
        internal static Point MousePosition { get; set; }

        /// <summary>
        /// VisualRoot - the main page
        /// </summary> 
        internal static FrameworkElement RootVisual
        {
            get
            {
                SetRootVisual();
                return _rootVisual;
            }
        }

        /// <summary> 
        /// Access the toolTip which is currenly open by mouse movements
        /// </summary> 
        internal static ToolTip CurrentToolTip => _currentToolTip;

        #endregion Internal Properties 

        #region Internal Methods

        internal static void OnOwnerMouseEnterInternal(object sender, object source)
        {
            if ((_lastEnterSource != null) && ReferenceEquals(_lastEnterSource, source))
            {
                // ToolTipService had processed this event once before, when it fired on the child
                // skip it now 
                return;
            }

            UIElement senderElement = (UIElement)sender;
            if (_currentToolTip != null)
            {
                if (senderElement.GetValue(AssignedToolTipProperty) != _currentToolTip)
                {
                    // first close the previous ToolTip if entering nested elements with tooltips 
                    CloseAutomaticToolTip(null, EventArgs.Empty);
                }
                else
                {
                    // reentering the same element
                    return;
                }
            }

            _owner = senderElement;
            _lastEnterSource = source;

            Debug.Assert(_currentToolTip == null);

            SetRootVisual();

            TimeSpan sinceLastOpen = DateTime.Now - _lastToolTipOpenedTime;
            if (TimeSpan.Compare(sinceLastOpen, new TimeSpan(0, 0, 0, 0, TOOLTIPSERVICE_betweenShowDelay)) <= 0)
            {
                // open the ToolTip immediately 
                OpenAutomaticToolTip(null, EventArgs.Empty);
            }
            else
            {
                // open the ToolTip after the InitialShowDelay interval expires
                if (_openTimer == null)
                {
                    _openTimer = new DispatcherTimer();
                    _openTimer.Tick += new EventHandler(OpenAutomaticToolTip);
                }
                _openTimer.Interval = new TimeSpan(0, 0, 0, 0, TOOLTIPSERVICE_initialShowDelay);
                _openTimer.Start();
            }
        }

        internal static void OnOwnerMouseLeave(object sender, MouseEventArgs e)
        {
            if (_currentToolTip == null)
            {
                // ToolTip had not been opened yet 
                _openTimer.Stop();
                _owner = null;
                _lastEnterSource = null;
                return;
            }
            CloseAutomaticToolTip(null, EventArgs.Empty);
        }

        #endregion Internal Methods 

        #region Private Methods

        // This method should be executed on the UI thread 
        private static void CloseAutomaticToolTip(object sender, EventArgs e)
        {
            _closeTimer.Stop();

            _currentToolTip.PlacementOverride = null;
            _currentToolTip.PlacementTargetOverride = null;
            _currentToolTip.IsOpen = false;
            _currentToolTip = null;
            _owner = null;
            _lastEnterSource = null;

            // set last opened timestamp only if the ToolTip is opened by a mouse movement 
            _lastToolTipOpenedTime = DateTime.Now;

        }

        private static ToolTip ConvertToToolTip(object o)
        {
            ToolTip toolTip = o as ToolTip;
            if (toolTip == null && o is FrameworkElement)
                toolTip = ((FrameworkElement)o).Parent as ToolTip;

            if (toolTip == null)
                toolTip = new ToolTip { Content = o };

            return toolTip;
        }

        private static bool IsSpecialKey(Key key)
        {
            Key[] specialKeys =
            {
                Key.Alt,
                Key.Back,
                Key.Delete,
                Key.Down,
                Key.End,
                Key.Home,
                Key.Insert,
                Key.Left,
                Key.PageDown,
                Key.PageUp,
                Key.Right,
                Key.Space,
                Key.Up
            };

            for (int i = 0; i < specialKeys.Length; i++)
            {
                if (key == specialKeys[i])
                {
                    return true;
                }
            }
            return false;
        }

        private static void OnOwnerKeyDown(object sender, KeyEventArgs e)
        {
            if ((_lastEnterSource != null) && ReferenceEquals(_lastEnterSource, e.OriginalSource))
            {
                return;
            }

            if (_owner != sender)
            {
                return;
            }

            // close the opened ToolTip or cancel mouse hover 
            if (_currentToolTip == null)
            {
                _openTimer.Stop();
                _owner = null;
                _lastEnterSource = null;
                return;
            }

            if (IsSpecialKey(e.Key))
            {
                return;
            }

            CloseAutomaticToolTip(null, EventArgs.Empty);
        }

        private static void OnOwnerMouseEnter(object sender, MouseEventArgs e)
        {
            // cache mouse position relative to the plug-in
            MousePosition = e.GetPosition(null);

            OnOwnerMouseEnterInternal(sender, e.OriginalSource);
        }

        private static void OnOwnerMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((_lastEnterSource != null) && ReferenceEquals(_lastEnterSource, e.OriginalSource))
            {
                return;
            }

            if (_owner != sender)
            {
                return;
            }

            // close the opened ToolTip or cancel mouse hover
            if (_currentToolTip == null)
            {
                _openTimer.Stop();
                _owner = null;
                _lastEnterSource = null;
                return;
            }

            CloseAutomaticToolTip(null, EventArgs.Empty);
        }

        private static void OnRootMouseMove(object sender, MouseEventArgs e)
        {
            MousePosition = e.GetPosition(null);
        }

        private static void OpenAutomaticToolTip(object sender, EventArgs e)
        {
            _openTimer.Stop();

            Debug.Assert(_owner != null, "ToolTip owner was not set prior to starting the open timer");

            _currentToolTip = (ToolTip)_owner.GetValue(AssignedToolTipProperty);

            if (_currentToolTip != null)
            {
                _currentToolTip.PlacementOverride = GetPlacement(_owner);
                _currentToolTip.PlacementTargetOverride = GetPlacementTarget(_owner) ?? _owner;
                _currentToolTip.IsOpen = true;

                // start the timer which closes the ToolTip
                if (_closeTimer == null)
                {
                    _closeTimer = new DispatcherTimer();
                    _closeTimer.Tick += new EventHandler(CloseAutomaticToolTip);
                }
                _closeTimer.Interval = new TimeSpan(0, 0, 0, 0, TOOLTIPSERVICE_showDuration);
                _closeTimer.Start();
            }
        }
        /*
        private static void PositiveValueValidation(DependencyObject d, DependencyPropertyChangedEventArgs e) 
        { 
            if ((int)e.NewValue <= 0)
            { 
                throw new ArgumentException(Resource.ToolTipService_SetTimeoutProperty_InvalidValue, "e");
            }
        } 
		 */
        private static void RegisterToolTip(UIElement owner, object toolTip)
        {
            Debug.Assert(owner != null, "ToolTip must have an owner");
            Debug.Assert(toolTip != null, "ToolTip can not be null");

            owner.MouseEnter += new MouseEventHandler(OnOwnerMouseEnter);
            owner.MouseLeave += new MouseEventHandler(OnOwnerMouseLeave);
            owner.MouseLeftButtonDown += new MouseButtonEventHandler(OnOwnerMouseLeftButtonDown);
            owner.KeyDown += new KeyEventHandler(OnOwnerKeyDown);
            var converted = ConvertToToolTip(toolTip);
            owner.SetValue(AssignedToolTipProperty, converted);
            converted.TooltipParent = owner as FrameworkElement;
        }

        private static void SetRootVisual()
        {
            if ((_rootVisual == null) && (Application.Current != null))
            {
                _rootVisual = Application.Current.RootVisual as FrameworkElement;
                if (_rootVisual != null)
                {
                    // keep caching mouse position because we can't query it from Silverlight 
                    _rootVisual.MouseMove += new MouseEventHandler(OnRootMouseMove);
                }
            }
        }

        private static void UnregisterToolTip(UIElement owner)
        {
            Debug.Assert(owner != null, "owner element is required");

            if (owner.GetValue(AssignedToolTipProperty) == null)
            {
                return;
            }

            owner.MouseEnter -= new MouseEventHandler(OnOwnerMouseEnter);
            owner.MouseLeave -= new MouseEventHandler(OnOwnerMouseLeave);
            owner.MouseLeftButtonDown -= new MouseButtonEventHandler(OnOwnerMouseLeftButtonDown);
            owner.KeyDown -= new KeyEventHandler(OnOwnerKeyDown);

            ToolTip toolTip = (ToolTip)owner.GetValue(AssignedToolTipProperty);
            toolTip.TooltipParent = null;
            if (toolTip.IsOpen)
            {
                if (toolTip == _currentToolTip)
                {
                    // unregistering a currently open automatic toltip
                    // thus need to stop the timer 
                    _closeTimer.Stop();
                    _currentToolTip = null;
                    _owner = null;
                    _lastEnterSource = null;
                }

                toolTip.IsOpen = false;
            }

            owner.ClearValue(AssignedToolTipProperty);
        }

        #endregion  Private Methods
    }
}
