using Microsoft.Win32;
using System;

namespace MyDoomLauncher.Services
{
    public class ThemeProvider
    {
        private const string RegistryPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        private const string RegistryKey = "AppsUseLightTheme";

        public static bool IsWindowsDarkModeEnabled()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(RegistryPath))
                {
                    if (key == null)
                        return false;

                    var value = key.GetValue(RegistryKey);
                    if (value is int intValue)
                    {
                        return intValue == 0;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }
    }
}
