using System;
using System.Collections.Generic;
using System.Text;

namespace BluOSTitleInfo
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
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

    }
}
