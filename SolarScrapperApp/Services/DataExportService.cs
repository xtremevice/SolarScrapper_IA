using Newtonsoft.Json;
using SolarScrapperApp.Models;
using System.Text;

namespace SolarScrapperApp.Services;

public class DataExportService
{
    public void ExportToJson(ScrapedData data, string filePath)
    {
        try
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, json, Encoding.UTF8);
            Console.WriteLine($"\n✓ Datos exportados a JSON: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error exportando a JSON: {ex.Message}");
        }
    }

    public void ExportToCsv(ScrapedData data, string basePath)
    {
        try
        {
            // Exportar paneles solares
            var panelsPath = basePath.Replace(".csv", "_paneles.csv");
            ExportPanelsToCsv(data.PanealesSolares, panelsPath);
            
            // Exportar inversores
            var invertersPath = basePath.Replace(".csv", "_inversores.csv");
            ExportInvertersToCsv(data.Inversores, invertersPath);
            
            Console.WriteLine($"\n✓ Datos exportados a CSV:");
            Console.WriteLine($"  - Paneles: {panelsPath}");
            Console.WriteLine($"  - Inversores: {invertersPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error exportando a CSV: {ex.Message}");
        }
    }

    private void ExportPanelsToCsv(List<SolarPanel> panels, string filePath)
    {
        var csv = new StringBuilder();
        
        // Encabezados
        csv.AppendLine("Marca,Modelo,Tipo,Voltaje_Max,Voltaje_Prom,Amperaje_Max,Amperaje_Prom,Watts_Max,Watts_Prom,Eficiencia,URL_Producto,Fecha_Extraccion");
        
        // Datos
        foreach (var panel in panels)
        {
            csv.AppendLine($"\"{panel.Marca}\",\"{panel.Modelo}\",\"{panel.TipoPanel}\"," +
                          $"{panel.VoltajeMaximo},{panel.VoltajePromedio}," +
                          $"{panel.AmperajeMaximo},{panel.AmperajePromedio}," +
                          $"{panel.WattsMaximo},{panel.WattsPromedio}," +
                          $"\"{panel.Eficiencia}\",\"{panel.UrlProducto}\"," +
                          $"{panel.FechaExtraccion:yyyy-MM-dd HH:mm:ss}");
        }
        
        File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
    }

    private void ExportInvertersToCsv(List<Inverter> inverters, string filePath)
    {
        var csv = new StringBuilder();
        
        // Encabezados
        csv.AppendLine("Marca,Modelo,Tipo,Voltaje_Entrada_Max,Voltaje_Entrada_Min,Potencia_Salida_Nominal,Potencia_Salida_Max,Numero_Fases,URL_Producto,Fecha_Extraccion");
        
        // Datos
        foreach (var inv in inverters)
        {
            csv.AppendLine($"\"{inv.Marca}\",\"{inv.Modelo}\",\"{inv.TipoInversor}\"," +
                          $"{inv.VoltajeEntradaMaximo},{inv.VoltajeEntradaMinimo}," +
                          $"{inv.PotenciaSalidaNominal},{inv.PotenciaSalidaMaxima}," +
                          $"{inv.NumeroFases},\"{inv.UrlProducto}\"," +
                          $"{inv.FechaExtraccion:yyyy-MM-dd HH:mm:ss}");
        }
        
        File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
    }

    public void PrintSummary(ScrapedData data)
    {
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("RESUMEN DE DATOS EXTRAÍDOS");
        Console.WriteLine(new string('=', 60));
        
        Console.WriteLine($"\n📊 Total de productos encontrados: {data.TotalProductos}");
        Console.WriteLine($"   - Paneles solares: {data.PanealesSolares.Count}");
        Console.WriteLine($"   - Inversores: {data.Inversores.Count}");
        
        if (data.PanealesSolares.Any())
        {
            Console.WriteLine("\n☀️  PANELES SOLARES:");
            foreach (var panel in data.PanealesSolares.Take(5))
            {
                Console.WriteLine($"   • {panel}");
            }
            if (data.PanealesSolares.Count > 5)
                Console.WriteLine($"   ... y {data.PanealesSolares.Count - 5} más");
        }
        
        if (data.Inversores.Any())
        {
            Console.WriteLine("\n⚡ INVERSORES:");
            foreach (var inv in data.Inversores.Take(5))
            {
                Console.WriteLine($"   • {inv}");
            }
            if (data.Inversores.Count > 5)
                Console.WriteLine($"   ... y {data.Inversores.Count - 5} más");
        }
        
        Console.WriteLine("\n" + new string('=', 60));
    }
}
