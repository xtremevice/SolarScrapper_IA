using HtmlAgilityPack;
using SolarScrapperApp.Models;
using SolarScrapperApp.Services;
using System.Text.RegularExpressions;

namespace SolarScrapperApp.Scrapers;

public class InverterScraper : WebScraperService
{
    public async Task<List<Inverter>> ScrapeInvertersFromUrlAsync(string url)
    {
        var inverters = new List<Inverter>();
        
        try
        {
            var document = await LoadWebPageAsync(url);
            if (document == null)
                return inverters;

            // Usar detector de palabras clave para encontrar nodos relevantes
            var relevantNodes = _keywordDetector.FindNodesWithInverterKeywords(document);
            if (relevantNodes.Any())
            {
                Console.WriteLine($"🔍 Encontrados {relevantNodes.Count} nodos con palabras clave de inversores");
                var topNodes = _keywordDetector.GetMostRelevantNodes(relevantNodes, isPanel: false, maxNodes: 20);
                
                foreach (var node in topNodes)
                {
                    var inverter = ExtractInverterFromNode(node, url);
                    if (inverter != null && !string.IsNullOrEmpty(inverter.Modelo))
                    {
                        inverters.Add(inverter);
                    }
                }
            }

            // Si no encontramos suficientes inversores, intentar con selectores comunes
            if (inverters.Count < 3)
            {
                var commonInverters = ExtractInvertersUsingCommonSelectors(document, url);
                foreach (var inverter in commonInverters)
                {
                    if (!inverters.Any(i => i.Modelo == inverter.Modelo && i.Marca == inverter.Marca))
                    {
                        inverters.Add(inverter);
                    }
                }
            }
            
            // Si aún no encontramos inversores, intentar extracción inteligente
            if (inverters.Count == 0)
            {
                inverters.AddRange(ExtractInvertersUsingIntelligentSearch(document, url));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error scrapeando inversores de {url}: {ex.Message}");
        }

        return inverters;
    }

    private List<Inverter> ExtractInvertersUsingCommonSelectors(HtmlDocument document, string baseUrl)
    {
        var inverters = new List<Inverter>();
        
        // Selectores comunes usados en sitios de e-commerce
        var productSelectors = new[]
        {
            "//div[contains(@class, 'product')]",
            "//div[contains(@class, 'item')]",
            "//article[contains(@class, 'product')]",
            "//li[contains(@class, 'product')]",
            "//div[contains(@class, 'card')]"
        };

        foreach (var selector in productSelectors)
        {
            var productNodes = document.DocumentNode.SelectNodes(selector);
            if (productNodes != null)
            {
                foreach (var node in productNodes)
                {
                    var inverter = ExtractInverterFromNode(node, baseUrl);
                    if (inverter != null && !string.IsNullOrEmpty(inverter.Modelo))
                    {
                        inverters.Add(inverter);
                    }
                }
                
                if (inverters.Count > 0)
                    break;
            }
        }

        return inverters;
    }

    private List<Inverter> ExtractInvertersUsingIntelligentSearch(HtmlDocument document, string baseUrl)
    {
        var inverters = new List<Inverter>();
        
        // Buscar todo el texto del documento para encontrar especificaciones
        var allText = document.DocumentNode.InnerText;
        
        // Patrones para encontrar inversores
        var powerPattern = @"(\d{1,4}(?:\.\d{1,2})?)\s*[kK][Ww]";
        // var wattagePattern = @"(\d{3,5})\s*[Ww](?:atts?)?";

        var powerMatches = Regex.Matches(allText, powerPattern);
        
        if (powerMatches.Count > 0)
        {
            // Crear inversores con la información encontrada
            foreach (Match match in powerMatches.Take(10)) // Limitar a 10 coincidencias
            {
                if (decimal.TryParse(match.Groups[1].Value, out decimal kw))
                {
                    var inverter = new Inverter
                    {
                        Modelo = $"Inversor {kw}kW",
                        PotenciaSalidaNominal = kw * 1000, // Convertir kW a W
                        UrlProducto = baseUrl,
                        FechaExtraccion = DateTime.Now
                    };
                    inverters.Add(inverter);
                }
            }
        }

        return inverters;
    }

    private Inverter? ExtractInverterFromNode(HtmlNode node, string baseUrl)
    {
        try
        {
            var inverter = new Inverter();
            
            // Extraer información del producto
            var titleNode = node.SelectSingleNode(".//h2 | .//h3 | .//h4 | .//a[contains(@class, 'title')] | .//*[contains(@class, 'product-title')]");
            var title = ExtractTextFromNode(titleNode);
            
            if (!string.IsNullOrEmpty(title))
            {
                inverter.Modelo = CleanText(title);
                
                // Intentar extraer marca del título
                var commonBrands = new[] { "Fronius", "SMA", "Huawei", "Growatt", "Goodwe", 
                                          "Solax", "SolarEdge", "Enphase", "ABB", "Delta", 
                                          "Sungrow", "KACO", "Schneider Electric" };
                
                foreach (var brand in commonBrands)
                {
                    if (title.Contains(brand, StringComparison.OrdinalIgnoreCase))
                    {
                        inverter.Marca = brand;
                        break;
                    }
                }
            }
            
            // Extraer especificaciones del texto del nodo
            var nodeText = node.InnerText;
            
            // Buscar potencia en kW
            var kwMatch = Regex.Match(nodeText, @"(\d{1,4}(?:\.\d{1,2})?)\s*[kK][Ww]");
            if (kwMatch.Success)
            {
                var kw = ExtractNumber(kwMatch.Groups[1].Value);
                if (kw.HasValue)
                {
                    inverter.PotenciaSalidaNominal = kw.Value * 1000; // Convertir a Watts
                }
            }
            
            // Buscar potencia en W (si no encontró kW)
            if (!inverter.PotenciaSalidaNominal.HasValue)
            {
                var wattsMatch = Regex.Match(nodeText, @"(\d{3,5})\s*[Ww]");
                if (wattsMatch.Success)
                {
                    inverter.PotenciaSalidaNominal = ExtractNumber(wattsMatch.Groups[1].Value);
                }
            }
            
            // Buscar voltaje de entrada
            var voltageInMatch = Regex.Match(nodeText, @"(?:entrada|input|DC).*?(\d{2,4})\s*[Vv]");
            if (voltageInMatch.Success)
            {
                inverter.VoltajeEntradaMaximo = ExtractNumber(voltageInMatch.Groups[1].Value);
            }
            
            // Buscar voltaje de salida (AC)
            var voltageOutMatch = Regex.Match(nodeText, @"(?:salida|output|AC).*?(\d{2,3})\s*[Vv]");
            if (voltageOutMatch.Success)
            {
                inverter.VoltajeSalidaNominal = ExtractNumber(voltageOutMatch.Groups[1].Value);
            }
            
            // Identificar tipo de inversor
            if (nodeText.Contains("híbrido", StringComparison.OrdinalIgnoreCase) ||
                nodeText.Contains("hybrid", StringComparison.OrdinalIgnoreCase))
                inverter.TipoInversor = "Híbrido";
            else if (nodeText.Contains("off-grid", StringComparison.OrdinalIgnoreCase) ||
                     nodeText.Contains("aislado", StringComparison.OrdinalIgnoreCase))
                inverter.TipoInversor = "Off-Grid";
            else if (nodeText.Contains("on-grid", StringComparison.OrdinalIgnoreCase) ||
                     nodeText.Contains("interconectado", StringComparison.OrdinalIgnoreCase) ||
                     nodeText.Contains("grid-tied", StringComparison.OrdinalIgnoreCase))
                inverter.TipoInversor = "On-Grid";
            
            // Identificar número de fases
            if (nodeText.Contains("trifásico", StringComparison.OrdinalIgnoreCase) ||
                nodeText.Contains("three-phase", StringComparison.OrdinalIgnoreCase) ||
                nodeText.Contains("3 phase", StringComparison.OrdinalIgnoreCase))
                inverter.NumeroFases = 3;
            else if (nodeText.Contains("monofásico", StringComparison.OrdinalIgnoreCase) ||
                     nodeText.Contains("single-phase", StringComparison.OrdinalIgnoreCase) ||
                     nodeText.Contains("1 phase", StringComparison.OrdinalIgnoreCase))
                inverter.NumeroFases = 1;
            
            // Buscar enlace del producto
            var linkNode = node.SelectSingleNode(".//a[@href]");
            if (linkNode != null)
            {
                var href = linkNode.GetAttributeValue("href", "");
                if (!string.IsNullOrEmpty(href))
                {
                    inverter.UrlProducto = href.StartsWith("http") ? href : new Uri(new Uri(baseUrl), href).ToString();
                }
            }
            
            inverter.FechaExtraccion = DateTime.Now;
            
            return inverter;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error extrayendo inversor: {ex.Message}");
            return null;
        }
    }

    public async Task<List<Inverter>> SearchMultipleUrlsAsync(List<string> urls)
    {
        var allInverters = new List<Inverter>();
        
        foreach (var url in urls)
        {
            Console.WriteLine($"\n=== Buscando inversores en: {url} ===");
            var inverters = await ScrapeInvertersFromUrlAsync(url);
            allInverters.AddRange(inverters);
            Console.WriteLine($"Encontrados {inverters.Count} inversores");
            
            // El rate limiter ya maneja las pausas automáticamente
        }
        
        return allInverters;
    }
}
