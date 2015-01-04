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
// <copyright file="Conversion.cs" company="Tethys">
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

namespace Tethys.Silverlight.Media
{
  using System.Windows.Media;

  /// <summary>
  /// Conversion methods.
  /// </summary>
  public static class Conversion
  {
    /// <summary>
    /// Converts a System.Drawing.Color value to a
    /// System.Windows.Media.Color value.
    /// </summary>
    /// <param name="color">A System.Drawing.Color value.</param>
    /// <returns>A System.Windows.Media.Color value.</returns>
    public static Color DrawingToMediaColor(System.Drawing.Color color)
    {
      var colorNew = new Color();
      colorNew.R = color.R;
      colorNew.G = color.G;
      colorNew.B = color.B;
      colorNew.A = color.A;

      return colorNew;
    } // DrawingToMediaColor()

    /// <summary>
    /// Converts a System.Windows.Media.Color value to a
    /// System.Drawing.Color value.
    /// </summary>
    /// <param name="color">A System.Windows.Media.Color value.</param>
    /// <returns>A System.Drawing.Color value.</returns>
    public static System.Drawing.Color MediaToDrawingColor(Color color)
    {
      System.Drawing.Color colorNew = System.Drawing.Color.FromArgb(
        color.A, color.R, color.G, color.B);
      
      return colorNew;
    } // MediaToDrawingColor()
  } // Conversion
} // Tethys.Silverlight.Media
