using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Win32ThemeStudio.Themes;

namespace Win32ThemeStudio.Demo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        ThemeComboBox.ItemsSource = ThemeManager.AvailableThemes.Keys;
        ThemeComboBox.SelectedItem = "Aurora Light";

        Opacity = OpacitySlider.Value;
        OpacityLabel.Text = $"{(int)(Opacity * 100)}%";
    }

    private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ThemeComboBox.SelectedItem is not string selectedTheme)
        {
            return;
        }

        ThemeManager.ApplyTheme(selectedTheme);
        if (TransparentToggle.IsChecked != true)
        {
            Background = (Brush)Application.Current.Resources["Brush.WindowGlass"];
        }
    }

    private void TransparentToggle_Changed(object sender, RoutedEventArgs e)
    {
        if (TransparentToggle.IsChecked == true)
        {
            Background = Brushes.Transparent;
            return;
        }

        Background = (Brush)Application.Current.Resources["Brush.WindowGlass"];
    }

    private void OpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        Opacity = OpacitySlider.Value;
        if (OpacityLabel is not null)
        {
            OpacityLabel.Text = $"{(int)(Opacity * 100)}%";
        }
    }
}