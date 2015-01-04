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
// <copyright file="AppErrorWindow.xaml.cs" company="Tethys">
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

namespace Tethys.Silverlight.Diagnostics
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;

    using Tethys.Silverlight.Helper;

    /// <summary>
    /// Interaction logic for AppErrorWindow.
    /// </summary>
    public partial class AppErrorWindow
    {
        #region Private Variables
        /// <summary>
        /// The dialog height maximum
        /// </summary>
        private const int DialogHeightMax = 548;

        /// <summary>
        /// The dialog height minimum.
        /// </summary>
        private const int DialogHeightMin = 230;

        /// <summary>
        /// The standard title text.
        /// </summary>
        private const string StandardTitleText = "Application Error";

        /// <summary>
        /// The standard message.
        /// </summary>
        private const string StandardMessage = "An error occured in the application. Click on \"Expand\" for more informationen.";

        /// <summary>
        /// The exception.
        /// </summary>
        private Exception exception;

        /// <summary>
        /// The initialize dialog flag.
        /// </summary>
        private bool initDialog;

        /// <summary>
        /// The exception information.
        /// </summary>
        private ExceptionInformation exceptionInfo;

        /// <summary>
        /// The show extended button flag.
        /// </summary>
        private bool showExtendedButton = true;

        /// <summary>
        /// The show extended flag.
        /// </summary>
        private bool showExtended;

        /// <summary>
        /// The title text.
        /// </summary>
        private string titleText = string.Empty;
        #endregion

        //// ----------------------------------------------------------------------

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AppErrorWindow"/> class.
        /// </summary>
        public AppErrorWindow()
        {
            this.titleText = StandardTitleText;
            this.InitializeComponent();
            this.Loaded += (s, e) => this.Initialize();
        } // AppErrorWindow()

        /// <summary>
        /// Initializes a new instance of the <see cref="AppErrorWindow"/> class.
        /// </summary>
        /// <param name="exceptionInfo">An <see cref="ExceptionInformation"/> object
        /// with information about the problem.</param>
        public AppErrorWindow(ExceptionInformation exceptionInfo)
            : this()
        {
            this.exceptionInfo = exceptionInfo;
        } // AppErrorWindow()

        /// <summary>
        /// Initializes a new instance of the <see cref="AppErrorWindow"/> class.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public AppErrorWindow(Exception ex)
            : this(new ExceptionInformation(ex))
        {
        } // AppErrorWindow()
        #endregion

        //// ----------------------------------------------------------------------

        #region Public Properties
        /// <summary>
        /// Gets or sets the exception information.
        /// </summary>
        public ExceptionInformation ExceptionInfo
        {
            get { return this.exceptionInfo; }
            set { this.exceptionInfo = value; }
        }

        /// <summary>
        /// Gets or sets the title text.
        /// </summary>
        public string TitleText
        {
            get { return this.titleText; }
            set { this.titleText = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the exception was unhandled.
        /// </summary>
        public bool UnhandledException { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the 'extended' button.
        /// </summary>
        public bool ShowExtendedButton
        {
            get { return this.showExtendedButton; }
            set { this.showExtendedButton = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the extended information.
        /// </summary>
        public bool ShowExtended
        {
            get { return this.showExtended; }
            set { this.showExtended = value; }
        }

        #endregion

        //// ----------------------------------------------------------------------

        #region Public Functions
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            // Hide minimize/maximize buttons
            this.RemoveControlBoxes();

            this.Height = DialogHeightMin;
            this.initDialog = true;
            this.mainExpander.Visibility = this.showExtendedButton ? Visibility.Visible : Visibility.Collapsed;
            this.titleTextBlock.Text = StandardTitleText;
            this.errorTextBlock.Text = StandardMessage;

            // Dialog ein-/ausklappen
            this.ToggleExtended(this.showExtended);

            if (this.exceptionInfo != null)
            {
                this.exception = this.exceptionInfo.Exception;

                // Exception-Informationen anzeigen
                this.ShowException();

                // InnerException-Tree füllen
                this.CreateInnerExceptionTree(this.exceptionInfo.Exception);

                // ExceptionInfo-Informationen anzeigen
                this.assemblyTextBox.Text = this.exceptionInfo.AssemblyName;
                this.appDomainTextBox.Text = this.exceptionInfo.AppDomainName;
                this.threadIdTextBox.Text = this.exceptionInfo.ThreadId.ToString(CultureInfo.InvariantCulture);
                this.threadUserTextBox.Text = this.exceptionInfo.ThreadUser;
                this.memoryTextBox.Text = this.exceptionInfo.WorkingSet.ToString(CultureInfo.InvariantCulture);
                this.titleTextBox.Text = this.exceptionInfo.ProductName;
                this.versionTextBox.Text = this.exceptionInfo.ProductVersion;
                this.startupDirectoryTextBox.Text = this.exceptionInfo.ExecutablePath;
                this.osVersionTextBox.Text = this.exceptionInfo.OperatingSystem.ToString();
                this.frameworkVersionTextBox.Text = this.exceptionInfo.FrameworkVersion.ToString();
            }
            else
            {
                MessageBox.Show("Die übergebene ExceptionInfo-Objekt ist ungültig!",
                  "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            } // if
            this.initDialog = false;
        } // Initialize()

        #endregion

        //// ----------------------------------------------------------------------

        #region Private Functions
        /// <summary>
        /// Removes the control boxes from the window.
        /// </summary>
        private void RemoveControlBoxes()
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            long windowLong = NativeMethods.GetWindowLong(hwnd, NativeMethods.Style);
            windowLong = windowLong & -131073 & -65537;
            NativeMethods.SetWindowLong(hwnd, NativeMethods.Style, Convert.ToInt32(windowLong));
        } // RemoveControlBoxes()

        /// <summary>
        /// Show the parts of the given exception in a window.
        /// </summary>
        private void ShowException()
        {
            if (this.exception != null)
            {
                if (this.exception.Message.Length > 0)
                {
                    this.errorTextBlock.Text = StandardMessage + Environment.NewLine + Environment.NewLine
                                               + this.exception.Message;
                }
                else
                {
                    this.errorTextBlock.Text = StandardMessage;
                } // if

                // Zusatz-Informationen aus dem StackTrace der Exception ermitteln
                this.classTextBox.Text = ExceptionInformation.GetClassName(this.exception);
                this.methodTextBox.Text = ExceptionInformation.GetMethodName(this.exception);
                this.sourceFileTextBox.Text = ExceptionInformation.GetFileName(this.exception);
                this.stackTraceTextBox.Text = ExceptionInformation.GetStackTrace(this.exception);
                this.rowColumnTextBox.Text = ExceptionInformation.GetFileLineNumber(this.exception)
                    + "/" + ExceptionInformation.GetFileColumnNumber(this.exception);
                this.ilOffsetTextBox.Text = ExceptionInformation.GetIlOffset(this.exception) 
                    + "/" + ExceptionInformation.GetNativeOffset(this.exception);

                // Exception an PropertyGrid binden
                this.extendedPropertyGrid.SelectedObject = this.exception;
            }
            else
            {
                MessageBox.Show("Die übergebene Exception ist ungültig!",
                            "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            } // if
        } // ShowException()

        /// <summary>
        /// Creates the inner exception tree.
        /// </summary>
        /// <param name="ex">The exception.</param>
        private void CreateInnerExceptionTree(Exception ex)
        {
            this.exceptionTreeView.Items.Clear();
            var rootNode = new TreeViewItem
                                      {
                                          Header = ex.GetType().ToString(),
                                          Tag = ex
                                      };

            this.exceptionTreeView.Items.Add(rootNode);
            var parentNode = rootNode;
            var currentException = ex;
            while (currentException.InnerException != null)
            {
                currentException = currentException.InnerException;
                var currentNode = new TreeViewItem
                                             {
                                                 Header = currentException.GetType().ToString(),
                                                 Tag = currentException
                                             };

                parentNode.Items.Add(currentNode);
                parentNode = currentNode;
            } // while

            rootNode.IsExpanded = true;
            rootNode.IsSelected = true;
        }

        /// <summary>
        /// Extends or shrinks the dialog.
        /// </summary>
        /// <param name="extended">if set to <c>true</c> extend the dialog.</param>
        private void ToggleExtended(bool extended)
        {
            if (extended)
            {
                this.MaxWidth = double.PositiveInfinity;
                this.MaxHeight = double.PositiveInfinity;
                this.Height = DialogHeightMax;
            }
            else
            {
                this.MaxWidth = this.Width;
                this.MaxHeight = DialogHeightMin;
                this.Height = DialogHeightMin;
            }

            if (extended)
            {
                this.exceptionTreeView.Focus();
            } // if
        } // ToggleExtended()
        #endregion

        //// ----------------------------------------------------------------------

        #region Event Handling
        /// <summary>
        /// Handles the Expanded event of the Expander control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance
        /// containing the event data.</param>
        private void ExpanderExpanded(object sender, RoutedEventArgs e)
        {
            this.ToggleExtended(true);
        } // ExpanderExpanded()

        /// <summary>
        /// Handles the Collapsed event of the Expander control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance
        /// containing the event data.</param>
        private void ExpanderCollapsed(object sender, RoutedEventArgs e)
        {
            this.ToggleExtended(false);
        } // ExpanderCollapsed()

        /// <summary>
        /// Handles the click on the close button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance
        /// containing the event data.</param>
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Is called when the selected TreeView item has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void ExceptionTreeViewSelectedItemChanged(object sender,
          RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue != null && this.initDialog == false)
            {
                this.exception = ((TreeViewItem)e.NewValue).Tag as Exception;
                this.ShowException();
            } // if
        }

        #endregion
    } // AppErrorWindow
} // Tethys.Silverlight.Diagnostics
