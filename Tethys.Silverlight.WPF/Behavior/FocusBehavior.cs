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
// <copyright file="FocusBehavior.cs" company="Tethys">
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

namespace Tethys.Silverlight.Behavior
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    /*
     * See http://igrali.com/2013/09/18/focus-a-textbox-from-viewmodel-using-a-simple-behavior/
     */

    /// <summary>
    /// Behavior to set the focus to a <see cref="TextBox"/> control.
    /// </summary>
    public class FocusBehavior : Behavior<TextBox>
    {
        /// <summary>
        /// The <c>IsFocused</c> property.
        /// </summary>
        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.Register("IsFocused", typeof(bool),
                typeof(FocusBehavior), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether this control is focused.
        /// </summary>
        public bool IsFocused
        {
            get { return (bool)this.GetValue(IsFocusedProperty); }
            set { this.SetValue(IsFocusedProperty, value); }
        }

        /// <summary>
        /// Called when the behavior is attached.
        /// </summary>
        protected override void OnAttached()
        {
            this.AssociatedObject.GotFocus += (sender, args) => this.IsFocused = true;
            this.AssociatedObject.LostFocus += (sender, a) => this.IsFocused = false;
            this.AssociatedObject.TextChanged += (sender, a) =>
            {
                if (!this.IsFocused)
                {
                    this.AssociatedObject.Focus();
                    this.AssociatedObject.Select(this.AssociatedObject.Text.Length, 0);
                }
            };
            base.OnAttached();
        } // OnAttached()
    } // FocusBehavior
} // Tethys.Silverlight.Behavior
