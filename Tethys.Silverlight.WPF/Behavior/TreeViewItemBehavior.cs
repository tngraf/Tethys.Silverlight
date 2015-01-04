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
// <copyright file="TreeViewItemBehavior.cs" company="Tethys">
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

    /// <summary>
    /// A behavior that allows to scroll the selected item of a tree view
    /// into the the current view area.
    /// </summary>
    public static class TreeViewItemBehavior
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// The dependency property that allows use to bind the
        /// "BringIntoViewWhenSelected" property
        /// </summary>
        public static readonly DependencyProperty BringIntoViewWhenSelectedProperty =
            DependencyProperty.RegisterAttached("BringIntoViewWhenSelected", 
            typeof(bool), typeof(TreeViewItemBehavior), 
            new UIPropertyMetadata(false, OnBringIntoViewWhenSelectedChanged));
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Gets the 'bring into view when selected' property.
        /// </summary>
        /// <param name="treeViewItem">The tree view item.</param>
        /// <returns>The value of the dependency property.</returns>
        public static bool GetBringIntoViewWhenSelected(DependencyObject treeViewItem)
        {
            return (bool)treeViewItem.GetValue(BringIntoViewWhenSelectedProperty);
        } // GetBringIntoViewWhenSelected()

        /// <summary>
        /// Sets the 'bring into view when selected' property.
        /// </summary>
        /// <param name="treeViewItem">The tree view item.</param>
        /// <param name="value">if set to <c>true</c> the specified tree item is brought
        /// into view.</param>
        public static void SetBringIntoViewWhenSelected(DependencyObject treeViewItem,
            bool value)
        {
            treeViewItem.SetValue(BringIntoViewWhenSelectedProperty, value);
        } // SetBringIntoViewWhenSelected()
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Called when the value of the 'bring into view when selected' dependency 
        /// property has changed.
        /// </summary>
        /// <param name="depObj">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private static void OnBringIntoViewWhenSelectedChanged(DependencyObject depObj,
            DependencyPropertyChangedEventArgs e)
        {
            var item = depObj as TreeViewItem;
            if (item == null)
            {
                return;
            } // if

            if (e.NewValue is bool == false)
            {
                return;
            } // if

            if ((bool)e.NewValue)
            {
                item.BringIntoView();
            } // if
        } // OnBringIntoViewWhenSelectedChanged()
        #endregion // PRIVATE METHODS
    } // TreeViewItemBehavior
} // Tethys.Silverlight.Behavior
