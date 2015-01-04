using System.Windows;

namespace WpfLayoutDemo
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void BtnGeneralDialogLayout_Click(object sender, RoutedEventArgs e)
    {
      DialogLayout dlg = new DialogLayout();
      dlg.ShowDialog();
    }

    private void BtnSimpleDialog_Click(object sender, RoutedEventArgs e)
    {
      MessageDialog dlg = new MessageDialog();
      dlg.ShowDialog();
    }

    private void BtnMessageDialogWin7_Click(object sender, RoutedEventArgs e)
    {
      MessageDialogWin7 dlg = new MessageDialogWin7();
      dlg.ShowDialog();
    }

    private void BtnWindowsMessageBox_Click(object sender, RoutedEventArgs e)
    {
      MessageBox.Show("Some message text. This may also be a much longer text with multiple lines that are automatically wrapped.",
        "A caption", MessageBoxButton.YesNoCancel,
        MessageBoxImage.Information);
    }
  }
}
