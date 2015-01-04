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
// <copyright file="DoubleToColorConverter.cs" company="Tethys">
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
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Data;
    using System.Windows.Media;

    /*
     * Index    Code        Color
     * -----    ----        -----
     *   0      {#FF000000} Black
     *   1      {#FF0000FF} Blue
     *   2      {#FFA52A2A} Brown
     *   3      {#FF00FFFF} Cyan
     *   4      {#FFA9A9A9} Dark Gray
     *   5      {#FF808080} Gray
     *   6      {#FF008000} Green
     *   7      {#FFD3D3D3} Light Gray
     *   8      {#FFFF00FF} Magenta
     *   9      {#FFFFA500} Orange
     *   a      {#FF800080} Purple
     *   b      {#FFFF0000} Red
     *   c      {#00FFFFFF} Transparent
     *   d      {#FFFFFFFF} White
     *   e      {#FFFFFF00} Yellow
     */

    /// <summary>
    /// Converts a double in the range 0..1 to one of the color codes specified
    /// in the color table.
    /// </summary>
    public class DoubleToColorConverter : IValueConverter
    {
        /// <summary>
        /// Converts a double value to a color.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The <see cref="T:System.Type" /> of data expected 
        /// by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter 
        /// logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        /// <returns>
        /// The value to be passed to the target dependency property.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, 
            CultureInfo culture)
        {
            if (parameter as string == "name")
            {
                var values = GetEnumNames<Colors>();
                var input = (double)value;
                var index = (int)(input * values.Length);
                if ((index >= 0) && (index < values.Length))
                {
                    return values[index];
                } // if

                return values[1];
            }
            else
            {
                var values = GetEnumValues<Colors, Color>();
                var input = (double)value;
                var index = (int)(input * values.Length);
                if ((index >= 0) && (index < values.Length))
                {
                    return values[index];
                } // if

                return values[1];
            } // if
        } // Convert()

        /// <summary>
        /// Does NOT convert a color to a double!!
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The <see cref="T:System.Type" /> of data expected
        /// by the source object.</param>
        /// <param name="parameter">An optional parameter to be used in the converter 
        /// logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        /// <returns>
        /// The value to be passed to the source object.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, 
            CultureInfo culture)
        {
            throw new NotSupportedException();
        } // ConvertBack()

        /// <summary>
        /// Gets the enumeration values.
        /// </summary>
        /// <typeparam name="T1">The first type.</typeparam>
        /// <typeparam name="T2">The second type.</typeparam>
        /// <returns><c>T2</c> values.</returns>
        public static T2[] GetEnumValues<T1, T2>()
        {
            var type = typeof(T1);

            return
                (from property in type.GetProperties(
                     BindingFlags.Public | BindingFlags.Static)
                select (T2)property.GetValue(null, null)).ToArray();
        } // GetEnumValues<T1,T2>()

        /// <summary>
        /// Gets the enumeration names of the given type using reflection.
        /// </summary>
        /// <typeparam name="T">A type.</typeparam>
        /// <returns>The enumeration names of the given type.</returns>
        public static string[] GetEnumNames<T>()
        {
            var type = typeof(T);

            return
                (from property in type.GetProperties(
                     BindingFlags.Public | BindingFlags.Static)
                select property.Name).ToArray();
        } // GetEnumNames<T>()
    }  // DoubleToColorConverter()
} // Tethys.Silverlight.Converter
