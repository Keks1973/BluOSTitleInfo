using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

class BluOsMonitor
{
    // Tauschen Sie diese IP gegen die IP Ihres NAD M10 V2 aus
    private static readonly string DeviceIp = "192.168.0.7";
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        string currentEtag = "";
        Console.WriteLine("BluOS Liedwechsel-Monitor gestartet...");

        string title = string.Empty;

        while (true)
        {
            try
            {
                // URL für das Long Polling zusammenbauen (Timeout: 30 Sekunden)
                string url = $"http://{DeviceIp}:11000/Status?timeout=30";
                if (!string.IsNullOrEmpty(currentEtag))
                {
                    url += $"&etag={currentEtag}";
                }

                // API aufrufen und Antwort als String laden
                string xmlResponse = await client.GetStringAsync(url);
                XDocument doc = XDocument.Parse(xmlResponse);
                XElement root = doc.Root;

                // Neuen ETag auslesen
                string newEtag = root.Attribute("etag")?.Value ?? "";

                // Wenn sich der ETag geändert hat, gab es ein Event (z.B. Liedwechsel)
                if (newEtag != currentEtag)
                {
                    currentEtag = newEtag;

                    // Metadaten extrahieren
                    string artist = root.Element("artist")?.Value ?? "Unbekannt";
                    string actualTitle = root.Element("title1")?.Value ?? "";
                    string album = root.Element("album")?.Value ?? "Unbekannt";
                    string service = root.Element("service")?.Value ?? "Unbekannt";
                    string quality = root.Element("quality")?.Value ?? "Keine Info";

                    if ((string.IsNullOrEmpty(title) || title != actualTitle) && string.IsNullOrEmpty(actualTitle) == false)
                    {
                        title = new String(actualTitle);

                        // Ausgabe in der Konsole
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n[LIEDWECHSEL] {DateTime.Now.ToLongTimeString()}");
                        Console.ResetColor();
                        Console.WriteLine($"Titel:  {actualTitle}");
                        Console.WriteLine($"Artist: {artist}");
                        Console.WriteLine($"Album:  {album}");
                        Console.WriteLine($"Service:  {service}");
                        Console.WriteLine($"Format: {quality.ToUpper()}");
                    }

                    await Task.Delay(100);
                }
            }
            catch (HttpRequestException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Fehler: NAD M10 V2 nicht erreichbar. Versuche es in 5 Sekunden erneut...");
                Console.ResetColor();
                await Task.Delay(5000); // Kurz warten vor dem nächsten Versuch
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unerwarteter Fehler: {ex.Message}");
                await Task.Delay(2000);
            }
        }
    }
}
