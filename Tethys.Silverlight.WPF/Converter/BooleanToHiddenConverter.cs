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
// <copyright file="BooleanToHiddenConverter.cs" company="Tethys">
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

namespace Tethys.Silverlight.Converter
{
    using System;
    using System.Diagnostics.CodeAnalysis;
#if NETFX_CORE || UNIVERSAL_APP81 || WINDOWS_UWP
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;
#else
    using System.Windows;
    using System.Windows.Data;
#endif

    /// <summary>
    /// Converts a boolean value to a Visibility value required by XAML.
    /// <para />
    /// Boolean == true => Visibility = Collapsed
    /// Boolean == false => Visibility = Visible
    /// </summary>
    public class BooleanToHiddenConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "This is ok here")]
        public object Convert(object value, Type targetType, object parameter,
          System.Globalization.CultureInfo culture)
        {
            var rv = Visibility.Collapsed;
            try
            {
                var x = bool.Parse(value.ToString());
                rv = x ? Visibility.Collapsed : Visibility.Visible;
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
                // ignore
            } // if

            return rv;
        } // Convert()

#if NETFX_CORE || UNIVERSAL_APP81 || WINDOWS_UWP
        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="text">The text.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter,
          string text)
        {
            return this.Convert(value, targetType, parameter,
              System.Globalization.CultureInfo.InvariantCulture);
        } // Convert()

        /// <summary>
        /// Converts a value back.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="text">The text.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter,
          string text)
        {
            return this.ConvertBack(value, targetType, parameter,
              System.Globalization.CultureInfo.InvariantCulture);
        } // ConvertBack()
#endif

        /// <summary>
        /// Converts a value back.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter,
          System.Globalization.CultureInfo culture)
        {
            return value;
        } // ConvertBack()
    } // BooleanToHiddenConverter
} // Tethys.Silverlight.Converter
