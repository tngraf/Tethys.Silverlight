using System.Windows;

namespace WpfApplicationExceptionDialog
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

    private void BtnThrowException(object sender, RoutedEventArgs e)
    {
      int x = 0;
      int a = 1/x;
    }
  }
}
