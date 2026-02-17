namespace SolarScrapperApp.Models;

public class ScrapedData
{
    public List<SolarPanel> PanealesSolares { get; set; } = new List<SolarPanel>();
    public List<Inverter> Inversores { get; set; } = new List<Inverter>();
    public DateTime FechaExtraccion { get; set; } = DateTime.Now;
    public List<string> SitiosWeb { get; set; } = new List<string>();
    
    public int TotalProductos => PanealesSolares.Count + Inversores.Count;
}
