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
// <copyright file="GridLengthConverter.cs" company="Tethys">
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
#else
    using System.Windows;
#endif

    /// <summary>
    /// A string to grid length converter.
    /// </summary>
    public class GridLengthConverter
    {
        /// <summary>
        /// Create a grid length from a string, consistently in WPF, WP7 and SL.
        /// </summary>
        /// <param name="gridLength">The grid length, e.g. 4*, Auto, 23 etc.</param>
        /// <returns>A grid length.</returns>
        public GridLength ConvertFromString(string gridLength)
        {
            // If we're NOT in silverlight, we have a gridlength converter
            // we can use.
#if !SILVERLIGHT && !NETFX_CORE
            // Create the standard windows grid length converter.
            var gridLengthConverter = new System.Windows.GridLengthConverter();

            // Return the converted grid length.
            return (GridLength)gridLengthConverter.ConvertFromString(gridLength);

#else
            // We are in silverlight and do not have a grid length converter.
            // We can do the conversion by hand.

            // Auto is easy.
            if (gridLength == "Auto")
            {
                return new GridLength(1, GridUnitType.Auto);
            } // if

            if (gridLength.Contains("*"))
            {
                // It's a starred value, remove the star and get the coefficient as a double.
                double coefficient = 1;
                var starVal = gridLength.Replace("*", string.Empty);

                // If there is a coefficient, try and convert it.
                // If we fail, throw an exception.
                if (starVal.Length > 0 && double.TryParse(starVal, out coefficient) == false)
                {
                    throw new Exception("'" + gridLength + "' is not a valid value.");
                } // if

                // We've handled the star value.
                return new GridLength(coefficient, GridUnitType.Star);
            } // if

            // It's not auto or star, so unless it's a plain old pixel 
            // value we must throw an exception.
            double pixelVal;
            if (double.TryParse(gridLength, out pixelVal) == false)
            {
                throw new Exception("'" + gridLength + "' is not a valid value.");
            } // if

            // We've handled the star value.
            return new GridLength(pixelVal, GridUnitType.Pixel);
#endif
        } // ConvertFromString
    } // GridLengthConverter
} // Tethys.Silverlight.Converter
