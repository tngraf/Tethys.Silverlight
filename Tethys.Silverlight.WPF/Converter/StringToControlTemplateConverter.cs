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
// <copyright file="StringToControlTemplateConverter.cs" company="Tethys">
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
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
#if NETFX_CORE
  using Windows.UI.Xaml;
  using Windows.UI.Xaml.Controls;
  using Windows.UI.Xaml.Data;
#else
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
#endif

    /// <summary>
    /// Converts a string property to control template. The control template
    /// must be in the applications resource dictionary.
    /// </summary>
    public class StringToControlTemplateConverter : IValueConverter
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
            ControlTemplate ctrlTemplate = null;

            try
            {
                object obj;
#if NETFX_CORE
        obj = Application.Current.Resources[value];
#else
                obj = Application.Current.FindResource(value);
#endif
                ctrlTemplate = obj as ControlTemplate;
            }
            catch
            {
                // ignore
                Debug.WriteLine("ControlTemplate '{0}' not found!", value);
            } // catch

            return ctrlTemplate;
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
      return this.Convert(value, targetType, parameter, System.Globalization.CultureInfo.InvariantCulture);
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
      return this.ConvertBack(value, targetType, parameter, System.Globalization.CultureInfo.InvariantCulture);
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
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        } // ConvertBack()
    } // StringToControlTemplateConverter
} // Tethys.Silverlight.Converter
