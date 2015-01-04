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
// <copyright file="EnhancedTreeView.cs" company="Tethys">
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
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// This tree view class allows data binding the
    /// "SelectedObject" property
    /// </summary>
    public class EnhancedTreeView : TreeView
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// The dependency property that allows use to bind the
        /// "SelectedObject" property
        /// </summary>
        public static readonly DependencyProperty
          SelectedObjectProperty =
            DependencyProperty.Register(
              "SelectedObject", typeof(object),
              typeof(EnhancedTreeView),
              new PropertyMetadata(SelectedObjectChangedCallback));

        /// <summary>
        /// Gets or sets the select object (a writable version of
        /// the "SelectedItem" property)
        /// </summary>
        [Bindable(true)]
        public object SelectedObject
        {
            get
            {
                return this.GetValue(SelectedObjectProperty);
            }

            set
            {
                this.SetValue(SelectedObjectProperty, value);
            }
        } // SelectedObject
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region PROTECTED METHODS
        /// <summary>
        /// When the selected item is updated from inside the tree,
        /// this method will update the "SelectedObject" property.
        /// </summary>
        /// <param name="e">Provides the item that was previously selected and 
        /// the item that is currently selected for the 
        /// <see cref="E:System.Windows.Controls.TreeView.SelectedItemChanged" />
        /// event.</param>
        protected override void OnSelectedItemChanged(
            RoutedPropertyChangedEventArgs<object> e)
        {
            this.SelectedObject = e.NewValue;

            base.OnSelectedItemChanged(e);
        } // OnSelectedItemChanged()
        #endregion // PROTECTED METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// This method is called whenever ever the selected
        /// object is changed, and if it was changed from the
        /// outside, this method will set the selected item.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="eventArgs">The <see cref="DependencyPropertyChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private static void SelectedObjectChangedCallback(DependencyObject obj,
           DependencyPropertyChangedEventArgs eventArgs)
        {
            var treeView = (EnhancedTreeView)obj;
            if (!object.ReferenceEquals(treeView.SelectedItem, eventArgs.NewValue))
            {
                SelectItem(treeView, eventArgs.NewValue);

                var childTreeNode = treeView.ItemContainerGenerator
                  .ContainerFromItem(eventArgs.NewValue) as TreeViewItem;
                if (childTreeNode != null)
                {
                    childTreeNode.BringIntoView();
                } // if
            } // if
        } // SelectedObjectChangedCallback()

        /// <summary>
        /// Searches the given item in the parent (recursively)
        /// and selects it, returns true if the item was found
        /// and selected, false otherwise.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="itemToSelect">The item to select.</param>
        /// <returns><c>true</c> if the item has been found and selected;
        /// otherwise <c>false</c>.</returns>
        private static bool SelectItem(ItemsControl parent, object itemToSelect)
        {
            if (parent == null)
            {
                return false;
            } // if

            var childTreeNode = parent.ItemContainerGenerator
                .ContainerFromItem(itemToSelect) as TreeViewItem;

            // if the item to select is directly under "parent",
            // just select it
            if (childTreeNode != null)
            {
                childTreeNode.Focus();
                return childTreeNode.IsSelected = true;
            } // if

            // if the item to select is not directly under "parent",
            // search the child nodes of "parent"
            if (parent.Items.Count > 0)
            {
                foreach (object childItem in parent.Items)
                {
                    var childItemsControl = parent.ItemContainerGenerator
                        .ContainerFromItem(childItem) as ItemsControl;

                    if (SelectItem(childItemsControl, itemToSelect))
                    {
                        return true;
                    } // if
                } // foreach
            } // if

            // if the given item wasn't found ...
            return false;
        } // SelectItem()
        #endregion // PRIVATE METHODS
    } // EnhancedTreeView
} // Tethys.Silverlight.Controls
