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
        
        // Configuración de base de datos
        var dbConfig = DatabaseConfig.GetDefault();
        var useDatabase = await PromptForDatabaseUsage();
        
        DatabaseService? dbService = null;
        if (useDatabase)
        {
            dbService = new DatabaseService(dbConfig);
            
            Console.WriteLine("\n📊 Configurando base de datos...");
            Console.WriteLine($"   Servidor: {dbConfig.Server}:{dbConfig.Port}");
            Console.WriteLine($"   Base de datos: {dbConfig.Database}");
            
            if (await dbService.TestConnectionAsync())
            {
                await dbService.InitializeDatabaseAsync();
            }
            else
            {
                Console.WriteLine("⚠️  No se pudo conectar a la base de datos. Continuando sin almacenamiento en BD.");
                dbService = null;
            }
        }
        
        // Obtener sitios web activos
        var websites = MexicanSolarWebsites.GetActiveWebsites();
        Console.WriteLine($"\n📍 Sitios web configurados: {websites.Count}");
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
        Console.WriteLine("\n⏱️  Limitación de velocidad activada: 2-3 segundos entre solicitudes por dominio");
        
        // Buscar paneles solares
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("BUSCANDO PANELES SOLARES");
        Console.WriteLine(new string('=', 60));
        
        var panels = await panelScraper.SearchMultipleUrlsAsync(allPanelUrls);
        scrapedData.PanelesSolares = panels;
        
        // Guardar paneles en base de datos si está habilitada
        if (dbService != null && panels.Any())
        {
            Console.WriteLine("\n💾 Guardando paneles en base de datos...");
            var savedPanels = await dbService.SavePanelsAsync(panels);
            Console.WriteLine($"✓ Paneles guardados en BD: {savedPanels} de {panels.Count} (duplicados omitidos)");
        }
        
        // Buscar inversores
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("BUSCANDO INVERSORES");
        Console.WriteLine(new string('=', 60));
        
        var inverters = await inverterScraper.SearchMultipleUrlsAsync(allInverterUrls);
        scrapedData.Inversores = inverters;
        
        // Guardar inversores en base de datos si está habilitada
        if (dbService != null && inverters.Any())
        {
            Console.WriteLine("\n💾 Guardando inversores en base de datos...");
            var savedInverters = await dbService.SaveInvertersAsync(inverters);
            Console.WriteLine($"✓ Inversores guardados en BD: {savedInverters} de {inverters.Count} (duplicados omitidos)");
        }
        
        // Mostrar resumen
        exportService.PrintSummary(scrapedData);
        
        // Exportar datos a archivos
        Console.WriteLine("\n💾 Exportando datos a archivos...");
        
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var jsonPath = $"solar_data_{timestamp}.json";
        var csvPath = $"solar_data_{timestamp}.csv";
        
        exportService.ExportToJson(scrapedData, jsonPath);
        exportService.ExportToCsv(scrapedData, csvPath);
        
        Console.WriteLine("\n✅ Proceso completado exitosamente!");
        
        if (dbService != null)
        {
            Console.WriteLine("\n📊 Los datos han sido guardados en la base de datos MySQL.");
            Console.WriteLine("   Los duplicados fueron detectados y omitidos automáticamente.");
        }
        
        Console.WriteLine("\n🔗 NOTAS:");
        Console.WriteLine("   • Los sitios web configurados son ejemplos.");
        Console.WriteLine("   • Actualiza las URLs en Config/MexicanSolarWebsites.cs con sitios reales.");
        Console.WriteLine("   • El sistema usa limitación de velocidad para no sobrecargar servidores.");
        Console.WriteLine("   • El detector de palabras clave identifica secciones relevantes automáticamente.");
    }
    
    static async Task<bool> PromptForDatabaseUsage()
    {
        Console.WriteLine("\n❓ ¿Desea usar almacenamiento en base de datos MySQL? (s/n)");
        Console.WriteLine("   Presione Enter para usar base de datos o escriba 'n' para solo archivos: ");
        
        var response = Console.ReadLine()?.Trim().ToLower();
        
        // Si no se proporciona respuesta o es 's', usar base de datos
        return string.IsNullOrEmpty(response) || response == "s" || response == "si" || response == "y" || response == "yes";
    }
    
    static void PrintBanner()
    {
        Console.WriteLine(@"
╔═══════════════════════════════════════════════════════════╗
║                                                           ║
║          ☀️  SOLAR SCRAPPER MÉXICO - v2.0 ☀️              ║
║                                                           ║
║     Buscador de Paneles Solares e Inversores             ║
║     para el Mercado Mexicano                             ║
║                                                           ║
║     ✨ Con Base de Datos MySQL y Detección de Keywords   ║
║                                                           ║
╚═══════════════════════════════════════════════════════════╝
");
    }
}
