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
// <copyright file="VisibleWhenWindowsConverter.cs" company="Tethys">
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
#if NETFX_CORE || UNIVERSAL_APP81 || WINDOWS_UWP
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;
#else
    using System.Windows;
    using System.Windows.Data;
#endif    

    /// <summary>
    /// A converter that returns <c>Visibility.Visible</c> if the current app is
    /// running on Windows; otherwise <c>Visibility.Collapsed</c> is returned.
    /// </summary>
    public class VisibleWhenWindowsConverter : IValueConverter
    {
#region IVALUECONVERTER MEMBERS
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
        public object Convert(object value, Type targetType, object parameter,
          System.Globalization.CultureInfo culture)
        {
#if WINDOWS_APP
            return Visibility.Visible;
#else
            return Visibility.Collapsed;
#endif
        } // Convert()

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
            throw new NotImplementedException();
        } // ConvertBack()

#if NETFX_CORE || UNIVERSAL_APP81 || WINDOWS_UWP
        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter,
          string language)
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
        /// <param name="language">The language.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter,
          string language)
        {
            return this.ConvertBack(value, targetType, parameter,
                System.Globalization.CultureInfo.InvariantCulture);
        } // ConvertBack()
#endif
#endregion // IVALUECONVERTER MEMBERS
    } // VisibleWhenWindowsConverter
} // Tethys.Silverlight.Converter
