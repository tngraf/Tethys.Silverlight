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
// The idea for this code has been taken from the Apex framework 
// (http://apex.codeplex.com/), written by David Kerr and license by an
// MIT style license.
//
// ===========================================================================
//
// <copyright file="EasyGrid.cs" company="Tethys">
// Copyright  2010-2016 by Thomas Graf
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
    using System.Collections.Generic;
#if NETFX_CORE || UNIVERSAL_APP81 || WINDOWS_UWP
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
#else
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
#endif

    using GridLengthConverter = Tethys.Silverlight.Converter.GridLengthConverter;

    /// <summary>
    /// The EasyGrid control is a Grid that supports easy definition of
    /// rows and columns.
    /// </summary>
    public class EasyGrid : Grid
    {
        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>The rows.</value>
#if !NETFX_CORE && !UNIVERSAL_APP81 && !WINDOWS_UWP
        [Description("The rows property."), Category("Common Properties")]
#endif
        public string Rows
        {
            get { return (string)this.GetValue(RowsProperty); }
            set { this.SetValue(RowsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>The columns.</value>
#if !NETFX_CORE && !UNIVERSAL_APP81 && !WINDOWS_UWP
        [Description("The columns property."), Category("Common Properties")]
#endif
        public string Columns
        {
            get { return (string)this.GetValue(ColumnsProperty); }
            set { this.SetValue(ColumnsProperty, value); }
        }

        /// <summary>
        /// Called when the rows property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="args">The
        ///  <see cref="DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        private static void OnRowsChanged(DependencyObject dependencyObject,
          DependencyPropertyChangedEventArgs args)
        {
            // Get the apex grid.
            EasyGrid easyGrid = dependencyObject as EasyGrid;
            if (easyGrid == null)
            {
                return;
            } // if

            // Clear any current rows definitions.
            easyGrid.RowDefinitions.Clear();

            // Add each row from the row lengths definition.
            foreach (var rowLength in StringLengthsToGridLengths(easyGrid.Rows))
            {
                easyGrid.RowDefinitions.Add(new RowDefinition { Height = rowLength });
            } // foreach
        } // InRowsChanged()

        /// <summary>
        /// Called when the columns property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="args">The 
        /// <see cref="DependencyPropertyChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private static void OnColumnsChanged(DependencyObject dependencyObject,
          DependencyPropertyChangedEventArgs args)
        {
            // Get the apex grid.
            EasyGrid easyGrid = dependencyObject as EasyGrid;
            if (easyGrid == null)
            {
                return;
            } // if

            // Clear any current column definitions.
            easyGrid.ColumnDefinitions.Clear();

            // Add each column from the column lengths definition.
            foreach (var columnLength in StringLengthsToGridLengths(easyGrid.Columns))
            {
                easyGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = columnLength });
            } // foreach
        } // OnColumnsChanged()

        /// <summary>
        /// Turns a string of lengths, such as "3*,Auto,2000" into a set 
        /// of grid length.
        /// </summary>
        /// <param name="lengths">The string of lengths, separated by commas.</param>
        /// <returns>A list of GridLengths.</returns>
        private static IEnumerable<GridLength> StringLengthsToGridLengths(
          string lengths)
        {
            // Create the list of GridLengths.
            List<GridLength> gridLengths = new List<GridLength>();

            // If the string is null or empty, this is all we can do.
            if (string.IsNullOrEmpty(lengths))
            {
                return gridLengths;
            } // if

            // Split the string by comma.
            string[] theLengths = lengths.Split(',');

            // Use a consistency grid length converter.
            GridLengthConverter gridLengthConverter
              = new GridLengthConverter();

            // Convert the lengths.
            foreach (var length in theLengths)
            {
                gridLengths.Add(gridLengthConverter.ConvertFromString(length));
            } // foreach

            // Return the grid lengths.
            return gridLengths;
        } // StringLengthsToGridLengths()

        /// <summary>
        /// The rows dependency property.
        /// </summary>
        private static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register("Rows", typeof(string), typeof(EasyGrid),
            new PropertyMetadata(null, OnRowsChanged));

        /// <summary>
        /// The columns dependency property.
        /// </summary>
        private static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(string), typeof(EasyGrid),
            new PropertyMetadata(null, OnColumnsChanged));
    } // EasyGrid 
} // Tethys.Silverlight.Controls
