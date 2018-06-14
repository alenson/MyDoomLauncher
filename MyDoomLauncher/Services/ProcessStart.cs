using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows;

namespace MyDoomLauncher.Services
{
    static class ProcessStart
    {
        public static void StartProcess(string target)
        {
            string exeFile = ConfigurationManager.AppSettings.Get("Executable");

            try
            {
                Process.Start(exeFile, target);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed to run DOOM port.\n" + exception.Message, "Critical error!", MessageBoxButton.OK);
                return;
            }
        }
    }
}
