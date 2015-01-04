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
// <copyright file="PasswordHelper.cs" company="Tethys">
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
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// A help class for passwords.
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public static string Password { get; set; }

#if WINDOWS_PHONE
        /// <summary>
        /// The password property.
        /// </summary>
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",
            typeof(string), typeof(PasswordHelper),
            new PropertyMetadata(string.Empty, OnPasswordPropertyChanged));
#else
        /// <summary>
        /// The password property.
        /// </summary>
    public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.RegisterAttached("Password",
        typeof(string), typeof(PasswordHelper),
        new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));
#endif
        /// <summary>
        /// The attach property.
        /// </summary>
        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach",
            typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, Attach));

#if WINDOWS_PHONE
        /// <summary>
        /// The 'is updating' property.
        /// </summary>
        private static readonly DependencyProperty IsUpdatingProperty =
           DependencyProperty.RegisterAttached("IsUpdating", typeof(bool),
           typeof(PasswordHelper), new PropertyMetadata(false));
#else
        /// <summary>
        /// The 'is updating' property.
        /// </summary>
        private static readonly DependencyProperty IsUpdatingProperty =
            DependencyProperty.RegisterAttached("IsUpdating", typeof(bool),
            typeof(PasswordHelper));
#endif

        /// <summary>
        /// Sets the attach.
        /// </summary>
        /// <param name="dp">The <see cref="DependencyObject"/>.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        } // SetAttach()

        /// <summary>
        /// Gets the attach.
        /// </summary>
        /// <param name="dp">The <see cref="DependencyObject"/>.</param>
        /// <returns>The attached property.</returns>
        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        } // GetAttach()

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <param name="dp">The <see cref="DependencyObject"/>.</param>
        /// <returns>The password.</returns>
        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        } // GetPassword()

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <param name="dp">The <see cref="DependencyObject"/>.</param>
        /// <param name="value">The value.</param>
        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        } // SetPassword()

        /// <summary>
        /// Gets the is updating.
        /// </summary>
        /// <param name="dp">The <see cref="DependencyObject"/>.</param>
        /// <returns>The is updating property.</returns>
        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        } // GetIsUpdating()

        /// <summary>
        /// Sets the is updating.
        /// </summary>
        /// <param name="dp">The <see cref="DependencyObject"/>.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        } // SetIsUpdating()

        /// <summary>
        /// Called when [password property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance 
        /// containing the event data.</param>
        private static void OnPasswordPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox == null)
            {
                return;
            } // if

            passwordBox.PasswordChanged -= PasswordChanged;

            if (!GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            } // if

            passwordBox.PasswordChanged += PasswordChanged;
        } // OnPasswordPropertyChanged()

        /// <summary>
        /// Attaches the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        private static void Attach(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;

            if (passwordBox == null)
            {
                return;
            } // if

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            } // if

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            } // if
        } // Attach()

        /// <summary>
        /// Called when the passwords has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing
        /// the event data.</param>
        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox == null)
            {
                return;
            } // if

            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
            Password = passwordBox.Password;
        } // PasswordChanged()
    } // PasswordHelper
} // Tethys.Silverlight.Controls
