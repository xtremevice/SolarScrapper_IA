using HtmlAgilityPack;
using SolarScrapperApp.Models;
using System.Text.RegularExpressions;

namespace SolarScrapperApp.Services;

public class WebScraperService
{
    private readonly HttpClient _httpClient;
    private readonly HtmlWeb _htmlWeb;

    public WebScraperService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", 
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
        
        _htmlWeb = new HtmlWeb();
    }

    public async Task<HtmlDocument?> LoadWebPageAsync(string url)
    {
        try
        {
            Console.WriteLine($"Cargando página: {url}");
            var document = await Task.Run(() => _htmlWeb.Load(url));
            return document;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cargando página {url}: {ex.Message}");
            return null;
        }
    }

    public decimal? ExtractNumber(string? text, string pattern = @"[\d,.]+")
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        var match = Regex.Match(text, pattern);
        if (match.Success)
        {
            var numberStr = match.Value.Replace(",", ".");
            if (decimal.TryParse(numberStr, out decimal result))
                return result;
        }
        return null;
    }

    public string? ExtractTextFromNode(HtmlNode? node)
    {
        if (node == null)
            return null;
        
        return node.InnerText?.Trim();
    }

    protected string CleanText(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;
        
        return Regex.Replace(text.Trim(), @"\s+", " ");
    }
}
