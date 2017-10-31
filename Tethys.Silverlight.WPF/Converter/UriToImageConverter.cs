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
// <copyright file="UriToImageConverter.cs" company="Tethys">
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
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media.Imaging;
#else
    using System.Windows.Data;
    using System.Windows.Media.Imaging;
#endif

    /// <summary>
    /// A URI to image converter.
    /// </summary>
    public class UriToImageConverter : IValueConverter
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
        public object Convert(object value, Type targetType, object parameter,
          System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            } // if

            if (value is string)
            {
                value = new Uri((string)value, UriKind.RelativeOrAbsolute);
            } // if

            if (value is Uri)
            {
                BitmapImage bi = new BitmapImage();
#if !NETFX_CORE && !UNIVERSAL_APP81 && !WINDOWS_UWP
                bi.BeginInit();
#endif
                bi.DecodePixelWidth = 80;
                //// bi.DecodePixelHeight = 60;    
                bi.UriSource = (Uri)value;
#if !NETFX_CORE && !UNIVERSAL_APP81 && !WINDOWS_UWP
                bi.EndInit();
#endif
                return bi;
            } // if

            return null;
        } // Convert

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
        public object ConvertBack(object value, Type targetType, object parameter,
      System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        } // ConvertBack()
    } // UriToImageConverter
} // Tethys.Silverlight.Converter
