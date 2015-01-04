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
// <copyright file="EnumHelper.cs" company="Tethys">
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

namespace Tethys.Silverlight.Helper
{
  using System;
  using System.Linq;

  /// <summary>
  /// The enumeration helper class.
  /// </summary>
  public static class EnumHelper
  {
    /// <summary>
    /// Gets the enumeration values.
    /// </summary>
    /// <param name="enumType">Type of the enumeration.</param>
    /// <returns>A value.</returns>
    public static object[] GetValues(Type enumType)
    {
      // Sanity check.
      if (!enumType.IsEnum)
      {
        throw new ArgumentException("Type '" + enumType.Name
          + "' is not an enum");
      } // if

      // Get the fields.
      var fields = from field in enumType.GetFields()
                   where field.IsLiteral
                   select field;

      // Get the values.
      return fields.Select(field => field.GetValue(enumType)).ToArray();
    } // GetValues()
  } // EnumHelper
} // Tethys.Silverlight.Helper
