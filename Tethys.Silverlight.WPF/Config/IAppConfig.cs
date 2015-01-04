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
// <copyright file="IAppConfig.cs" company="Tethys">
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
  /// <summary>
  /// Interface for application configurations.
  /// </summary>
  public interface IAppConfig
  {
    /// <summary>
    /// Loads application settings from isolated storage.
    /// </summary>
    void Load();

    /// <summary>
    /// Saves application settings to isolated storage.
    /// </summary>
    void Save();
  } // IAppConfig
} // Tethys.Silverlight.Config