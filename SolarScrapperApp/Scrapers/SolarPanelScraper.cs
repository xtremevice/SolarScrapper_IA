using HtmlAgilityPack;
using SolarScrapperApp.Models;
using SolarScrapperApp.Services;
using System.Text.RegularExpressions;

namespace SolarScrapperApp.Scrapers;

public class SolarPanelScraper : WebScraperService
{
    public async Task<List<SolarPanel>> ScrapePanelsFromUrlAsync(string url)
    {
        var panels = new List<SolarPanel>();
        
        try
        {
            var document = await LoadWebPageAsync(url);
            if (document == null)
                return panels;

            // Intentar múltiples estrategias de búsqueda de productos
            panels.AddRange(ExtractPanelsUsingCommonSelectors(document, url));
            
            // Si no encontramos paneles con selectores comunes, intentar extracción inteligente
            if (panels.Count == 0)
            {
                panels.AddRange(ExtractPanelsUsingIntelligentSearch(document, url));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error scrapeando paneles de {url}: {ex.Message}");
        }

        return panels;
    }

    private List<SolarPanel> ExtractPanelsUsingCommonSelectors(HtmlDocument document, string baseUrl)
    {
        var panels = new List<SolarPanel>();
        
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
                    var panel = ExtractPanelFromNode(node, baseUrl);
                    if (panel != null && !string.IsNullOrEmpty(panel.Modelo))
                    {
                        panels.Add(panel);
                    }
                }
                
                if (panels.Count > 0)
                    break;
            }
        }

        return panels;
    }

    private List<SolarPanel> ExtractPanelsUsingIntelligentSearch(HtmlDocument document, string baseUrl)
    {
        var panels = new List<SolarPanel>();
        
        // Buscar todo el texto del documento para encontrar especificaciones
        var allText = document.DocumentNode.InnerText;
        
        // Patrones para encontrar paneles solares
        var wattagePattern = @"(\d{2,4})\s*[Ww](?:atts?)?";
        // var voltagePattern = @"(\d{1,3}(?:\.\d{1,2})?)\s*[Vv](?:olts?)?";
        // var ampPattern = @"(\d{1,3}(?:\.\d{1,2})?)\s*[Aa](?:mps?|mperes?)?";

        var wattMatches = Regex.Matches(allText, wattagePattern);
        
        if (wattMatches.Count > 0)
        {
            // Crear paneles ficticios con la información encontrada
            foreach (Match match in wattMatches.Take(10)) // Limitar a 10 coincidencias
            {
                if (decimal.TryParse(match.Groups[1].Value, out decimal watts))
                {
                    var panel = new SolarPanel
                    {
                        Modelo = $"Panel {watts}W",
                        WattsMaximo = watts,
                        UrlProducto = baseUrl,
                        FechaExtraccion = DateTime.Now
                    };
                    panels.Add(panel);
                }
            }
        }

        return panels;
    }

    private SolarPanel? ExtractPanelFromNode(HtmlNode node, string baseUrl)
    {
        try
        {
            var panel = new SolarPanel();
            
            // Extraer información del producto
            var titleNode = node.SelectSingleNode(".//h2 | .//h3 | .//h4 | .//a[contains(@class, 'title')] | .//*[contains(@class, 'product-title')]");
            var title = ExtractTextFromNode(titleNode);
            
            if (!string.IsNullOrEmpty(title))
            {
                panel.Modelo = CleanText(title);
                
                // Intentar extraer marca del título
                var commonBrands = new[] { "Canadian Solar", "Jinko", "Trina", "LONGi", "JA Solar", 
                                          "Risen", "Phono Solar", "Sunpower", "LG", "Panasonic", 
                                          "Hanwha Q CELLS", "First Solar", "Vikram Solar" };
                
                foreach (var brand in commonBrands)
                {
                    if (title.Contains(brand, StringComparison.OrdinalIgnoreCase))
                    {
                        panel.Marca = brand;
                        break;
                    }
                }
            }
            
            // Extraer especificaciones del texto del nodo
            var nodeText = node.InnerText;
            
            // Buscar Watts
            var wattsMatch = Regex.Match(nodeText, @"(\d{2,4})\s*[Ww]");
            if (wattsMatch.Success)
            {
                panel.WattsMaximo = ExtractNumber(wattsMatch.Groups[1].Value);
            }
            
            // Buscar Voltaje
            var voltageMatch = Regex.Match(nodeText, @"(\d{1,3}(?:\.\d{1,2})?)\s*[Vv]");
            if (voltageMatch.Success)
            {
                panel.VoltajeMaximo = ExtractNumber(voltageMatch.Groups[1].Value);
            }
            
            // Buscar Amperaje
            var ampMatch = Regex.Match(nodeText, @"(\d{1,3}(?:\.\d{1,2})?)\s*[Aa]");
            if (ampMatch.Success)
            {
                panel.AmperajeMaximo = ExtractNumber(ampMatch.Groups[1].Value);
            }
            
            // Identificar tipo de panel
            if (nodeText.Contains("monocristalino", StringComparison.OrdinalIgnoreCase))
                panel.TipoPanel = "Monocristalino";
            else if (nodeText.Contains("policristalino", StringComparison.OrdinalIgnoreCase))
                panel.TipoPanel = "Policristalino";
            else if (nodeText.Contains("PERC", StringComparison.OrdinalIgnoreCase))
                panel.TipoPanel = "PERC";
            else if (nodeText.Contains("bifacial", StringComparison.OrdinalIgnoreCase))
                panel.TipoPanel = "Bifacial";
            
            // Buscar enlace del producto
            var linkNode = node.SelectSingleNode(".//a[@href]");
            if (linkNode != null)
            {
                var href = linkNode.GetAttributeValue("href", "");
                if (!string.IsNullOrEmpty(href))
                {
                    panel.UrlProducto = href.StartsWith("http") ? href : new Uri(new Uri(baseUrl), href).ToString();
                }
            }
            
            panel.FechaExtraccion = DateTime.Now;
            
            return panel;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error extrayendo panel: {ex.Message}");
            return null;
        }
    }

    public async Task<List<SolarPanel>> SearchMultipleUrlsAsync(List<string> urls)
    {
        var allPanels = new List<SolarPanel>();
        
        foreach (var url in urls)
        {
            Console.WriteLine($"\n=== Buscando paneles en: {url} ===");
            var panels = await ScrapePanelsFromUrlAsync(url);
            allPanels.AddRange(panels);
            Console.WriteLine($"Encontrados {panels.Count} paneles");
            
            // Pequeña pausa para no sobrecargar los servidores
            await Task.Delay(2000);
        }
        
        return allPanels;
    }
}
