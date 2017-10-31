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
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// Abstract application configuration class.
    /// </summary>
    public abstract class AppConfigBase : IAppConfig
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// The application configuration filename.
        /// </summary>
        protected string AppConfigFilename = "Config.config";
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region PROTECTED METHODS
        /// <summary>
        /// Gets the user app data path.
        /// </summary>
        /// <param name="includeVersion">if set to <c>true</c> [include version].</param>
        /// <returns>
        /// The user app data path.
        /// </returns>
        /// <remarks>
        /// This a WPF replacement for
        /// System.Windows.Forms.Application.UserAppDataPath
        /// </remarks>
        protected static string GetUserAppDataPath(bool includeVersion)
        {
            return "config";
        } // GetUserAppDataPath()

        /// <summary>
        /// Gets the config filename.
        /// </summary>
        /// <returns>The config filename.</returns>
        protected string GetConfigFilename()
        {
            string fullpath = GetUserAppDataPath(false);
            fullpath = fullpath + "\\" + this.AppConfigFilename;

            return fullpath;
        } // GetConfigFilename()

        /// <summary>
        /// Checks that the path exists.
        /// </summary>
        /// <param name="path">The path.</param>
        protected void CheckPathExists(string path)
        {
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
        /// <returns>The element value.</returns>
        protected static string GetElementValue(XElement parent, string name)
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
