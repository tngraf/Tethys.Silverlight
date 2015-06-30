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
// <copyright file="DialogHelper.cs" company="Tethys">
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

namespace Tethys.Silverlight.Support
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;

    using Tethys.Silverlight.Helper;

    /// <summary>
    /// Helper class to show/hide dialog title bar buttons.
    /// Based on code of <c>Jörg Neumann</c> published in DotNetPro 06/2012.
    /// </summary>
    public static class DialogHelper
    {
        #region PROPERTIES

        #region ShowMinimizeButton
        /// <summary>
        /// ShowMinimizeButtonProperty DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty ShowMinimizeButtonProperty =
            DependencyProperty.RegisterAttached("ShowMinimizeButton", typeof(bool),
            typeof(DialogHelper),
            new UIPropertyMetadata(true, OnButtonChanged));

        /// <summary>
        /// Gets the show minimize button.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The show minimize button value.</returns>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetShowMinimizeButton(DependencyObject element)
        {
            return (bool)element.GetValue(ShowMinimizeButtonProperty);
        }

        /// <summary>
        /// Sets the show minimize button.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetShowMinimizeButton(DependencyObject element, bool value)
        {
            element.SetValue(ShowMinimizeButtonProperty, value);
        }
        #endregion

        #region ShowMaximizeButton
        /// <summary>
        /// ShowMaximizeButtonProperty DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty ShowMaximizeButtonProperty =
            DependencyProperty.RegisterAttached("ShowMaximizeButton", typeof(bool), typeof(DialogHelper),
            new UIPropertyMetadata(true, OnButtonChanged));

        /// <summary>
        /// Gets the show maximize button.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The show maximize button value.</returns>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetShowMaximizeButton(DependencyObject element)
        {
            return (bool)element.GetValue(ShowMaximizeButtonProperty);
        }

        /// <summary>
        /// Sets the show maximize button.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetShowMaximizeButton(DependencyObject element, bool value)
        {
            element.SetValue(ShowMaximizeButtonProperty, value);
        }
        #endregion

        #region ShowHelpButton
        /// <summary>
        /// ShowHelpButtonProperty DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty ShowHelpButtonProperty =
            DependencyProperty.RegisterAttached("ShowHelpButton", typeof(bool), typeof(DialogHelper),
            new UIPropertyMetadata(false, OnButtonChanged));

        /// <summary>
        /// Gets the show help button.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The show help button value.</returns>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetShowHelpButton(DependencyObject element)
        {
            return (bool)element.GetValue(ShowHelpButtonProperty);
        }

        /// <summary>
        /// Sets the show help button.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetShowHelpButton(DependencyObject element, bool value)
        {
            element.SetValue(ShowHelpButtonProperty, value);
        }
        #endregion

        #region WindowStartupLocation
        /// <summary>
        /// WindowStartupLocationProperty DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty WindowStartupLocationProperty =
            DependencyProperty.RegisterAttached("WindowStartupLocation",
            typeof(WindowStartupLocation), typeof(DialogHelper),
            new PropertyMetadata(WindowStartupLocation.Manual,
              OnWindowStartupLocationPropertyChanged));

        /// <summary>
        /// Called when the window startup location property has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        private static void OnWindowStartupLocationPropertyChanged(
          DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var window = sender as Window;
            if (window != null)
            {
                window.WindowStartupLocation = (WindowStartupLocation)e.NewValue;
            }
        }

        /// <summary>
        /// Gets the window startup location.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The window startup location value.</returns>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static WindowStartupLocation GetWindowStartupLocation(DependencyObject element)
        {
            return (WindowStartupLocation)element.GetValue(WindowStartupLocationProperty);
        }

        /// <summary>
        /// Sets the window startup location.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetWindowStartupLocation(DependencyObject element, WindowStartupLocation value)
        {
            element.SetValue(WindowStartupLocationProperty, value);
        }
        #endregion

        #region ShowIcon
        /// <summary>
        /// HideIconProperty DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty HideIconProperty =
            DependencyProperty.RegisterAttached("HideIcon", typeof(bool),
            typeof(DialogHelper),
            new UIPropertyMetadata(false, OnButtonChanged));

        /// <summary>
        /// Gets the hide icon.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>the hide icon value.</returns>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetHideIcon(DependencyObject element)
        {
            return (bool)element.GetValue(HideIconProperty);
        }

        /// <summary>
        /// Sets the hide icon.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetHideIcon(DependencyObject element, bool value)
        {
            element.SetValue(HideIconProperty, value);
        }
        #endregion

        #endregion

        //// ----------------------------------------------------------------------

        #region Event Handling
        /// <summary>
        /// Called when a button has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The
        /// <see cref="System.Windows.DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        private static void OnButtonChanged(DependencyObject sender,
          DependencyPropertyChangedEventArgs e)
        {
            var window = sender as Window;
            if (window != null)
            {
                var handle = new WindowInteropHelper(window).Handle;
                if (handle == IntPtr.Zero)
                {
                    window.SourceInitialized += OnSourceInitialized;
                }
                else
                {
                    UpdateStyle(window);
                } // if
            } // if
        } // OnButtonChanged()

        /// <summary>
        /// Called when the source has been initialized.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance 
        /// containing the event data.</param>
        private static void OnSourceInitialized(object sender, EventArgs e)
        {
            var window = sender as Window;
            if (window != null)
            {
                window.SourceInitialized -= OnSourceInitialized;
                UpdateStyle(window);
            } // if
        } // OnSourceInitialized()
        #endregion

        //// ----------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Updates the style.
        /// </summary>
        /// <param name="window">The window.</param>
        private static void UpdateStyle(Window window)
        {
            var handle = new WindowInteropHelper(window).Handle;
            var style = NativeMethods.GetWindowLong(handle, NativeMethods.Style);

            if (GetShowMaximizeButton(window))
            {
                style |= NativeMethods.MaximizeBox;
            }
            else
            {
                style &= ~NativeMethods.MaximizeBox;
            } // if

            if (GetShowMinimizeButton(window))
            {
                style |= NativeMethods.MinimizeBox;
            }
            else
            {
                style &= ~NativeMethods.MinimizeBox;
            } // if

            NativeMethods.SetWindowLong(handle, NativeMethods.Style, style);

            var extendedStyle = NativeMethods.GetWindowLong(handle, NativeMethods.ExtStyle);
            if (GetShowHelpButton(window))
            {
                extendedStyle |= NativeMethods.ContextHelp;
            }
            else
            {
                extendedStyle &= -(~NativeMethods.ContextHelp);
            } // if

            if (GetHideIcon(window))
            {
                extendedStyle |= NativeMethods.DialogModalFrame;
            }
            else
            {
                extendedStyle &= -(~NativeMethods.DialogModalFrame);
            } // if

            NativeMethods.SetWindowLong(handle, NativeMethods.ExtStyle, extendedStyle);

            // Update the window's non-client area to reflect the changes
            NativeMethods.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0,
              NativeMethods.NoMove | NativeMethods.NoSize | NativeMethods.NoZOrder | NativeMethods.SwpFramechanged);

            if (GetHideIcon(window))
            {
                NativeMethods.SendMessage(handle, NativeMethods.Seticon, IntPtr.Zero, IntPtr.Zero);
            } // if
        } // UpdateStyle()
        #endregion // PRIVATE METHODS
    } // DialogHelper
} // Tethys.Silverlight.Support
