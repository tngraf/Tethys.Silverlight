#region Header
// --------------------------------------------------------------------------
// Tethys.Silverlight
// ==========================================================================
//
// This library contains common code for WPF, Silverlight, Windows Phone and
// Windows 8 projects.
//
// ===========================================================================
//
// The code of the TiltEffect class has been posted by Peter Torr on his blog
// at http://blogs.msdn.com/b/ptorr/archive/2010/08/11/updated-tilt-effect.aspx
// and is presumably (c) by Peter Torr.
//
// <copyright file="TiltEffect.cs" company="Tethys">
// Copyright  2010-2015 by Thomas Graf
//            All rights reserved.
//            Licensed under the Apache License, Version 2.0.
//            Unless required by applicable law or agreed to in writing, 
//            software distributed under the License is distributed on an
//            "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
//            either express or implied. 
// </copyright>
//
// System ... Microsoft .Net Framework 4.5
// Tools .... Microsoft Visual Studio 2013
//
// ---------------------------------------------------------------------------
#endregion

namespace Tethys.Silverlight.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    using Microsoft.Phone.Controls;

    /// <summary>
    /// Provides attached properties for adding a 'tilt' effect to all
    /// controls within a container
    /// </summary>
    public class TiltEffect : DependencyObject
    {
        #region PRIVATE PROPERTIES
        // These constants are the same as the built-in effects

        /// <summary>
        /// Maximum amount of tilt, in radians
        /// </summary>
        private const double MaxAngle = 0.3;

        /// <summary>
        /// Maximum amount of depression, in pixels
        /// </summary>
        private const double MaxDepression = 25;

        /// <summary>
        /// Delay between releasing an element and the tilt release animation playing
        /// </summary>
        private static readonly TimeSpan TiltReturnAnimationDelay
          = TimeSpan.FromMilliseconds(200);

        /// <summary>
        /// Duration of tilt release animation
        /// </summary>
        private static readonly TimeSpan TiltReturnAnimationDuration
          = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// The control that is currently being tilted
        /// </summary>
        private static FrameworkElement currentTiltElement;

        /// <summary>
        /// The single instance of a storyboard used for all tilts
        /// </summary>
        private static Storyboard tiltReturnStoryboard;

        /// <summary>
        /// The single instance of an X rotation used for all tilts
        /// </summary>
        private static DoubleAnimation tiltReturnXAnimation;

        /// <summary>
        /// The single instance of a Y rotation used for all tilts
        /// </summary>
        private static DoubleAnimation tiltReturnYAnimation;

        /// <summary>
        /// The single instance of a Z depression used for all tilts
        /// </summary>
        private static DoubleAnimation tiltReturnZAnimation;

        /// <summary>
        /// The center of the tilt element
        /// </summary>
        private static Point currentTiltElementCenter;

        /// <summary>
        /// Whether the animation just completed was for a 'pause' or not
        /// </summary>
        private static bool wasPauseAnimation;

        /// <summary>
        /// Gets or sets a value indicating whether to use a slightly more 
        /// accurate (but slightly slower) tilt animation easing function.
        /// </summary>
        public static bool UseLogarithmicEase { get; set; }

        /// <summary>
        /// Gets the default list of items that are tiltable.
        /// </summary>
        public static List<Type> TiltableItems { get; private set; }
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Prevents a default instance of the <see cref="TiltEffect"/> class 
        /// from being created. This is not a class that can get instantiated, 
        /// but can't be static because it derives from DependencyObject.
        /// </summary>
        private TiltEffect()
        {
        } // TiltEffect()

        /// <summary>
        /// Initializes static members of the <see cref="TiltEffect"/> class.
        /// </summary>
        static TiltEffect()
        {
            // For extra fun, add this to the list:
            // typeof(Microsoft.Phone.Controls.PhoneApplicationPage)
            TiltableItems = new List<Type> 
            { 
                typeof(ButtonBase),
                typeof(ListBoxItem), 
            };
            UseLogarithmicEase = false;
        } // TiltEffect()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region DEPENDENCY PROPERTIES
        /// <summary>
        /// Whether the tilt effect is enabled on a container (and all its children)
        /// </summary>
        public static readonly DependencyProperty IsTiltEnabledProperty
          = DependencyProperty.RegisterAttached("IsTiltEnabled",
          typeof(bool), typeof(TiltEffect),
          new PropertyMetadata(OnIsTiltEnabledChanged));

        /// <summary>
        /// Gets the IsTiltEnabled dependency property from an object
        /// </summary>
        /// <param name="source">The object to get the property from</param>
        /// <returns>The property's value</returns>
        public static bool GetIsTiltEnabled(DependencyObject source)
        {
            return (bool)source.GetValue(IsTiltEnabledProperty);
        } // GetIsTiltEnabled()

        /// <summary>
        /// Sets the IsTiltEnabled dependency property on an object
        /// </summary>
        /// <param name="source">The object to set the property on</param>
        /// <param name="value">The value to set</param>
        public static void SetIsTiltEnabled(DependencyObject source, bool value)
        {
            source.SetValue(IsTiltEnabledProperty, value);
        } // SetIsTiltEnabled()

        /// <summary>
        /// Suppresses the tilt effect on a single control that would otherwise be tilted
        /// </summary>
        public static readonly DependencyProperty SuppressTiltProperty
          = DependencyProperty.RegisterAttached("SuppressTilt",
          typeof(bool), typeof(TiltEffect), null);

        /// <summary>
        /// Gets the SuppressTilt dependency property from an object
        /// </summary>
        /// <param name="source">The object to get the property from</param>
        /// <returns>The property's value</returns>
        public static bool GetSuppressTilt(DependencyObject source)
        {
            return (bool)source.GetValue(SuppressTiltProperty);
        } // SetSuppressTilt()

        /// <summary>
        /// Sets the SuppressTilt dependency property from an object
        /// </summary>
        /// <param name="source">The object to get the property from</param>
        /// <param name="value">The value to be set.</param>
        public static void SetSuppressTilt(DependencyObject source, bool value)
        {
            source.SetValue(SuppressTiltProperty, value);
        } // SetSuppressTilt()

        /// <summary>
        /// Property change handler for the IsTiltEnabled dependency property.
        /// </summary>
        /// <param name="target">The element that the property is attached to.</param>
        /// <param name="args">Event args</param>
        /// <remarks>
        /// Adds or removes event handlers from the element that has (un)registered for tilting
        /// </remarks>
        private static void OnIsTiltEnabledChanged(DependencyObject target,
          DependencyPropertyChangedEventArgs args)
        {
            if (target is FrameworkElement)
            {
                // Add / remove our event handler if necessary
                if ((bool)args.NewValue == true)
                {
                    (target as FrameworkElement).ManipulationStarted
                      += TiltEffectManipulationStarted;
                }
                else
                {
                    (target as FrameworkElement).ManipulationStarted
                      -= TiltEffectManipulationStarted;
                } // if
            } // if
        } // OnIsTiltEnabledChanged()
        #endregion // DEPENDENCY PROPERTIES

        //// ---------------------------------------------------------------------

        #region TOP-LEVEL MANIPULATION EVENT HANDLERS
        /// <summary>
        /// Event handler for ManipulationStarted
        /// </summary>
        /// <param name="sender">sender of the event - this will be the tilt
        /// container (e.g., entire page)</param>
        /// <param name="e">event args</param>
        private static void TiltEffectManipulationStarted(object sender,
          ManipulationStartedEventArgs e)
        {
            Debug.WriteLine("Started: " + e.ManipulationOrigin.X + ", "
              + e.ManipulationOrigin.Y);
            TryStartTiltEffect(sender as FrameworkElement, e);
        } // TiltEffectManipulationStarted()

        /// <summary>
        /// Event handler for ManipulationDelta
        /// </summary>
        /// <param name="sender">sender of the event - this will be the tilting
        /// object (e.g. a button)</param>
        /// <param name="e">event args</param>
        private static void TiltEffectManipulationDelta(object sender,
          ManipulationDeltaEventArgs e)
        {
            Debug.WriteLine("Delta: " + e.ManipulationOrigin.X + ", "
              + e.ManipulationOrigin.Y);
            ContinueTiltEffect(sender as FrameworkElement, e);
        } // TiltEffectManipulationDelta()

        /// <summary>
        /// Event handler for ManipulationCompleted
        /// </summary>
        /// <param name="sender">sender of the event - this will be the tilting
        /// object (e.g. a button)</param>
        /// <param name="e">event args</param>
        private static void TiltEffectManipulationCompleted(object sender,
          ManipulationCompletedEventArgs e)
        {
            Debug.WriteLine("Completed: " + e.ManipulationOrigin.X + ", "
              + e.ManipulationOrigin.Y);
            EndTiltEffect(currentTiltElement);
        } // TiltEffectManipulationCompleted()
        #endregion // TOP-LEVEL MANIPULATION EVENT HANDLERS

        //// ---------------------------------------------------------------------

        #region CORE TILT LOGIC
        /// <summary>
        /// Checks if the manipulation should cause a tilt, and if so starts the
        /// tilt effect
        /// </summary>
        /// <param name="source">The source of the manipulation (the tilt 
        /// container, e.g. entire page)</param>
        /// <param name="e">The args from the ManipulationStarted event</param>
        private static void TryStartTiltEffect(FrameworkElement source,
          ManipulationStartedEventArgs e)
        {
            foreach (FrameworkElement ancestor in (e.OriginalSource
              as FrameworkElement).GetVisualAncestors())
            {
                foreach (Type t in TiltableItems)
                {
                    if (t.IsAssignableFrom(ancestor.GetType()))
                    {
                        if ((bool)ancestor.GetValue(SuppressTiltProperty) != true)
                        {
                            // Use first child of the control, so that we can add transforms
                            // and not impact any transforms on the control itself
                            var element = VisualTreeHelper.GetChild(ancestor,
                              0) as FrameworkElement;
                            var container = e.ManipulationContainer
                              as FrameworkElement;

                            if (element == null || container == null)
                            {
                                return;
                            }

                            // touch point relative to the element being tilted
                            var tiltTouchPoint = container.TransformToVisual(
                              element).Transform(e.ManipulationOrigin);

                            // center of the element being tilted
                            var elementCenter = new Point(element.ActualWidth / 2,
                              element.ActualHeight / 2);

                            // Camera adjustment
                            var centerToCenterDelta = GetCenterToCenterDelta(element,
                              source);

                            BeginTiltEffect(element, tiltTouchPoint, elementCenter,
                              centerToCenterDelta);
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Computes the delta between the centre of an element and its container
        /// </summary>
        /// <param name="element">The element to compare</param>
        /// <param name="container">The element to compare against</param>
        /// <returns>A point that represents the delta between the two centers</returns>
        private static Point GetCenterToCenterDelta(FrameworkElement element,
          FrameworkElement container)
        {
            var elementCenter = new Point(element.ActualWidth / 2,
              element.ActualHeight / 2);
            Point containerCenter;

#if WINDOWS_PHONE

            // need to special-case the frame because it lies about its width / height
            if (container is PhoneApplicationFrame)
            {
                var frame = container as PhoneApplicationFrame;

                // Switch width and height in landscape mode
                if ((frame.Orientation & PageOrientation.Landscape)
                  == PageOrientation.Landscape)
                {
                    Debug.WriteLine("Switching container coordinates because it's the root frame");
                    containerCenter = new Point(container.ActualHeight / 2,
                      container.ActualWidth / 2);
                }
                else
                {
                    containerCenter = new Point(container.ActualWidth / 2,
                      container.ActualHeight / 2);
                } // if
            }
            else
            {
                containerCenter = new Point(container.ActualWidth / 2,
                  container.ActualHeight / 2);
            } // if
#else
              containerCenter = new Point(container.ActualWidth / 2,
              container.ActualHeight / 2);
#endif

            var transformedElementCenter = element.TransformToVisual(
              container).Transform(elementCenter);
            var result = new Point(containerCenter.X - transformedElementCenter.X,
              containerCenter.Y - transformedElementCenter.Y);
            Debug.WriteLine("Transforming center " + transformedElementCenter
              + " to " + containerCenter + "; got " + result);
            return result;
        }

        /// <summary>
        /// Begins the tilt effect by preparing the control and doing the initial
        /// animation
        /// </summary>
        /// <param name="element">The element to tilt </param>
        /// <param name="touchPoint">The touch point, in element coordinates</param>
        /// <param name="centerPoint">The center point of the element in element
        /// coordinates</param>
        /// <param name="centerDelta">The delta between the 
        /// <paramref name="element"/>'s center and 
        /// the container's center</param>
        private static void BeginTiltEffect(FrameworkElement element, Point touchPoint,
          Point centerPoint, Point centerDelta)
        {
            Debug.WriteLine("BeginTilt: " + touchPoint + " / " + centerPoint + " / "
              + centerDelta);

            if (tiltReturnStoryboard != null)
            {
                StopTiltReturnStoryboardAndCleanup();
            } // if

            if (PrepareControlForTilt(element, centerDelta) == false)
            {
                return;
            } // if

            currentTiltElement = element;
            currentTiltElementCenter = centerPoint;
            PrepareTiltReturnStoryboard(element);

            ApplyTiltEffect(currentTiltElement, touchPoint,
              currentTiltElementCenter);
        } // BeginTiltEffect()

        /// <summary>
        /// Prepares a control to be tilted by setting up a plane projection
        /// and some event handlers
        /// </summary>
        /// <param name="element">The control that is to be tilted</param>
        /// <param name="centerDelta">Delta between the element's center and the
        /// tilt container's</param>
        /// <returns>true if successful; false otherwise</returns>
        /// <remarks>
        /// This method is pretty conservative; it will fail any attempt to tilt
        /// a control that already
        /// has a projection on it
        /// </remarks>
        private static bool PrepareControlForTilt(FrameworkElement element,
          Point centerDelta)
        {
            // Don't clobber any existing transforms
            if (element.Projection != null
                || (element.RenderTransform != null 
                && element.RenderTransform.GetType() != typeof(MatrixTransform)))
            {
                return false;
            } // if

            var transform = new TranslateTransform();
            transform.X = centerDelta.X;
            transform.Y = centerDelta.Y;
            element.RenderTransform = transform;

            var projection = new PlaneProjection();
            projection.GlobalOffsetX = -1 * centerDelta.X;
            projection.GlobalOffsetY = -1 * centerDelta.Y;
            element.Projection = projection;

            element.ManipulationDelta += TiltEffectManipulationDelta;
            element.ManipulationCompleted += TiltEffectManipulationCompleted;

            return true;
        } // PrepareControlForTilt()

        /// <summary>
        /// Removes modifications made by PrepareControlForTilt
        /// </summary>
        /// <param name="element">THe control to be un-prepared</param>
        /// <remarks>
        /// This method is pretty basic; it doesn't do anything to detect if the
        /// control being un-prepared
        /// was previously prepared
        /// </remarks>
        private static void RevertPrepareControlForTilt(FrameworkElement element)
        {
            element.ManipulationDelta -= TiltEffectManipulationDelta;
            element.ManipulationCompleted -= TiltEffectManipulationCompleted;
            element.Projection = null;
            element.RenderTransform = null;
        } // RevertPrepareControlForTilt()

        /// <summary>
        /// Creates the tilt return storyboard (if not already created) and 
        /// targets it to the projection
        /// </summary>
        /// <param name="element">the projection that should be the target of
        /// the animation</param>
        private static void PrepareTiltReturnStoryboard(FrameworkElement element)
        {
            if (tiltReturnStoryboard == null)
            {
                tiltReturnStoryboard = new Storyboard();
                tiltReturnStoryboard.Completed += TiltReturnStoryboardCompleted;

                tiltReturnXAnimation = new DoubleAnimation();
                Storyboard.SetTargetProperty(tiltReturnXAnimation,
                  new PropertyPath(PlaneProjection.RotationXProperty));
                tiltReturnXAnimation.BeginTime = TiltReturnAnimationDelay;
                tiltReturnXAnimation.To = 0;
                tiltReturnXAnimation.Duration = TiltReturnAnimationDuration;

                tiltReturnYAnimation = new DoubleAnimation();
                Storyboard.SetTargetProperty(tiltReturnYAnimation,
                  new PropertyPath(PlaneProjection.RotationYProperty));
                tiltReturnYAnimation.BeginTime = TiltReturnAnimationDelay;
                tiltReturnYAnimation.To = 0;
                tiltReturnYAnimation.Duration = TiltReturnAnimationDuration;

                tiltReturnZAnimation = new DoubleAnimation();
                Storyboard.SetTargetProperty(tiltReturnZAnimation,
                  new PropertyPath(PlaneProjection.GlobalOffsetZProperty));
                tiltReturnZAnimation.BeginTime = TiltReturnAnimationDelay;
                tiltReturnZAnimation.To = 0;
                tiltReturnZAnimation.Duration = TiltReturnAnimationDuration;

                if (UseLogarithmicEase)
                {
                    tiltReturnXAnimation.EasingFunction = new LogarithmicEase();
                    tiltReturnYAnimation.EasingFunction = new LogarithmicEase();
                    tiltReturnZAnimation.EasingFunction = new LogarithmicEase();
                }

                tiltReturnStoryboard.Children.Add(tiltReturnXAnimation);
                tiltReturnStoryboard.Children.Add(tiltReturnYAnimation);
                tiltReturnStoryboard.Children.Add(tiltReturnZAnimation);
            }

            Storyboard.SetTarget(tiltReturnXAnimation, element.Projection);
            Storyboard.SetTarget(tiltReturnYAnimation, element.Projection);
            Storyboard.SetTarget(tiltReturnZAnimation, element.Projection);
        }

        /// <summary>
        /// Continues a tilt effect that is currently applied to an element,
        /// presumably because
        /// the user moved their finger
        /// </summary>
        /// <param name="element">The element being tilted</param>
        /// <param name="e">The manipulation event args</param>
        private static void ContinueTiltEffect(FrameworkElement element,
          ManipulationDeltaEventArgs e)
        {
            var container = e.ManipulationContainer as FrameworkElement;
            if (container == null || element == null)
            {
                return;
            } // if

            var tiltTouchPoint = container.TransformToVisual(element).Transform(
              e.ManipulationOrigin);

            // If touch moved outside bounds of element, then pause the tilt
            // (but don't cancel it)
            if (new Rect(0, 0, currentTiltElement.ActualWidth,
              currentTiltElement.ActualHeight).Contains(tiltTouchPoint) != true)
            {
                Debug.WriteLine("Pause at " + tiltTouchPoint.X + ", "
                  + tiltTouchPoint.Y);
                PauseTiltEffect();
                return;
            }

            // Apply the updated tilt effect
            ApplyTiltEffect(currentTiltElement, e.ManipulationOrigin,
              currentTiltElementCenter);
        }

        /// <summary>
        /// Ends the tilt effect by playing the animation  
        /// </summary>
        /// <param name="element">The element being tilted</param>
        private static void EndTiltEffect(FrameworkElement element)
        {
            if (element != null)
            {
                element.ManipulationCompleted -= TiltEffectManipulationCompleted;
                element.ManipulationDelta -= TiltEffectManipulationDelta;
            }

            if (tiltReturnStoryboard != null)
            {
                wasPauseAnimation = false;
                if (tiltReturnStoryboard.GetCurrentState() != ClockState.Active)
                {
                    tiltReturnStoryboard.Begin();
                } // if
            }
            else
            {
                StopTiltReturnStoryboardAndCleanup();
            } // if
        } // EndTiltEffect()

        /// <summary>
        /// Handler for the storyboard complete event
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">event args</param>
        private static void TiltReturnStoryboardCompleted(object sender, EventArgs e)
        {
            if (wasPauseAnimation)
            {
                ResetTiltEffect(currentTiltElement);
            }
            else
            {
                StopTiltReturnStoryboardAndCleanup();
            } // if
        } // TiltReturnStoryboardCompleted()

        /// <summary>
        /// Resets the tilt effect on the control, making it appear 'normal' again 
        /// </summary>
        /// <param name="element">The element to reset the tilt on</param>
        /// <remarks>
        /// This method doesn't turn off the tilt effect or cancel any current
        /// manipulation; it just temporarily cancels the effect
        /// </remarks>
        private static void ResetTiltEffect(FrameworkElement element)
        {
            var projection = element.Projection as PlaneProjection;
            if (projection == null)
            {
                return;
            } // if

            projection.RotationY = 0;
            projection.RotationX = 0;
            projection.GlobalOffsetZ = 0;
        } // ResetTiltEffect()

        /// <summary>
        /// Stops the tilt effect and release resources applied to the
        /// currently-tilted control
        /// </summary>
        private static void StopTiltReturnStoryboardAndCleanup()
        {
            if (tiltReturnStoryboard != null)
            {
                tiltReturnStoryboard.Stop();
            } // if

            RevertPrepareControlForTilt(currentTiltElement);
        } // StopTiltReturnStoryboardAndCleanup()

        /// <summary>
        /// Pauses the tilt effect so that the control returns to the 'at rest'
        /// position, but doesn't
        /// stop the tilt effect (handlers are still attached, etc.)
        /// </summary>
        private static void PauseTiltEffect()
        {
            if ((tiltReturnStoryboard != null) && !wasPauseAnimation)
            {
                tiltReturnStoryboard.Stop();
                wasPauseAnimation = true;
                tiltReturnStoryboard.Begin();
            } // if
        } // PauseTiltEffect()

        /// <summary>
        /// Resets the storyboard to not running
        /// </summary>
        private static void ResetTiltReturnStoryboard()
        {
            tiltReturnStoryboard.Stop();
            wasPauseAnimation = false;
        } // ResetTiltReturnStoryboard()

        /// <summary>
        /// Applies the tilt effect to the control
        /// </summary>
        /// <param name="element">the control to tilt</param>
        /// <param name="touchPoint">The touch point, in the container's 
        /// coordinates</param>
        /// <param name="centerPoint">The center point of the container</param>
        private static void ApplyTiltEffect(FrameworkElement element, Point touchPoint,
          Point centerPoint)
        {
            // Kill any active animation
            ResetTiltReturnStoryboard();

            // Get relative point of the touch in percentage of container size
            var normalizedPoint = new Point(
                Math.Min(Math.Max(touchPoint.X / (centerPoint.X * 2), 0), 1),
                Math.Min(Math.Max(touchPoint.Y / (centerPoint.Y * 2), 0), 1));

            // Magic math from shell...
            var xMagnitude = Math.Abs(normalizedPoint.X - 0.5);
            var yMagnitude = Math.Abs(normalizedPoint.Y - 0.5);
            var xDirection = -Math.Sign(normalizedPoint.X - 0.5);
            var yDirection = Math.Sign(normalizedPoint.Y - 0.5);
            var angleMagnitude = xMagnitude + yMagnitude;
            var xAngleContribution = xMagnitude + yMagnitude > 0 ? xMagnitude / (xMagnitude + yMagnitude) : 0;

            var angle = angleMagnitude * MaxAngle * 180 / Math.PI;
            var depression = (1 - angleMagnitude) * MaxDepression;

            // RotationX and RotationY are the angles of rotations about the x- or y-*axis*;
            // to achieve a rotation in the x- or y-*direction*, we need to swap the two.
            // That is, a rotation to the left about the y-axis is a rotation to the left in the x-direction,
            // and a rotation up about the x-axis is a rotation up in the y-direction.
            var projection = element.Projection as PlaneProjection;
            if (projection == null)
            {
                return;
            } // if

            projection.RotationY = angle * xAngleContribution * xDirection;
            projection.RotationX = angle * (1 - xAngleContribution) * yDirection;
            projection.GlobalOffsetZ = -depression;
        }
        #endregion // CORE TILT LOGIC

        //// ---------------------------------------------------------------------

        #region CUSTOM EASING FUNCTION
        /// <summary>
        /// Provides an easing function for the tilt return
        /// </summary>
        private class LogarithmicEase : EasingFunctionBase
        {
            /// <summary>
            /// Computes the easing function
            /// </summary>
            /// <param name="normalizedTime">The time</param>
            /// <returns>The eased value</returns>
            protected override double EaseInCore(double normalizedTime)
            {
                return Math.Log(normalizedTime + 1) / 0.693147181; // ln(t + 1) / ln(2)
            } // EaseInCore()
        } // LogarithmicEase
        #endregion // CUSTOM EASING FUNCTION
    } // TiltEffect

    /// <summary>
    /// Couple of simple helpers for walking the visual tree
    /// </summary>
    public static class TreeHelpers
    {
        /// <summary>
        /// Gets the ancestors of the element, up to the root
        /// </summary>
        /// <param name="node">The element to start from</param>
        /// <returns>An enumerator of the ancestors</returns>
        public static IEnumerable<FrameworkElement> GetVisualAncestors(this
      FrameworkElement node)
        {
            FrameworkElement parent = node.GetVisualParent();
            while (parent != null)
            {
                yield return parent;
                parent = parent.GetVisualParent();
            } // while
        } // GetVisualAncestors()

        /// <summary>
        /// Gets the visual parent of the element
        /// </summary>
        /// <param name="node">The element to check</param>
        /// <returns>The visual parent</returns>
        public static FrameworkElement GetVisualParent(this FrameworkElement node)
        {
            return VisualTreeHelper.GetParent(node) as FrameworkElement;
        } // GetVisualParent()
    } // TreeHelpers
} // Tethys.Silverlight.Controls
