using System;
using System.Diagnostics;
using System.Windows;

namespace MyDoomLauncher.Services
{
    static class ProcessStart
    {
        public static void StartProcess(string parameters)
        {
            string executableFile = ConfigurationProvider.GetValueForCurrentConfiguration("Executable");

            try
            {
                Process.Start(executableFile, parameters);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed to run DOOM port.\n" + exception.Message, "Critical error!", MessageBoxButton.OK);
                return;
            }
        }
    }
}
