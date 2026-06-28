using System;
using System.Collections.Generic;
using System.Text;

namespace BluOSTitleInfo.Classes
{
    internal static class NetworkManager
    {
        /// <summary>
        /// Liest die IP-Adresse des BluOS-Geräts entweder aus den Kommandozeilenargumenten oder aus der Konfigurationsdatei, abhängig von der Einstellung "DeviceIpViaCommandLine".
        /// </summary>
        /// <param name="CommandLineArgs"></param>
        /// <returns></returns>
        internal static string GetDeviceIp(string[] CommandLineArgs)
        {
            string deviceIp = string.Empty;

            string settingIPAdressViaCommandLine = ConfigManager.GetAppSettingsValue("DeviceIpViaCommandLine");

            if (settingIPAdressViaCommandLine == "true")
            {
                //IP-Adresse des BluOS-Geräts aus den Kommandozeilenargumenten lesen
                if (CommandLineArgs.Length == 0)
                {
                    Console.WriteLine("Fehler: Bitte geben Sie die IP-Adresse des BluOS-Geräts als Argument an.");
                    
                    return string.Empty;
                }

                deviceIp = CommandLineArgs[0];

                if (System.Net.IPAddress.IsValid(deviceIp))
                {
                    //IP-Adresse ist gültig, Ausgabe in der Konsole
                    Console.WriteLine("\nIP-Adresse des BluOS-Geräts (aus Kommandozeile): " + deviceIp);
                    
                    return deviceIp;
                }
                else
                {
                    //IP-Adresse ist ungültig, Fehlermeldung ausgeben
                    Console.WriteLine("Fehler: Die angegebene IP-Adresse ist keine gültige IP-Adresse.");
                    
                    return string.Empty;
                }
            }
            else
            {
                //IP-Adresse des BluOS-Geräts aus der Konfigurationsdatei lesen
                deviceIp = ConfigManager.GetAppSettingsValue("DeviceIp");

                if (string.IsNullOrEmpty(deviceIp))
                {
                    //Fehlermeldung ausgeben, wenn die IP-Adresse nicht in der Konfigurationsdatei angegeben ist
                    Console.WriteLine("Fehler: DeviceIp ist nicht in der Konfigurationsdatei angegeben.");

                    return string.Empty;
                }
                else
                {
                    if (System.Net.IPAddress.IsValid(deviceIp))
                    {
                        //IP-Adresse ist gültig, Ausgabe in der Konsole
                        Console.WriteLine("\nIP-Adresse des BluOS-Geräts: " + deviceIp);

                        return deviceIp;
                    }
                    else
                    {
                        //IP-Adresse ist ungültig, Fehlermeldung ausgeben
                        Console.WriteLine("Fehler: deviceIp ist keine gültige IP-Adresse.");

                        return string.Empty;
                    }
                }
            }
        }
    }
}
