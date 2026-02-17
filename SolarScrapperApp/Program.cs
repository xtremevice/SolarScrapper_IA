using SolarScrapperApp.Config;
using SolarScrapperApp.Models;
using SolarScrapperApp.Scrapers;
using SolarScrapperApp.Services;

namespace SolarScrapperApp;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        PrintBanner();
        
        Console.WriteLine("🔍 Iniciando búsqueda de equipos solares en el mercado mexicano...\n");
        
        // Obtener sitios web activos
        var websites = MexicanSolarWebsites.GetActiveWebsites();
        Console.WriteLine($"📍 Sitios web configurados: {websites.Count}");
        foreach (var site in websites)
        {
            Console.WriteLine($"   • {site.Nombre} - {site.Url}");
        }
        
        // Inicializar scrapers
        var panelScraper = new SolarPanelScraper();
        var inverterScraper = new InverterScraper();
        var exportService = new DataExportService();
        
        var scrapedData = new ScrapedData();
        scrapedData.SitiosWeb = websites.Select(w => w.Url).ToList();
        
        // Recolectar todas las URLs a procesar
        var allPanelUrls = websites.SelectMany(w => w.UrlsPaneles).ToList();
        var allInverterUrls = websites.SelectMany(w => w.UrlsInversores).ToList();
        
        Console.WriteLine($"\n🌐 Total de URLs a procesar:");
        Console.WriteLine($"   - URLs de paneles: {allPanelUrls.Count}");
        Console.WriteLine($"   - URLs de inversores: {allInverterUrls.Count}");
        
        // Buscar paneles solares
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("BUSCANDO PANELES SOLARES");
        Console.WriteLine(new string('=', 60));
        
        var panels = await panelScraper.SearchMultipleUrlsAsync(allPanelUrls);
        scrapedData.PanealesSolares = panels;
        
        // Buscar inversores
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("BUSCANDO INVERSORES");
        Console.WriteLine(new string('=', 60));
        
        var inverters = await inverterScraper.SearchMultipleUrlsAsync(allInverterUrls);
        scrapedData.Inversores = inverters;
        
        // Mostrar resumen
        exportService.PrintSummary(scrapedData);
        
        // Exportar datos
        Console.WriteLine("\n💾 Exportando datos...");
        
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var jsonPath = $"solar_data_{timestamp}.json";
        var csvPath = $"solar_data_{timestamp}.csv";
        
        exportService.ExportToJson(scrapedData, jsonPath);
        exportService.ExportToCsv(scrapedData, csvPath);
        
        Console.WriteLine("\n✅ Proceso completado exitosamente!");
        Console.WriteLine("\n🔗 NOTA: Los sitios web configurados son ejemplos.");
        Console.WriteLine("   Para obtener datos reales, actualiza las URLs en Config/MexicanSolarWebsites.cs");
        Console.WriteLine("   con sitios web mexicanos reales que vendan equipos solares.");
    }
    
    static void PrintBanner()
    {
        Console.WriteLine(@"
╔═══════════════════════════════════════════════════════════╗
║                                                           ║
║          ☀️  SOLAR SCRAPPER MÉXICO - v1.0 ☀️              ║
║                                                           ║
║     Buscador de Paneles Solares e Inversores             ║
║     para el Mercado Mexicano                             ║
║                                                           ║
╚═══════════════════════════════════════════════════════════╝
");
    }
}
