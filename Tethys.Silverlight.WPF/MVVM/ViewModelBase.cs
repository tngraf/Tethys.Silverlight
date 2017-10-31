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
// <copyright file="ViewModelBase.cs" company="Tethys">
// Copyright  2010-2016 by Thomas Graf
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
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System.Windows;

    /// <summary>
    /// A base class for view models.
    /// </summary>
    [DataContract]
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// The _is in design mode
        /// </summary>
        private static bool? isInDesignMode;
        #endregion PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        /// <summary>
        /// This event is raised when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a value indicating whether the control is in design mode.
        /// </summary>
        [SuppressMessage(
          "Microsoft.Performance", "CA1822:MarkMembersAsStatic",
          Justification = "This is needed for data binding")]
        [Obsolete("Not to be used any longer", true)]
        public bool IsInDesignMode
        {
            get
            {
                return IsInDesignModeStatic;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control is in design mode.
        /// </summary>
        [SuppressMessage(
          "Microsoft.Security",
          "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands",
          Justification = "This is still the best solution.")]
        [Obsolete("Not to be used any longer", true)]
        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!isInDesignMode.HasValue)
                {
#if SILVERLIGHT || SILVERLIGHT3 || WINDOWS_PHONE
                    isInDesignMode = DesignerProperties.IsInDesignTool;
#else
#if NETFX_CORE || UNIVERSAL_APP81 || WINDOWS_UWP
                    isInDesignMode = Windows.ApplicationModel.DesignMode.DesignModeEnabled;
#else
                    var prop = DesignerProperties.IsInDesignModeProperty;
                    isInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
#endif
#endif
                } // if

                return isInDesignMode.Value;
            }
        } // IsInDesignModeStatic
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region PROTECTED METHODS
        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The property name of the property that has
        /// changed.</param>
#if NET45 || UNIVERSAL_APP81 || WINDOWS_UWP
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
#else
        protected virtual void RaisePropertyChanged(string propertyName)
#endif
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            } // if
        } // RaisePropertyChanged()

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The property name of the property that has
        /// changed.</param>
        /// <remarks>Exists due to legacy reasons.</remarks>
#if NET45 || UNIVERSAL_APP81 || WINDOWS_UWP
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
#else
        protected virtual void OnPropertyChanged(string propertyName)
#endif
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            } // if
        } // OnPropertyChanged()

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The
        /// <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance
        /// containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            } // if
        } // OnPropertyChanged()
        #endregion // PROTECTED METHODS
    } // ViewModelBase
} // Tethys.Silverlight.MVVM