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
// <copyright file="FocusOnLoadedBehavior.cs" company="Tethys">
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

    /// <summary>
    /// UI Behavior to set focus to the associated object on loaded event.
    /// </summary>
    public class FocusOnLoadedBehavior : Behavior<Control>
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// The set focus on loaded property.
        /// </summary>
        public static readonly DependencyProperty SetFocusOnLoadedProperty =
            DependencyProperty.Register("SetFocusOnLoaded", typeof(bool), 
            typeof(FocusOnLoadedBehavior), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether the focus has to be set to the 
        /// associated object on loaded event.
        /// By default <value>true</value>.
        /// </summary>
        public bool SetFocusOnLoaded
        {
            get { return (bool)this.GetValue(SetFocusOnLoadedProperty); }
            set { this.SetValue(SetFocusOnLoadedProperty, value); }
        }
        #endregion // PUBLIC PROPERTIES

        //// -----------------------------------------------------------------

        #region BEHAVIOR OVERRIDES
        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.Loaded += this.AssociatedObjectLoaded;
        } // OnAttached()

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, 
        /// but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.Loaded -= this.AssociatedObjectLoaded;
        } // OnDetaching()
        #endregion // BEHAVIOR OVERRIDES

        //// -----------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Called when the object has been loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the
        /// event data.</param>
        private void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            this.AssociatedObject.Loaded -= this.AssociatedObjectLoaded;

            if (this.SetFocusOnLoaded)
            {
                this.AssociatedObject.Focus();
            } // if
        } // AssociatedObjectLoaded()
        #endregion // PRIVATE METHODS
    } // FocusOnLoadedBehavior
} // Tethys.Silverlight.Behavior
