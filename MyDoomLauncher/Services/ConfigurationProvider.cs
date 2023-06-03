using System.Collections.Specialized;
using System.Configuration;

namespace MyDoomLauncher.Services
{
    public static class ConfigurationProvider
    {
        public static string GetValueForCurrentConfiguration(string key)
        {
            string configurationName = ConfigurationManager.AppSettings.Get("SelectedConfiguration");
            NameValueCollection configuration = ConfigurationManager.GetSection(configurationName) as NameValueCollection;
            return configuration.Get(key);
        }

        public static string GetCurrentConfigurationName()
        {
            return ConfigurationManager.AppSettings.Get("SelectedConfiguration");
        }

        public static string[] GetKnownInternalWads()
        {
            return ConfigurationManager.AppSettings.Get("KnownInternalWads").Split(';');
        }
    }
}
