using System.Windows;
using Win32ThemeStudio.Themes;

namespace Win32ThemeStudio.BootstrapperSample;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        ThemeManager.InitializeApplicationTheme(this, ThemeCatalog.DefaultDarkTheme);
        base.OnStartup(e);
    }
}