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
// <copyright file="AppConfigBase.cs" company="Tethys">
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

namespace Tethys.Silverlight.Config
{
  using System;
  using System.IO;
  using System.Reflection;
  using System.Xml;
  using System.Xml.Linq;

  /// <summary>
  /// Abstract application configuration class.
  /// </summary>
  public abstract class AppConfigBase : IAppConfig
  {
    #region PRIVATE PROPERTIES
    #endregion // PRIVATE PROPERTIES

    //// ---------------------------------------------------------------------

    #region PUBLIC PROPERTIES
    /// <summary>
    /// Default config file name.
    /// </summary>
    protected const string AppConfigFilename = "Config.config";
    #endregion // PUBLIC PROPERTIES

    //// ---------------------------------------------------------------------

    #region CONSTRUCTION
    #endregion // CONSTRUCTION

    //// ---------------------------------------------------------------------

    #region PROTECTED METHODS
    /// <summary>
    /// Gets the user app data path.
    /// </summary>
    /// <param name="includeVersion">if set to <c>true</c> [include version].</param>
    /// <returns>The user AppData path.</returns>
    /// <remarks>
    /// This a WPF replacement for
    /// System.Windows.Forms.Application.UserAppDataPath
    /// </remarks>
    protected static string GetUserAppDataPath(bool includeVersion)
    {
      // Get the .EXE assembly
      Assembly assm = Assembly.GetEntryAssembly();
      
      // Get a 'Type' of the AssemblyCompanyAttribute
      Type act = typeof(AssemblyCompanyAttribute);
      
      // Get a collection of custom attributes from the .EXE assembly
      object[] rc = assm.GetCustomAttributes(act, false);
      
      // Get the Company Attribute
      AssemblyCompanyAttribute ct =
                    (AssemblyCompanyAttribute)rc[0];

      Type aps = typeof(AssemblyProductAttribute);
      
      // Get a collection of custom attributes from the .EXE assembly
      object[] rp = assm.GetCustomAttributes(aps, false);
      
      // Get the Company Attribute
      AssemblyProductAttribute pt =
                    (AssemblyProductAttribute)rp[0];

      // Build the User App Data Path
      string path = Environment.GetFolderPath(
        Environment.SpecialFolder.ApplicationData);
      path += @"\" + ct.Company;
      path += @"\" + pt.Product;
      if (includeVersion)
      {
        path += @"\" + assm.GetName().Version;
      } // if

      return path;
    } // GetUserAppDataPath()

    /// <summary>
    /// Gets the config filename.
    /// </summary>
    /// <returns>The config file name.</returns>
    protected string GetConfigFilename()
    {
      string fullpath = GetUserAppDataPath(false);
      fullpath = fullpath + "\\" + AppConfigFilename;

      return fullpath;
    } // GetConfigFilename()

    /// <summary>
    /// Checks that the path exists.
    /// </summary>
    /// <param name="path">The path.</param>
    protected void CheckPathExists(string path)
    {
      Directory.CreateDirectory(path);
    } // CheckPathExists()

    /// <summary>
    /// Saves application settings to isolated storage.
    /// </summary>
    public abstract void Save();

    /// <summary>
    /// Loads application settings from isolated storage.
    /// </summary>
    public abstract void Load();

    /// <summary>
    /// Gets the element value.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="name">The name.</param>
    /// <returns>The element value</returns>
    protected static string GetElementValue(XContainer parent, string name)
    {
      var help = parent.Element(name);
      if (help == null)
      {
        throw new XmlException("Element not found: " + name);
      } // if

      return help.Value;
    } // GetElementValue()
    #endregion // PROTECTED METHODS
  } // AppConfig
} // Tethys.Silverlight.Config
