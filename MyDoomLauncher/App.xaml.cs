using MyDoomLauncher.Services;
using System.Windows;

namespace MyDoomLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            InitializeTheme();
            base.OnStartup(e);
        }

        private void InitializeTheme()
        {
            bool isDarkMode = ThemeProvider.IsWindowsDarkModeEnabled();
            ApplyTheme(isDarkMode);
        }

        private void ApplyTheme(bool isDarkMode)
        {
            var resources = this.Resources;

            if (isDarkMode)
            {
                // Dark theme
                resources["WindowBackground"] = resources["DarkBackground"];
                resources["WindowForeground"] = resources["DarkForeground"];
                resources["ControlBackground"] = resources["DarkBackgroundSecondary"];
                resources["ControlForeground"] = resources["DarkForeground"];
                resources["BorderBrush"] = resources["DarkBorder"];
                resources["ButtonBackground"] = resources["DarkButtonBackground"];
                resources["ButtonHoverBackground"] = resources["DarkButtonHover"];
                resources["GridViewSeparator"] = resources["DarkGridViewSeparator"];
            }
            else
            {
                // Light theme
                resources["WindowBackground"] = resources["LightBackground"];
                resources["WindowForeground"] = resources["LightForeground"];
                resources["ControlBackground"] = resources["LightBackgroundSecondary"];
                resources["ControlForeground"] = resources["LightForeground"];
                resources["BorderBrush"] = resources["LightBorder"];
                resources["ButtonBackground"] = resources["LightButtonBackground"];
                resources["ButtonHoverBackground"] = resources["LightButtonHover"];
                resources["GridViewSeparator"] = resources["LightGridViewSeparator"];
            }
        }
    }
}
