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
// <copyright file="ObservableCollectionExtensions.cs" company="Tethys">
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

namespace Tethys.Silverlight.Extensions
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Extension methods for ObservableCollection.
    /// </summary>
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// Inserts items sorted by the comparison method.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item.</param>
        /// <param name="comparison">The comparison.</param>
        public static void InsertSorted<T>(this ObservableCollection<T> collection, 
            T item, Comparison<T> comparison)
        {
            if (collection.Count == 0)
            {
                collection.Add(item);
            }
            else
            {
                bool last = true;
                for (int i = 0; i < collection.Count; i++)
                {
                    int result = comparison.Invoke(collection[i], item);
                    if (result >= 1)
                    {
                        collection.Insert(i, item);
                        last = false;
                        break;
                    } // if
                } // for
                if (last)
                {
                    collection.Add(item);
                } // if
            } // if
        } // InsertSorted<T>()
    } // ObservableCollectionExtensions
} // Tethys.Silverlight.Extensions
