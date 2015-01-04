using System;
using System.Windows;
using System.Windows.Threading;
using TgLib.Diagnostics;

namespace WpfApplicationExceptionDialog
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
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
    static void OnApplicationException(object sender, 
      DispatcherUnhandledExceptionEventArgs e)
    {
      AppErrorWindow win = new AppErrorWindow(e.Exception);
      win.Owner = Current.MainWindow;
      win.ShowDialog();
      e.Handled = true;
    } // OnApplicationException()

    /// <summary>
    /// Called when an AppDomain exception is thrown.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.UnhandledExceptionEventArgs"/>
    ///  instance containing the event data.</param>
    static void OnAppDomainException(object sender, 
      UnhandledExceptionEventArgs e)
    {
      Exception ex = e.ExceptionObject as Exception;
      if (ex != null)
      {
        AppErrorWindow win = new AppErrorWindow(ex);
        win.Owner = Current.MainWindow;
        win.ShowDialog();
      } // if
    } // OnAppDomainException()
  } // App
} // WpfApplicationExceptionDialog
