﻿// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------

using Mono;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace System.Windows
{
    class VisualStateGroupCollection : Collection<VisualStateGroup>
    {
        public VisualStateGroupCollection()
        {
        }
    }

    /// <summary>
    ///     Manages visual states and their transitions on a control.
    /// </summary>
    public class VisualStateManager : DependencyObject
    {
        /// <summary>
        ///     Transitions a control's state.
        /// </summary>
        /// <param name="control">The control who's state is changing.</param>
        /// <param name="stateName">The new state that the control is in.</param>
        /// <param name="useTransitions">Whether to use transition animations.</param>
        /// <returns>true if the state changed successfully, false otherwise.</returns>
        public static bool GoToState(Control control, string stateName, bool useTransitions)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            if (stateName == null)
            {
                throw new ArgumentNullException("stateName");
            }

            FrameworkElement root = GetTemplateRoot(control);
            if (root == null)
            {
                return false; // Ignore state changes if a ControlTemplate hasn't been applied
            }

            IList<VisualStateGroup> groups = GetVisualStateGroupsInternal(root);
            if (groups == null)
            {
                return false;
            }

            // System.Diagnostics.Debug.WriteLine("Going to " + stateName);
            VisualState state;
            VisualStateGroup group;
            if (!TryGetState(groups, stateName, out group, out state))
                return false;

            // Look for a custom VSM, and call it if it was found, regardless of whether the state was found or not.
            // This is because we don't know what the custom VSM will want to do. But for our default implementation,
            // we know that if we haven't found the state, we don't actually want to do anything.
            VisualStateManager customVsm = GetCustomVisualStateManager(root);
            if (customVsm != null)
            {
                return customVsm.GoToStateCore(control, root, stateName, group, state, useTransitions);
            }
            else if (state != null)
            {
                return GoToStateInternal(control, root, group, state, useTransitions);
            }

            return false;
        }

        /// <summary>
        ///     Allows subclasses to override the GoToState logic.
        /// </summary>
        protected virtual bool GoToStateCore(Control control, FrameworkElement templateRoot, string stateName, VisualStateGroup group, VisualState state, bool useTransitions)
        {
            return GoToStateInternal(control, templateRoot, group, state, useTransitions);
        }

        #region CustomVisualStateManager

        public static readonly DependencyProperty CustomVisualStateManagerProperty =
            DependencyProperty.RegisterAttachedCore(
            "CustomVisualStateManager",
            typeof(VisualStateManager),
            typeof(VisualStateManager),
            null);

        public static VisualStateManager GetCustomVisualStateManager(FrameworkElement obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            return obj.GetValue(CustomVisualStateManagerProperty) as VisualStateManager;
        }

        public static void SetCustomVisualStateManager(FrameworkElement obj, VisualStateManager value)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            obj.SetValue(CustomVisualStateManagerProperty, value);
        }

        #endregion

        #region VisualStateGroups
        private static DependencyProperty VisualStateGroupsProperty = DependencyProperty.RegisterAttachedCore("VisualStateGroups",
                                                           typeof(VisualStateGroupCollection),
                                                           typeof(VisualStateManager),
                                                           null);

        internal static VisualStateGroupCollection GetVisualStateGroupsInternal(FrameworkElement obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            VisualStateGroupCollection groups = obj.GetValue(VisualStateGroupsProperty) as VisualStateGroupCollection;
            if (groups == null)
            {
                groups = new VisualStateGroupCollection();
                SetVisualStateGroups(obj, groups);
            }

            return groups;
        }

        public static IList GetVisualStateGroups(FrameworkElement obj)
        {
            return GetVisualStateGroupsInternal(obj);
        }

        internal static void SetVisualStateGroups(FrameworkElement obj, VisualStateGroupCollection value)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            obj.SetValue(VisualStateGroupsProperty, value);
        }

        /*
                private static void OnVisualStateGroupsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
                {
                    FrameworkElement element = obj as FrameworkElement;
                    if (element != null)
                    {
                        Control control = VisualStateManager.GetTemplatedParent(element);
                        if (control != null)
                        {
                            System.Diagnostics.Debug.Assert(element == VisualStateManager.GetTemplateRoot(control));
        //                    Microsoft.Windows.Controls.VisualStateBehaviorFactory.AttachBehavior(control);
                        }
                    }
                }
        */

        #endregion

        #region State Change

        internal static bool TryGetState(IList<VisualStateGroup> groups, string stateName, out VisualStateGroup group, out VisualState state)
        {
            for (int groupIndex = 0; groupIndex < groups.Count; ++groupIndex)
            {
                VisualStateGroup g = groups[groupIndex];
                VisualState s = g.GetState(stateName);
                if (s != null)
                {
                    group = g;
                    state = s;
                    return true;
                }
            }

            group = null;
            state = null;
            return false;
        }

        private static bool GoToStateInternal(Control control, FrameworkElement element, VisualStateGroup group, VisualState state, bool useTransitions)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            if (state == null)
            {
                throw new ArgumentNullException("state");
            }

            if (group == null)
            {
                throw new InvalidOperationException();
            }

            VisualState lastState = group.CurrentState;
            if (lastState == state)
            {
                return true;
            }

            // Get the transition Storyboard. Even if there are no transitions specified, there might
            // be properties that we're rolling back to their default values.
            VisualTransition transition = useTransitions ? GetTransition(element, group, lastState, state) : null;

            // Generate dynamicTransition Storyboard
            Storyboard dynamicTransition = GenerateDynamicTransitionAnimations(element, group, state, transition);
            dynamicTransition.SetValue(Control.IsTemplateItemProperty, true);

            // If the transition is null, then we want to instantly snap. The dynamicTransition will
            // consist of everything that is being moved back to the default state.
            // If the transition.Duration and explicit storyboard duration is zero, then we want both the dynamic
            // and state Storyboards to happen in the same tick, so we start them at the same time.
            if (transition == null || (transition.GeneratedDuration == DurationZero &&
                                            (transition.Storyboard == null || transition.Storyboard.Duration == DurationZero)))
            {
                // Start new state Storyboard and stop any previously running Storyboards
                if (transition != null && transition.Storyboard != null)
                {
                    group.StartNewThenStopOld(element, transition.Storyboard, state.Storyboard);
                }
                else
                {
                    group.StartNewThenStopOld(element, state.Storyboard);
                }

                // Fire both CurrentStateChanging and CurrentStateChanged events
                group.RaiseCurrentStateChanging(element, lastState, state, control);
                group.RaiseCurrentStateChanged(element, lastState, state, control);
            }
            else
            {
                // In this case, we have an interstitial storyboard of duration > 0 and/or
                // explicit storyboard of duration >0 , so we need
                // to run them first, and then we'll run the state storyboard.
                // we have to wait for both storyboards to complete before
                // starting the steady state animations.
                transition.DynamicStoryboardCompleted = false;

                // Hook up generated Storyboard's Completed event handler
                dynamicTransition.Completed += delegate (object sender, EventArgs e)
                {
                    if (transition.Storyboard == null || transition.ExplicitStoryboardCompleted)
                    {
                        group.StartNewThenStopOld(element, state.Storyboard);
                        group.RaiseCurrentStateChanged(element, lastState, state, control);
                    }

                    transition.DynamicStoryboardCompleted = true;
                };

                if (transition.Storyboard != null && transition.ExplicitStoryboardCompleted == true)
                {
                    EventHandler transitionCompleted = null;
                    transitionCompleted = new EventHandler(delegate (object sender, EventArgs e)
                    {
                        if (transition.DynamicStoryboardCompleted)
                        {
                            group.StartNewThenStopOld(element, state.Storyboard);
                            group.RaiseCurrentStateChanged(element, lastState, state, control);
                        }

                        transition.Storyboard.Completed -= transitionCompleted;
                        transition.ExplicitStoryboardCompleted = true;
                    });

                    // hook up explicit storyboard's Completed event handler
                    transition.ExplicitStoryboardCompleted = false;
                    transition.Storyboard.Completed += transitionCompleted;
                }

                // Start transition and dynamicTransition Storyboards
                // Stop any previously running Storyboards
                group.StartNewThenStopOld(element, transition.Storyboard, dynamicTransition);

                group.RaiseCurrentStateChanging(element, lastState, state, control);
            }

            group.CurrentState = state;

            return true;
        }

        protected void RaiseCurrentStateChanging(VisualStateGroup stateGroup, VisualState oldState, VisualState newState, Control control)
        {
            if (stateGroup == null)
            {
                throw new ArgumentNullException("stateGroup");
            }

            if (newState == null)
            {
                throw new ArgumentNullException("newState");
            }

            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            FrameworkElement root = GetTemplateRoot(control);
            if (root == null)
            {
                return; // Ignore if a ControlTemplate hasn't been applied
            }

            stateGroup.RaiseCurrentStateChanging(root, oldState, newState, control);
        }

        protected void RaiseCurrentStateChanged(VisualStateGroup stateGroup, VisualState oldState, VisualState newState, Control control)
        {
            if (stateGroup == null)
            {
                throw new ArgumentNullException("stateGroup");
            }

            if (newState == null)
            {
                throw new ArgumentNullException("newState");
            }

            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            FrameworkElement root = GetTemplateRoot(control);
            if (root == null)
            {
                return; // Ignore if a ControlTemplate hasn't been applied
            }

            stateGroup.RaiseCurrentStateChanged(root, oldState, newState, control);
        }

        #endregion

        #region Transitions

        private static Storyboard GenerateDynamicTransitionAnimations(FrameworkElement root, VisualStateGroup group, VisualState newState, VisualTransition transition)
        {
            Storyboard dynamic = new Storyboard();
            if (transition != null/* && transition.GeneratedDuration != null "this comparison is always true" */)
            {
                dynamic.Duration = transition.GeneratedDuration;
            }
            else
            {
                dynamic.Duration = new Duration(TimeSpan.Zero);
            }

            Dictionary<TimelineDataToken, Timeline> currentAnimations = FlattenTimelines(group.CurrentStoryboards);
            Dictionary<TimelineDataToken, Timeline> transitionAnimations = FlattenTimelines(transition != null ? transition.Storyboard : null);
            Dictionary<TimelineDataToken, Timeline> newStateAnimations = FlattenTimelines(newState.Storyboard);

            // Remove any animations that the transition already animates.
            // There is no need to create an interstitial animation if one already exists.
            foreach (KeyValuePair<TimelineDataToken, Timeline> pair in transitionAnimations)
            {
                currentAnimations.Remove(pair.Key);
                newStateAnimations.Remove(pair.Key);
            }

            // Generate the "to" animations
            foreach (KeyValuePair<TimelineDataToken, Timeline> pair in newStateAnimations)
            {
                // The new "To" Animation -- the root is passed as a reference point for name
                // lookup.
                Timeline toAnimation = GenerateToAnimation(root, pair.Value, true);

                // If the animation is of a type that we can't generate transition animations
                // for, GenerateToAnimation will return null, and we should just keep going.
                if (toAnimation != null)
                {
                    EnsureTarget(root, pair.Value, toAnimation);
                    toAnimation.Duration = dynamic.Duration;
                    dynamic.Children.Add(toAnimation);
                }

                // Remove this from the list of current state animations we have to consider next
                currentAnimations.Remove(pair.Key);
            }

            // Generate the "from" animations
            foreach (KeyValuePair<TimelineDataToken, Timeline> pair in currentAnimations)
            {
                Timeline fromAnimation = GenerateFromAnimation(pair.Value);
                if (fromAnimation != null)
                {
                    EnsureTarget(root, pair.Value, fromAnimation);
                    fromAnimation.Duration = dynamic.Duration;

                    PropertyPath propertyName = Storyboard.GetTargetProperty(pair.Value);
                    Storyboard.SetTargetProperty(fromAnimation, propertyName);
                    dynamic.Children.Add(fromAnimation);
                }
            }

            return dynamic;
        }

        static void EnsureTarget(FrameworkElement root, Timeline source, Timeline dest)
        {
            dest.SetValue(Control.IsTemplateItemProperty, source.GetValue(Control.IsTemplateItemProperty));
            if (source.ManualTarget != null)
            {
                Storyboard.SetTarget(dest, source.ManualTarget);
            }
            else
            {
                string targetName = Storyboard.GetTargetName(source);
                if (!string.IsNullOrEmpty(targetName))
                    Storyboard.SetTargetName(dest, targetName);
            }
        }

        private static Timeline GenerateFromAnimation(Timeline timeline)
        {
            if (timeline is ColorAnimation || timeline is ColorAnimationUsingKeyFrames)
            {
                return new ColorAnimation();
            }

            if (timeline is DoubleAnimation || timeline is DoubleAnimationUsingKeyFrames)
            {
                return new DoubleAnimation();
            }

            if (timeline is PointAnimation || timeline is PointAnimationUsingKeyFrames)
            {
                return new PointAnimation();
            }

            // All other animation types are ignored. We will not build transitions for them,
            // but they will end up being executed.
            return null;
        }

        private static Timeline GenerateToAnimation(FrameworkElement root, Timeline timeline, bool isEntering)
        {
            Timeline result = null;

            Color? targetColor = GetTargetColor(timeline, isEntering);
            if (targetColor.HasValue)
            {
                ColorAnimation ca = new ColorAnimation() { To = targetColor };
                result = ca;
            }

            if (result == null)
            {
                double? targetDouble = GetTargetDouble(timeline, isEntering);
                if (targetDouble.HasValue)
                {
                    DoubleAnimation da = new DoubleAnimation() { To = targetDouble };
                    result = da;
                }
            }

            if (result == null)
            {
                Point? targetPoint = GetTargetPoint(timeline, isEntering);
                if (targetPoint.HasValue)
                {
                    PointAnimation pa = new PointAnimation() { To = targetPoint };
                    result = pa;
                }
            }

            if (result != null)
            {
                string targetName = Storyboard.GetTargetName(timeline);
                Storyboard.SetTargetName(result, targetName);
                if (!String.IsNullOrEmpty(targetName))
                {
                    DependencyObject target = root.FindName(targetName) as DependencyObject;
                    if (target != null)
                    {
                        Storyboard.SetTarget(result, target);
                    }
                }

                Storyboard.SetTargetProperty(result, Storyboard.GetTargetProperty(timeline));
            }

            return result;
        }

        /// <summary>
        /// Get the most appropriate transition between two states.
        /// </summary>
        /// <param name="element">Element being transitioned.</param>
        /// <param name="group">Group being transitioned.</param>
        /// <param name="from">VisualState being transitioned from.</param>
        /// <param name="to">VisualState being transitioned to.</param>
        /// <returns>
        /// The most appropriate transition between the desired states.
        /// </returns>
        internal static VisualTransition GetTransition(FrameworkElement element, VisualStateGroup group, VisualState from, VisualState to)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            if (group == null)
            {
                throw new ArgumentNullException("group");
            }

            if (to == null)
            {
                throw new ArgumentNullException("to");
            }

            VisualTransition best = null;
            VisualTransition defaultTransition = null;
            int bestScore = -1;

            IList<VisualTransition> transitions = (IList<VisualTransition>)group.Transitions;
            if (transitions != null)
            {
                foreach (VisualTransition transition in transitions)
                {
                    if (defaultTransition == null && transition.IsDefault)
                    {
                        defaultTransition = transition;
                        continue;
                    }

                    int score = -1;

                    VisualState transitionFromState = group.GetState(transition.From);
                    VisualState transitionToState = group.GetState(transition.To);

                    if (from == transitionFromState)
                    {
                        score += 1;
                    }
                    else if (transitionFromState != null)
                    {
                        continue;
                    }

                    if (to == transitionToState)
                    {
                        score += 2;
                    }
                    else if (transitionToState != null)
                    {
                        continue;
                    }

                    if (score > bestScore)
                    {
                        bestScore = score;
                        best = transition;
                    }
                }
            }

            return best ?? defaultTransition;
        }

        #endregion

        #region GetTarget Methods

        // These methods are used when generating a transition animation between states.
        // The timeline is the "to" state, and we need to find the To value for the
        // animation we're generating.
        private static Color? GetTargetColor(Timeline timeline, bool isEntering)
        {
            ColorAnimation ca = timeline as ColorAnimation;
            if (ca != null)
            {
                return ca.From.HasValue ? ca.From : ca.To;
            }

            ColorAnimationUsingKeyFrames cak = timeline as ColorAnimationUsingKeyFrames;
            if (cak != null)
            {
                if (cak.KeyFrames.Count == 0)
                {
                    return null;
                }

                ColorKeyFrame keyFrame = cak.KeyFrames[isEntering ? 0 : cak.KeyFrames.Count - 1];
                return keyFrame.Value;
            }

            return null;
        }

        private static double? GetTargetDouble(Timeline timeline, bool isEntering)
        {
            DoubleAnimation da = timeline as DoubleAnimation;
            if (da != null)
            {
                return da.From.HasValue ? da.From : da.To;
            }

            DoubleAnimationUsingKeyFrames dak = timeline as DoubleAnimationUsingKeyFrames;
            if (dak != null)
            {
                if (dak.KeyFrames.Count == 0)
                {
                    return null;
                }

                DoubleKeyFrame keyFrame = dak.KeyFrames[isEntering ? 0 : dak.KeyFrames.Count - 1];
                return keyFrame.Value;
            }

            return null;
        }

        private static Point? GetTargetPoint(Timeline timeline, bool isEntering)
        {
            PointAnimation pa = timeline as PointAnimation;
            if (pa != null)
            {
                return pa.From.HasValue ? pa.From : pa.To;
            }

            PointAnimationUsingKeyFrames pak = timeline as PointAnimationUsingKeyFrames;
            if (pak != null)
            {
                if (pak.KeyFrames.Count == 0)
                {
                    return null;
                }

                PointKeyFrame keyFrame = pak.KeyFrames[isEntering ? 0 : pak.KeyFrames.Count - 1];
                return keyFrame.Value;
            }

            return null;
        }

        #endregion

        #region FlattenTimelines

        // These methods exist to put extract all animations from a Storyboard, and store them in
        // a Dictionary keyed on what element:property is being animated. Storyboards can contain
        // Storyboards, hence the "Flatten".
        private static Dictionary<TimelineDataToken, Timeline> FlattenTimelines(Storyboard storyboard)
        {
            Dictionary<TimelineDataToken, Timeline> result = new Dictionary<TimelineDataToken, Timeline>();

            FlattenTimelines(storyboard, result);

            return result;
        }

        private static Dictionary<TimelineDataToken, Timeline> FlattenTimelines(Collection<Storyboard> storyboards)
        {
            Dictionary<TimelineDataToken, Timeline> result = new Dictionary<TimelineDataToken, Timeline>();

            for (int index = 0; index < storyboards.Count; ++index)
            {
                FlattenTimelines(storyboards[index], result);
            }

            return result;
        }

        private static void FlattenTimelines(Storyboard storyboard, Dictionary<TimelineDataToken, Timeline> result)
        {
            if (storyboard == null)
            {
                return;
            }

            storyboard.FlattenTimelines((timeline_ptr, do_ptr, dp_ptr) =>
            {
                var timeline = (Timeline)NativeDependencyObjectHelper.Lookup(timeline_ptr);
                var dep_ob = (DependencyObject)NativeDependencyObjectHelper.Lookup(do_ptr);
                var dp = (DependencyProperty)DependencyProperty.Lookup(dp_ptr);
                if (dp != null && dep_ob != null)
                    result[new TimelineDataToken(dep_ob, dp)] = timeline;
            });
        }

        // specifies a token to uniquely identify a Timeline object
        private struct TimelineDataToken : IEquatable<TimelineDataToken>
        {
            DependencyProperty property;
            DependencyObject target;

            public TimelineDataToken(DependencyObject targetObject, DependencyProperty targetProperty)
            {
                target = targetObject;
                property = targetProperty;
            }

            public bool Equals(TimelineDataToken other)
            {
                return property == other.property
                    && target == other.target;
            }

            public override bool Equals(object obj)
            {
                if (obj is TimelineDataToken)
                    return Equals((TimelineDataToken)obj);
                return false;
            }

            public override int GetHashCode()
            {
                return target.GetHashCode() ^ property.GetHashCode();
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        ///     Returns the root element of an expanded template for a given control.
        /// </summary>
        private static FrameworkElement GetTemplateRoot(Control control)
        {
            UserControl userControl = control as UserControl;
            if (userControl != null)
            {
                // If using a UserControl, the states will be specified on the
                // root of the content instead of the root of the template.
                return userControl.Content as FrameworkElement;
            }
            else
            {
                if (VisualTreeHelper.GetChildrenCount(control) > 0)
                {
                    return VisualTreeHelper.GetChild(control, 0) as FrameworkElement;
                }
            }

            return null;
        }

        /*
                private static Control GetTemplatedParent(FrameworkElement element)
                {
        //            // If the element has a templated parent, then return that.
        //            DependencyObject templatedParent = element.TemplatedParent;
        //            if (templatedParent != null)
        //            {
        //                return templatedParent as Control;
        //            }

                    // If the element is the root of a UserControl, then it
                    // will not have a templated parent. It's Parent property
                    // should instead be used.
                    UserControl userControl = element.Parent as UserControl;
                    if (userControl != null)
                    {
                        return userControl;
                    }

                    // No parent was found.
                    return null;
                }
        */

        #endregion

        #region Data

        private static readonly Duration DurationZero = new Duration(TimeSpan.Zero);

        #endregion
    }
}
