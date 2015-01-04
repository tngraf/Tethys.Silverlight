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
// <copyright file="TreeViewItemExtensions.cs" company="Tethys">
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
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Extensions for the class <see cref="TreeViewItem"/>.
    /// </summary>
    public static class TreeViewItemExtensions
    {
        /// <summary>
        /// Gets the depth.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The depth of the specified tree item.</returns>
        public static int GetDepth(this TreeViewItem item)
        {
            TreeViewItem parent;
            while ((parent = GetParent(item)) != null)
            {
                return GetDepth(parent) + 1;
            } // while
            return 0;
        } // GetDepth()

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The parent of the specified <see cref="TreeViewItem"/>.
        /// </returns>
        private static TreeViewItem GetParent(TreeViewItem item)
        {
            var parent = VisualTreeHelper.GetParent(item);
            while (!(parent is TreeViewItem || parent is TreeView))
            {
                parent = VisualTreeHelper.GetParent(parent);
            } // while
            return parent as TreeViewItem;
        } // GetParent()
    } // TreeViewItemExtensions
} // Tethys.Silverlight.Controls
