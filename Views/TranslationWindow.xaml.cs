using System.Windows;
using System.Windows.Input;

namespace ScreTran;

/// <summary>
/// Interaction logic for TranslationWindow.xaml
/// </summary>
public partial class TranslationWindow : Window
{
    public TranslationWindow()
    {
        InitializeComponent();

        this.DataContext = App.GetService<TranslationViewModel>();
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        this.DragMove();
    }
}
