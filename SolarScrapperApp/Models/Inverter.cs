namespace SolarScrapperApp.Models;

public class Inverter
{
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string TipoInversor { get; set; } = string.Empty; // On-grid, Off-grid, Híbrido
    
    // Especificaciones eléctricas de entrada (DC)
    public decimal? VoltajeEntradaMaximo { get; set; }
    public decimal? VoltajeEntradaMinimo { get; set; }
    public decimal? CorrienteEntradaMaxima { get; set; }
    public decimal? PotenciaEntradaMaxima { get; set; }
    
    // Especificaciones eléctricas de salida (AC)
    public decimal? VoltajeSalidaNominal { get; set; }
    public decimal? PotenciaSalidaNominal { get; set; }
    public decimal? PotenciaSalidaMaxima { get; set; }
    public decimal? FrecuenciaSalida { get; set; }
    
    // Información adicional
    public string? Eficiencia { get; set; }
    public string? Dimensiones { get; set; }
    public string? Peso { get; set; }
    public string? Garantia { get; set; }
    public int? NumeroFases { get; set; }
    public string? UrlProducto { get; set; }
    public string? UrlFichaTecnica { get; set; }
    public DateTime FechaExtraccion { get; set; } = DateTime.Now;
    
    public override string ToString()
    {
        return $"{Marca} {Modelo} - {PotenciaSalidaNominal}W - {TipoInversor}";
    }
}
