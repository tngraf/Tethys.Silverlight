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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO.IsolatedStorage;

    /// <summary>
    /// abstract base class for application configuration.
    /// </summary>
    public abstract class AppConfigBase
    {
        #region PROTECTED PROPERTIES
        /// <summary>
        /// Our isolated storage settings.
        /// </summary>
        protected IsolatedStorageSettings isolatedStore;
        #endregion // PROTECTED PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Saves application settings to isolated storage.
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// Loads application settings from isolated storage.
        /// </summary>
        public abstract void Load();
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region PROTECTED METHODS
        /// <summary>
        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the value has been updated.</returns>
        protected bool AddOrUpdateValue(string key, Object value)
        {
            bool valueChanged = false;

            try
            {
                // if new value is different, set the new value.
                if (this.isolatedStore[key] != value)
                {
                    this.isolatedStore[key] = value;
                    valueChanged = true;
                } // if
            }
            catch (KeyNotFoundException)
            {
                this.isolatedStore.Add(key, value);
                valueChanged = true;
            }
            catch (ArgumentException)
            {
                this.isolatedStore.Add(key, value);
                valueChanged = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while using IsolatedStorageSettings: {0}", e);
            } // catch

            return valueChanged;
        } // AddOrUpdateValue()

        /// <summary>
        /// Get the current value of the setting, or if it is not found, set the
        /// setting to the default setting.
        /// </summary>
        /// <typeparam name="TValueType">The type of the value type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>A result.</returns>
        protected TValueType GetValueOrDefault<TValueType>(string key,
          TValueType defaultValue)
        {
            TValueType value;

            try
            {
                value = (TValueType)this.isolatedStore[key];
            }
            catch (KeyNotFoundException)
            {
                value = defaultValue;
            }
            catch (ArgumentException)
            {
                value = defaultValue;
            } // catch

            return value;
        } // GetValueOrDefault()
        #endregion // PROTECTED METHODS
    } // AppConfigBase
} // Tethys.Silverlight.Config
