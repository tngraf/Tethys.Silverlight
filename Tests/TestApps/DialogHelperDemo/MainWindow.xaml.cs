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
// <copyright file="MainWindow.xaml.cs" company="Tethys">
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

namespace WpfLayoutDemo
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        } // MainWindow

        /// <summary>
        /// Handles the Click event of the <c>BtnGeneralDialogLayout</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance
        /// containing the event data.</param>
        private void BtnGeneralDialogLayoutClick(object sender, RoutedEventArgs e)
        {
            var dlg = new DialogLayout();
            dlg.ShowDialog();
        }

        /// <summary>
        /// Handles the Click event of the <c>BtnSimpleDialog</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance
        /// containing the event data.</param>
        private void BtnSimpleDialogClick(object sender, RoutedEventArgs e)
        {
            var dlg = new MessageDialog();
            dlg.ShowDialog();
        }

        /// <summary>
        /// Handles the Click event of the <c>BtnMessageDialogWin7</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance
        /// containing the event data.</param>
        private void BtnMessageDialogWin7Click(object sender, RoutedEventArgs e)
        {
            var dlg = new MessageDialogWin7();
            dlg.ShowDialog();
        }

        /// <summary>
        /// Handles the Click event of the <c>BtnWindowsMessageBox</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance
        /// containing the event data.</param>
        private void BtnWindowsMessageBoxClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Some message text. This may also be a much longer text with multiple lines that are automatically wrapped.",
              "A caption", MessageBoxButton.YesNoCancel,
              MessageBoxImage.Information);
        }
    } // MainWindows
} // WpfLayoutDemo
