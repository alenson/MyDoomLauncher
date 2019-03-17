using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows;

namespace MyDoomLauncher.Services
{
    static class ProcessStart
    {
        public static void StartProcess(string parameters)
        {
            string exeFile = ConfigurationManager.AppSettings.Get("Executable");

            try
            {
                Process.Start(exeFile, parameters);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed to run DOOM port.\n" + exception.Message, "Critical error!", MessageBoxButton.OK);
                return;
            }
        }
    }
}
