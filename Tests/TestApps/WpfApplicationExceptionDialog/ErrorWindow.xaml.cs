using System;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.InteropServices;

namespace Neumann.Diagnostics
{
  public partial class ErrorWindow : Window
  {
    #region Private Variables
    
    // Konstanten
    private const int DIALOG_HEIGHT_MAX = 548;
    private const int DIALOG_HEIGHT_MIN = 230;
    private readonly string STANDARD_TITLE_TEXT = "Anwendungsfehler";
    private readonly string STANDARD_MESSAGE = "In der Anwendung ist ein Fehler aufgetreten. Klicken Sie auf die Schaltfläche \"Erweitert\" für weitere Informationen.";
    
    // Allgemeine Variablen
    private Exception _exception;
    private bool _extended = true;
    private bool _initDialog = false;
    
    // Property-Variablen
    private ExceptionInformation _exceptionInfo;
    private bool _unhandledException = false;
    private bool _showExtendedButton = true;
    private bool _showExtended = false;
    private string _titleText = string.Empty;
    private string _reportErrorText = String.Empty;
    
    // Interop
    [DllImport("user32.dll")]
    private extern static int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    private extern static int GetWindowLong(IntPtr hWnd, int nIndex);
    private const int GWL_STYLE = -16;
    #endregion

    // ----------------------------------------------------------------------
    
    #region Constructors
    
    /// <summary>
    /// Erstellt eine neue Instanz der Klasse.
    /// </summary>
    public ErrorWindow()
    {
      _titleText = STANDARD_TITLE_TEXT;
      this.InitializeComponent();
            this.Loaded += (s, e) => this.Initialize();
        }
    
    /// <summary>
        /// Erstellt eine neue Instanz der Klasse mit einem ExceptionInfo-Objekt.
    /// </summary>
        /// <param name="exInfo">Ein ExceptionInfo-Objekt, mit Informationen über den aufgetretenen Fehler.</param>
    public ErrorWindow(ExceptionInformation exInfo) : this()
    {
      _exceptionInfo = exInfo;
    }

        /// <summary>
        /// Erstellt eine neue Instanz der Klasse mit der aufgetretenen Exception.
        /// </summary>
        /// <param name="ex">Die aufgetretene Exception.</param>
        public ErrorWindow(Exception ex)
            : this(new ExceptionInformation(ex))
        {
        }
    
    #endregion

    // ----------------------------------------------------------------------
    
    #region Public Properties
    
    /// <summary>
    /// Gibt das ExceptionInfo-Objekt an, das angezeigt werden soll.
    /// </summary>
    public ExceptionInformation ExceptionInfo 
    {
      get{return _exceptionInfo;}
      set{_exceptionInfo = value;}
    }
    
    /// <summary>
    /// Bestimmt, den Titeltext, der angezeigt wird.
    /// </summary>
    public string TitleText
    {
      get{return _titleText;}
      set{_titleText = value;}
    }
    
    /// <summary>
    /// Bestimmt, ob die aufgetretene Exception nicht abgefangen wurde.
    /// </summary>
    public bool UnhandledException
    {
      get{return _unhandledException;}
      set{_unhandledException = value;}
    }
    
    /// <summary>
    /// Bestimmt, ob die "Erweitert"-Schaltfläche angezeigt wird.
    /// </summary>
    public bool ShowExtendedButton
    {
      get{return _showExtendedButton;}
      set{_showExtendedButton = value;}
    }
    
    /// <summary>
    /// Bestimmt, ob der Dialog in erweiterter Form angezeigt wird.
    /// </summary>
    public bool ShowExtended
    {
      get{return _showExtended;}
      set{_showExtended = value;}
    }
    
    #endregion

    // ----------------------------------------------------------------------

    #region Public Functions

