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
// <copyright file="AssembliesHelper.cs" company="Tethys">
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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The AssembliesHelper helps gets assemblies and types used for brokering views and 
    /// view models etc, in a consistent way across platforms.
    /// </summary>
    public static class AssembliesHelper
    {
        /// <summary>
        /// Gets the domain assemblies.
        /// </summary>
        /// <returns>Assemblies in the domain.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Best solution here")]
        public static IEnumerable<Assembly> GetDomainAssemblies()
        {
#if SILVERLIGHT3 || WINDOWS_PHONE
      return new List<Assembly> {Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly()};
#elif SILVERLIGHT4
      //  TODO: According to MSDN AppDomain.CurrentDomain.GetAssemblies should compile in SL4 - it doesn't seem to.
      //  We can force it to work by making things dynamic.
      dynamic appDomain = AppDomain.CurrentDomain;
      return appDomain.GetAssemblies();
#else
            return AppDomain.CurrentDomain.GetAssemblies();
#endif
        } // GetDomainAssemblies()

        /// <summary>
        /// Gets the domain types.
        /// </summary>
        /// <returns>Domain types.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Best solution here")]
        public static IEnumerable<Type> GetTypesInDomain()
        {
#if SILVERLIGHT3
      var typesToSearch = (from a in GetDomainAssemblies()
                                from t in a.GetExportedTypes()
                                select t).ToList();
#elif SILVERLIGHT4
      var typesToSearch = (from a in GetDomainAssemblies()
                            where a.IsDynamic == false
                            from t in a.GetExportedTypes()
                            select t).ToList();
#elif SILVERLIGHT5
      var typesToSearch = (from a in GetDomainAssemblies()
                            where a.IsDynamic == false
                            from t in a.GetExportedTypes()
                            select t).ToList();
#elif SILVERLIGHT
      var typesToSearch = (from a in GetDomainAssemblies()
                            from t in a.GetExportedTypes()
                            select t).ToList();
#else
            var typesToSearch = (from a in GetDomainAssemblies()
                                 where a.GlobalAssemblyCache == false && a.IsDynamic == false
                                 from t in a.GetExportedTypes()
                                 select t).ToList();
#endif
            return typesToSearch.Distinct();
        } // GetTypesInDomain()
    } // AssembliesHelper
} // Tethys
