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
// <copyright file="AssemblyInformation.cs" company="Tethys">
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

namespace Tethys.Silverlight.Diagnostics
{
  using System.Reflection;

  /// <summary>
  /// Determines the assembly information (what is provided by the WinForm
  /// Application class).
  /// </summary>
  public static class AssemblyInformation
  {
    #region PRIVATE PROPERTIES
    /// <summary>
    /// The assembly.
    /// </summary>
    private static readonly Assembly Asm = Assembly.GetEntryAssembly();
    #endregion // PRIVATE PROPERTIES

    //// ----------------------------------------------------------------------

    #region PUBLIC PROPERTIES
    /// <summary>
    /// Gets the title of the assembly.
    /// </summary>
    public static string Title
    {
      get
      {
        var attribs = Asm.GetCustomAttributes(
          typeof(AssemblyTitleAttribute), true);
        if (attribs.Length > 0)
        {
          return ((AssemblyTitleAttribute)attribs[0]).Title;
        } // if

        return string.Empty;
      }
    } // Title

    /// <summary>
    /// Gets the description of the assembly.
    /// </summary>
    public static string Description
    {
      get
      {
        var attribs = Asm.GetCustomAttributes(
          typeof(AssemblyDescriptionAttribute), true);
        if (attribs.Length > 0)
        {
          return ((AssemblyDescriptionAttribute)attribs[0]).Description;
        } // if

        return string.Empty;
      }
    } // Description

    /// <summary>
    /// Gets the configuration of the assembly.
    /// </summary>
    public static string Configuration
    {
      get
      {
        var attribs = Asm.GetCustomAttributes(
          typeof(AssemblyConfigurationAttribute), true);
        if (attribs.Length > 0)
        {
          return ((AssemblyConfigurationAttribute)attribs[0]).Configuration;
        } // if

        return string.Empty;
      }
    } // Configuration

    /// <summary>
    /// Gets the company name of the assembly.
    /// </summary>
    public static string Company
    {
      get
      {
        var attribs = Asm.GetCustomAttributes(
          typeof(AssemblyCompanyAttribute), true);
        if (attribs.Length > 0)
        {
          return ((AssemblyCompanyAttribute)attribs[0]).Company;
        } // if

        return string.Empty;
      }
    } // Company

    /// <summary>
    /// Gets the product name of the assembly.
    /// </summary>
    public static string Product
    {
      get
      {
        var attribs = Asm.GetCustomAttributes(
          typeof(AssemblyProductAttribute), true);
        if (attribs.Length > 0)
        {
          return ((AssemblyProductAttribute)attribs[0]).Product;
        } // if

        return string.Empty;
      }
    } // Product

    /// <summary>
    /// Gets the copyright of the assembly.
    /// </summary>
    public static string Copyright
    {
      get
      {
        var attribs = Asm.GetCustomAttributes(
          typeof(AssemblyCopyrightAttribute), true);
        if (attribs.Length > 0)
        {
          return ((AssemblyCopyrightAttribute)attribs[0]).Copyright;
        } // if

        return string.Empty;
      }
    } // Copyright

    /// <summary>
    /// Gets the trademark of the assembly.
    /// </summary>
    public static string Trademark
    {
      get
      {
        var attribs = Asm.GetCustomAttributes(
          typeof(AssemblyTrademarkAttribute), true);
        if (attribs.Length > 0)
        {
          return ((AssemblyTrademarkAttribute)attribs[0]).Trademark;
        } // if

        return string.Empty;
      }
    } // Trademark

    /// <summary>
    /// Gets the version of the assembly.
    /// </summary>
    public static string Version
    {
      get
      {
        return Asm.GetName().Version.ToString();
      }
    } // Version
    #endregion // PUBLIC PROPERTIES
  } // AssemblyInformation
} // Tethys.Silverlight.Diagnostics
