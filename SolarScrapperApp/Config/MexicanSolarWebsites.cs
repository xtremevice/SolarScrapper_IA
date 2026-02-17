namespace SolarScrapperApp.Config;

public static class MexicanSolarWebsites
{
    // Lista de sitios web mexicanos que venden equipos solares
    public static readonly List<WebsiteConfig> Websites = new List<WebsiteConfig>
    {
        new WebsiteConfig
        {
            Nombre = "SolarMex",
            Url = "https://www.solarmex.com.mx",
            UrlsPaneles = new List<string> 
            { 
                "https://www.solarmex.com.mx/paneles-solares"
            },
            UrlsInversores = new List<string> 
            { 
                "https://www.solarmex.com.mx/inversores"
            },
            Activo = true
        },
        new WebsiteConfig
        {
            Nombre = "Enlight",
            Url = "https://enlight.mx",
            UrlsPaneles = new List<string> 
            { 
                "https://enlight.mx/tienda/paneles-solares"
            },
            UrlsInversores = new List<string> 
            { 
                "https://enlight.mx/tienda/inversores"
            },
            Activo = true
        },
        new WebsiteConfig
        {
            Nombre = "SolarTech México",
            Url = "https://www.solartechmexico.com",
            UrlsPaneles = new List<string> 
            { 
                "https://www.solartechmexico.com/productos/paneles"
            },
            UrlsInversores = new List<string> 
            { 
                "https://www.solartechmexico.com/productos/inversores"
            },
            Activo = true
        },
        new WebsiteConfig
        {
            Nombre = "EcoSolar México",
            Url = "https://www.ecosolarmexico.com.mx",
            UrlsPaneles = new List<string> 
            { 
                "https://www.ecosolarmexico.com.mx/paneles-solares"
            },
            UrlsInversores = new List<string> 
            { 
                "https://www.ecosolarmexico.com.mx/inversores-solares"
            },
            Activo = true
        }
    };
    
    public static List<WebsiteConfig> GetActiveWebsites()
    {
        return Websites.Where(w => w.Activo).ToList();
    }
}

public class WebsiteConfig
{
    public string Nombre { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public List<string> UrlsPaneles { get; set; } = new List<string>();
    public List<string> UrlsInversores { get; set; } = new List<string>();
    public bool Activo { get; set; } = true;
    
    // Selectores CSS específicos por sitio (se pueden configurar)
    public string? SelectorProducto { get; set; }
    public string? SelectorMarca { get; set; }
    public string? SelectorModelo { get; set; }
    public string? SelectorPrecio { get; set; }
}