    /// <summary>
    /// Initialisiert den Dialog.
    /// </summary>
    public void Initialize()
    {
            // Hide minimize/maximize buttons
            this.RemoveControlBoxes();

            this.Height = DIALOG_HEIGHT_MIN;
            _initDialog = true;
            this.mainExpander.Visibility = _showExtendedButton ? Visibility.Visible : Visibility.Collapsed;
            this.titleTextBlock.Text = STANDARD_TITLE_TEXT;
            this.errorTextBlock.Text = STANDARD_MESSAGE;
      
      // Dialog ein-/ausklappen
      this.ToggleExtended(_showExtended);
      _extended = _showExtended;
      
      if (_exceptionInfo != null)
      {
        _exception = _exceptionInfo.Exception;
        
        // Exception-Informationen anzeigen
        this.ShowException();
        
        // InnerException-Tree füllen
        this.CreateInnerExceptionTree(_exceptionInfo.Exception);
        
        // ExceptionInfo-Informationen anzeigen
        this.assemblyTextBox.Text			= _exceptionInfo.AssemblyName;
        this.appDomainTextBox.Text			= _exceptionInfo.AppDomainName;
        this.threadIdTextBox.Text			= _exceptionInfo.ThreadId.ToString();
        this.threadUserTextBox.Text			= _exceptionInfo.ThreadUser;
        this.memoryTextBox.Text			    = _exceptionInfo.WorkingSet.ToString();
        this.titleTextBox.Text		        = _exceptionInfo.ProductName;
        this.versionTextBox.Text		    = _exceptionInfo.ProductVersion;
        this.startupDirectoryTextBox.Text	= _exceptionInfo.ExecutablePath;
        this.osVersionTextBox.Text	        = _exceptionInfo.OperatingSystem.ToString();
        this.frameworkVersionTextBox.Text	= _exceptionInfo.FrameworkVersion.ToString();
      }
      else
        MessageBox.Show("Die übergebene ExceptionInfo-Objekt ist ungültig!", 
          "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
      _initDialog = false;
    }

    #endregion

    // ----------------------------------------------------------------------

    #region Private Functions

        private void RemoveControlBoxes()
        {
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            long windowLong = GetWindowLong(hwnd, GWL_STYLE);
            windowLong = windowLong & -131073 & -65537;
            SetWindowLong(hwnd, GWL_STYLE, Convert.ToInt32(windowLong));
        }
    
    /// <summary>
    /// Zeigt die Bestandteile der übergebenen Exception im Fenster an.
    /// </summary>
    private void ShowException()
    {
      if (_exception != null)
      {
                if (_exception.Message.Length > 0)
                    this.errorTextBlock.Text = STANDARD_MESSAGE + Environment.NewLine + Environment.NewLine + _exception.Message;
                else
                    this.errorTextBlock.Text = STANDARD_MESSAGE;
        
        // Zusatz-Informationen aus dem StackTrace der Exception ermitteln
        this.classTextBox.Text		= _exceptionInfo.GetClassName(_exception);
        this.methodTextBox.Text		= _exceptionInfo.GetMethodName(_exception);
        this.sourceFileTextBox.Text	= _exceptionInfo.GetFileName(_exception);
        this.stackTraceTextBox.Text	= _exceptionInfo.GetStackTrace(_exception);
        this.rowColumnTextBox.Text	= _exceptionInfo.GetFileLineNumber(_exception).ToString() + "/" + _exceptionInfo.GetFileColumnNumber(_exception).ToString();
        this.ilOffsetTextBox.Text	= _exceptionInfo.GetILOffset(_exception).ToString() + "/" + _exceptionInfo.GetNativeOffset(_exception).ToString();
        
        // Exception an PropertyGrid binden
                this.extendedPropertyGrid.SelectedObject = _exception;
      }
      else
        MessageBox.Show("Die übergebene Exception ist ungültig!",
                    "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
    }
    
    /// <summary>
    /// Füllt den InnerException-Treeview.
    /// </summary>
    /// <param name="ex"></param>
    private void CreateInnerExceptionTree(Exception ex)
    {
      this.exceptionTreeView.Items.Clear();
            TreeViewItem rootNode = new TreeViewItem()
                {
                    Header = ex.GetType().ToString(),
                    Tag = ex
                };
      this.exceptionTreeView.Items.Add(rootNode);
      TreeViewItem parentNode = rootNode;
      Exception currentException = ex;
      while (currentException.InnerException != null)
      {
        currentException = currentException.InnerException;
                TreeViewItem currentNode = new TreeViewItem
                {
                    Header = currentException.GetType().ToString(),
                    Tag = currentException
                };
        parentNode.Items.Add(currentNode);
        parentNode = currentNode;
      }
      rootNode.IsExpanded = true;
            rootNode.IsSelected = true;
        }
    
    /// <summary>
    /// Erweitert/Verringert den Dialog.
    /// </summary>
    private void ToggleExtended(bool extended)
    {
      if (extended)
      {
                this.MaxWidth = double.PositiveInfinity;
                this.MaxHeight = double.PositiveInfinity;
        this.Height = DIALOG_HEIGHT_MAX;
      }
      else
      {
                this.MaxWidth = this.Width;
                this.MaxHeight = DIALOG_HEIGHT_MIN;
                this.Height = DIALOG_HEIGHT_MIN;
      }
      
      if (extended)
        this.exceptionTreeView.Focus();
    }
    
    /// <summary>
    /// Erweitert/Verringert den Dialog.
    /// </summary>
    private void ToggleExtended()
    {
      _extended = !_extended;
      this.ToggleExtended(_extended);
    }
    
    #endregion

    // ----------------------------------------------------------------------

    #region Event Handling

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            this.ToggleExtended(true);
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            this.ToggleExtended(false);
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void exceptionTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue != null && _initDialog == false)
            {
                _exception = ((TreeViewItem)e.NewValue).Tag as Exception;
                this.ShowException();
            }
        }
    
    #endregion

    }
}