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
// <copyright file="DesignTime.cs" company="Tethys">
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

namespace Tethys.Silverlight.MVVM
{
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;

    public static class DesignTime
    {
        /// <summary>
        /// Gets a value indicating whether the control is in design mode.
        /// </summary>
        [SuppressMessage(
          "Microsoft.Security",
          "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands",
          Justification = "This is still the best solution.")]
        public static bool IsInDesignMode
        {
            get
            {
#if SILVERLIGHT || SILVERLIGHT3 || WINDOWS_PHONE
                return DesignerProperties.IsInDesignTool;
#else
  #if NETFX_CORE
                return Windows.ApplicationModel.DesignMode.DesignModeEnabled;
  #else
                var prop = DesignerProperties.IsInDesignModeProperty;
                return  (bool)DependencyPropertyDescriptor
                    .FromProperty(prop, typeof(FrameworkElement))
                    .Metadata.DefaultValue;
  #endif
#endif
            }
        } // IsInDesignModeStatic
    } // DesignTime
} // Tethys.Silverlight.MVVM
