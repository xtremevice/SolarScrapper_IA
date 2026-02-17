namespace SolarScrapperApp.Models;

public class ScrapedData
{
    public List<SolarPanel> PanelesSolares { get; set; } = new List<SolarPanel>();
    public List<Inverter> Inversores { get; set; } = new List<Inverter>();
    public DateTime FechaExtraccion { get; set; } = DateTime.Now;
    public List<string> SitiosWeb { get; set; } = new List<string>();
    
    public int TotalProductos => PanelesSolares.Count + Inversores.Count;
}
