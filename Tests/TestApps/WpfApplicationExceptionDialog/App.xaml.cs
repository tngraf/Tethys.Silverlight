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
// <copyright file="App.xaml.cs" company="Tethys">
// Copyright  2010-2015 by Thomas Graf
//            All rights reserved.
//            Licensed under the Apache License, Version 2.0.
//            Unless required by applicable law or agreed to in writing, 
//            software distributed under the License is distributed on an
//            "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
//            either express or implied. 
// </copyright>
//
// ---------------------------------------------------------------------------
#endregion

namespace WpfApplicationExceptionDialog
{
    using System;
    using System.Windows.Threading;

    using Tethys.Silverlight.Diagnostics;

    /// <summary>
    /// Interaction logic for App.
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            DispatcherUnhandledException += OnApplicationException;
            AppDomain.CurrentDomain.UnhandledException += OnAppDomainException;
        } // App()

        /// <summary>
        /// Called when an application exception is thrown.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The
        ///  <see cref="System.Windows.Threading.DispatcherUnhandledExceptionEventArgs"/> 
        /// instance containing the event data.</param>
        private static void OnApplicationException(object sender,
          DispatcherUnhandledExceptionEventArgs e)
        {
            var win = new AppErrorWindow(e.Exception);
            win.Owner = Current.MainWindow;
            win.ShowDialog();
            e.Handled = true;
        } // OnApplicationException()

        /// <summary>
        /// Called when an AppDomain exception is thrown.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.UnhandledExceptionEventArgs"/>
        /// instance containing the event data.</param>
        private static void OnAppDomainException(object sender,
          UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                AppErrorWindow win = new AppErrorWindow(ex);
                win.Owner = Current.MainWindow;
                win.ShowDialog();
            } // if
        } // OnAppDomainException()
    } // App
} // WpfApplicationExceptionDialog
