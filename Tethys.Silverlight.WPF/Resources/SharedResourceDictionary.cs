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
// <copyright file="SharedResourceDictionary.cs" company="Tethys">
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

namespace Tethys.Silverlight.Resources
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;

    /// <summary>
    /// Provides a resource dictionary that caches the initialization of 
    /// resource dictionaries.
    /// </summary>
    public class SharedResourceDictionary : ResourceDictionary
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// Local member of the source uri.
        /// </summary>
        private Uri sourceUri;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets or sets the internal cache of loaded dictionaries.
        /// </summary>
        public static Dictionary<Uri, ResourceDictionary> SharedDictionaries { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is in design mode.
        /// </summary>
        public bool IsInDesignMode
        {
            get
            {
                return (bool)DependencyPropertyDescriptor.FromProperty(
                    DesignerProperties.IsInDesignModeProperty, 
                    typeof(DependencyObject)).Metadata.DefaultValue;
            }
        }

        /// <summary>Gets or sets the uniform resource identifier (URI) to 
        /// load resources from.</summary>
        /// <value>Source of ResourceDictionary.</value>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Ok here.")]
        public new Uri Source
        {
            get
            {
                return this.IsInDesignMode ? base.Source : this.sourceUri;
            }

            set
            {
                if (this.IsInDesignMode)
                {
                    try
                    {
                        base.Source = new Uri(value.OriginalString);
                    }
                    // ReSharper disable once EmptyGeneralCatchClause
                    catch
                    {
                        // do nothing?
                    } // catch

                    return;
                } // if

                try
                {
                    this.sourceUri = new Uri(value.OriginalString);
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                    // do nothing?
                } // catch

                if (!SharedDictionaries.ContainsKey(value))
                {
                    // If the dictionary is not yet loaded, load it by setting
                    // the source of the base class
                    base.Source = value;

                    // add it to the cache
                    SharedDictionaries.Add(value, this);
                }
                else
                {
                    // If the dictionary is already loaded, get it from the cache
                    this.MergedDictionaries.Add(SharedDictionaries[value]);
                } // if
            } // set
        } // Source
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes static members of the <see cref="SharedResourceDictionary"/> class.
        /// </summary>
        static SharedResourceDictionary()
        {
            SharedDictionaries = new Dictionary<Uri, ResourceDictionary>();
        } // SharedResourceDictionary()
        #endregion // CONSTRUCTION
    } // SharedResourceDictionary
} // Tethys.Silverlight.Resources