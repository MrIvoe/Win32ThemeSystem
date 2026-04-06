using System.Windows;
using Win32ThemeStudio.Themes;

namespace Win32ThemeStudio.Demo;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	protected override void OnStartup(StartupEventArgs e)
	{
		ThemeManager.InitializeApplicationTheme(this, ThemeCatalog.DefaultLightTheme);
		base.OnStartup(e);
	}
}

