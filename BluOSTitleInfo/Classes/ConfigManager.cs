using System;
using System.Collections.Generic;
using System.Text;

namespace BluOSTitleInfo.Classes
{
    internal static class ConfigManager
    {
        /// <summary>
        /// Gets the value of the specified key from the appSettings section of the configuration file.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static string GetAppSettingsValue(string key)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Lesen der Konfiguration: {ex.Message}");

                return null;
            }
        }

    }
}
