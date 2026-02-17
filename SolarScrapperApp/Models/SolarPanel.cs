namespace SolarScrapperApp.Models;

public class SolarPanel
{
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string TipoPanel { get; set; } = string.Empty;
    
    // Especificaciones eléctricas
    public decimal? VoltajeMaximo { get; set; }
    public decimal? VoltajePromedio { get; set; }
    public decimal? AmperajeMaximo { get; set; }
    public decimal? AmperajePromedio { get; set; }
    public decimal? WattsMaximo { get; set; }
    public decimal? WattsPromedio { get; set; }
    
    // Información adicional
    public string? Eficiencia { get; set; }
    public string? Dimensiones { get; set; }
    public string? Peso { get; set; }
    public string? Garantia { get; set; }
    public string? UrlProducto { get; set; }
    public string? UrlFichaTecnica { get; set; }
    public DateTime FechaExtraccion { get; set; } = DateTime.Now;
    
    public override string ToString()
    {
        return $"{Marca} {Modelo} - {WattsMaximo}W - {TipoPanel}";
    }
}
