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
// <copyright file="BooleanToVisibilityConverter.cs" company="Tethys">
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

namespace Tethys.Silverlight.Converter
{
    using System;
#if NETFX_CORE
  using Windows.UI.Xaml;
  using Windows.UI.Xaml.Data;
#else
    using System.Windows;
    using System.Windows.Data;
#endif

    /// <summary>
    /// Converts a boolean value to a Visibility value required by XAML.
    /// <para />
    /// Boolean == true => Visibility = Visible
    /// Boolean == false => Visibility = Collapsed
    /// <para />
    /// Boolean == true, Parameter=invert => Visibility = Collapsed
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
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
            // If the data isn't a bool, bail.
            if (value is bool == false)
            {
                return null;
            } // if

            // Cast the data.
            bool boolean = (bool)value;

            // If we have the invert string, return the inverted value.
            if (parameter != null && parameter.ToString().ToUpperInvariant() == "INVERT")
            {
                return boolean ? Visibility.Collapsed : Visibility.Visible;
            } // if

            return boolean ? Visibility.Visible : Visibility.Collapsed;
        } // Convert()

#if NETFX_CORE
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
        /// Converts a value.
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
        #endregion // IVALUECONVERTER MEMBERS
    } // BooleanToVisibilityConverter
} // Tethys.Silverlight.Converter
