using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace SolarScrapperApp.Services;

public class KeywordDetectorService
{
    // Palabras clave para paneles solares
    private readonly List<string> _panelKeywords = new List<string>
    {
        "panel solar", "panel fotovoltaico", "módulo solar", "módulo fotovoltaico",
        "celda solar", "placa solar", "monocristalino", "policristalino", "perc",
        "bifacial", "watts", "potencia", "voltaje", "amperaje", "eficiencia",
        "wp", "watt peak", "voc", "isc", "vmp", "imp"
    };
    
    // Palabras clave para inversores
    private readonly List<string> _inverterKeywords = new List<string>
    {
        "inversor", "inverter", "convertidor", "on-grid", "off-grid", "híbrido",
        "interconectado", "aislado", "grid-tied", "monofásico", "trifásico",
        "mppt", "potencia nominal", "eficiencia", "voltaje entrada", "voltaje salida",
        "corriente máxima", "frecuencia", "kw", "kilowatt"
    };
    
    // Palabras clave para especificaciones técnicas
    private readonly List<string> _technicalKeywords = new List<string>
    {
        "especificaciones", "características", "ficha técnica", "datasheet",
        "datos técnicos", "specifications", "technical data", "performance",
        "rendimiento", "garantía", "warranty", "certificación", "norma"
    };
    
    public bool ContainsPanelKeywords(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;
        
        var lowerText = text.ToLower();
        return _panelKeywords.Any(keyword => lowerText.Contains(keyword.ToLower()));
    }
    
    public bool ContainsInverterKeywords(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;
        
        var lowerText = text.ToLower();
        return _inverterKeywords.Any(keyword => lowerText.Contains(keyword.ToLower()));
    }
    
    public bool ContainsTechnicalKeywords(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;
        
        var lowerText = text.ToLower();
        return _technicalKeywords.Any(keyword => lowerText.Contains(keyword.ToLower()));
    }
    
    public List<HtmlNode> FindNodesWithPanelKeywords(HtmlDocument document)
    {
        var relevantNodes = new List<HtmlNode>();
        
        if (document?.DocumentNode == null)
            return relevantNodes;
        
        // Buscar nodos que contengan palabras clave de paneles
        var allNodes = document.DocumentNode.SelectNodes("//div | //section | //article | //li");
        if (allNodes != null)
        {
            foreach (var node in allNodes)
            {
                var nodeText = node.InnerText;
                if (ContainsPanelKeywords(nodeText) || ContainsTechnicalKeywords(nodeText))
                {
                    relevantNodes.Add(node);
                }
            }
        }
        
        return relevantNodes;
    }
    
    public List<HtmlNode> FindNodesWithInverterKeywords(HtmlDocument document)
    {
        var relevantNodes = new List<HtmlNode>();
        
        if (document?.DocumentNode == null)
            return relevantNodes;
        
        // Buscar nodos que contengan palabras clave de inversores
        var allNodes = document.DocumentNode.SelectNodes("//div | //section | //article | //li");
        if (allNodes != null)
        {
            foreach (var node in allNodes)
            {
                var nodeText = node.InnerText;
                if (ContainsInverterKeywords(nodeText) || ContainsTechnicalKeywords(nodeText))
                {
                    relevantNodes.Add(node);
                }
            }
        }
        
        return relevantNodes;
    }
    
    public int CalculateRelevanceScore(string text, bool isPanel)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;
        
        var lowerText = text.ToLower();
        var score = 0;
        
        var keywords = isPanel ? _panelKeywords : _inverterKeywords;
        
        foreach (var keyword in keywords)
        {
            if (lowerText.Contains(keyword.ToLower()))
            {
                score++;
            }
        }
        
        // Bonus por palabras clave técnicas
        foreach (var keyword in _technicalKeywords)
        {
            if (lowerText.Contains(keyword.ToLower()))
            {
                score++;
            }
        }
        
        return score;
    }
    
    public List<HtmlNode> GetMostRelevantNodes(List<HtmlNode> nodes, bool isPanel, int maxNodes = 10)
    {
        return nodes
            .Select(node => new { Node = node, Score = CalculateRelevanceScore(node.InnerText, isPanel) })
            .OrderByDescending(x => x.Score)
            .Where(x => x.Score > 0)
            .Take(maxNodes)
            .Select(x => x.Node)
            .ToList();
    }
}
