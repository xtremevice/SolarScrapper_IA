# SolarScrapper_IA

Aplicación de consola en C# .NET 8 para buscar y recopilar datos de paneles solares e inversores del mercado mexicano.

## 📋 Descripción

SolarScrapper_IA es una herramienta de scraping web diseñada para recopilar información técnica de equipos solares (paneles e inversores) de sitios web mexicanos que venden productos en México. 

### Características

- ☀️ **Búsqueda de Paneles Solares**: Extrae especificaciones técnicas como:
  - Marca y modelo
  - Voltaje (máximo y promedio)
  - Amperaje (máximo y promedio)
  - Potencia en Watts (máximo y promedio)
  - Tipo de panel (monocristalino, policristalino, PERC, bifacial)
  - Eficiencia, dimensiones, peso y garantía

- ⚡ **Búsqueda de Inversores**: Recopila información como:
  - Marca y modelo
  - Tipo (On-grid, Off-grid, Híbrido)
  - Especificaciones de entrada (DC) y salida (AC)
  - Voltajes y potencias
  - Número de fases (monofásico/trifásico)

- 💾 **Exportación de Datos**: 
  - Formato JSON (datos completos)
  - Formato CSV (paneles e inversores separados)

## 🚀 Instalación y Uso

### Requisitos Previos

- .NET 8.0 SDK o superior
- Conexión a Internet

### Compilar la Aplicación

```bash
cd SolarScrapperApp
dotnet build
```

### Ejecutar la Aplicación

```bash
dotnet run
```

### Publicar como Ejecutable

Para crear un ejecutable standalone:

```bash
# Windows
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true

# Linux
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true

# macOS
dotnet publish -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true
```

El ejecutable se generará en: `bin/Release/net8.0/[runtime]/publish/`

## ⚙️ Configuración

### Agregar Sitios Web Mexicanos

Edita el archivo `Config/MexicanSolarWebsites.cs` para agregar o modificar los sitios web a escanear:

```csharp
new WebsiteConfig
{
    Nombre = "Nombre del Sitio",
    Url = "https://www.ejemplo.com.mx",
    UrlsPaneles = new List<string> 
    { 
        "https://www.ejemplo.com.mx/paneles-solares"
    },
    UrlsInversores = new List<string> 
    { 
        "https://www.ejemplo.com.mx/inversores"
    },
    Activo = true
}
```

## 📁 Estructura del Proyecto

```
SolarScrapperApp/
├── Config/
│   └── MexicanSolarWebsites.cs    # Configuración de sitios web
├── Models/
│   ├── SolarPanel.cs               # Modelo de panel solar
│   ├── Inverter.cs                 # Modelo de inversor
│   └── ScrapedData.cs             # Modelo de datos recopilados
├── Services/
│   ├── WebScraperService.cs       # Servicio base de scraping
│   └── DataExportService.cs       # Servicio de exportación
├── Scrapers/
│   ├── SolarPanelScraper.cs       # Scraper de paneles
│   └── InverterScraper.cs         # Scraper de inversores
└── Program.cs                      # Punto de entrada de la aplicación
```

## 📊 Formato de Salida

### JSON
Archivo completo con todos los datos estructurados:
```json
{
  "PanealesSolares": [...],
  "Inversores": [...],
  "FechaExtraccion": "2024-01-01T10:00:00",
  "SitiosWeb": [...],
  "TotalProductos": 150
}
```

### CSV
Dos archivos separados:
- `solar_data_[timestamp]_paneles.csv`
- `solar_data_[timestamp]_inversores.csv`

## 🔧 Tecnologías Utilizadas

- **C# .NET 8**: Framework principal
- **HtmlAgilityPack**: Parsing de HTML
- **Newtonsoft.Json**: Serialización JSON

## ⚠️ Consideraciones

- Los sitios web configurados por defecto son ejemplos ilustrativos
- Para uso en producción, actualiza las URLs con sitios web mexicanos reales
- Respeta las políticas de uso y términos de servicio de cada sitio web
- Implementa pausas entre solicitudes para no sobrecargar los servidores
- Algunos sitios pueden requerir autenticación o tener protecciones anti-scraping

## 🛠️ Desarrollo Futuro

- [ ] Soporte para más tipos de equipos (baterías, controladores de carga)
- [ ] Interfaz gráfica de usuario (GUI)
- [ ] Base de datos para almacenamiento persistente
- [ ] API REST para consultas
- [ ] Notificaciones de nuevos productos o cambios de precio
- [ ] Comparador de precios

## 📝 Licencia

Este proyecto es de código abierto y está disponible bajo la licencia MIT.

## 🤝 Contribuciones

Las contribuciones son bienvenidas. Por favor:
1. Haz un Fork del proyecto
2. Crea una rama para tu característica (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📧 Contacto

Para preguntas o sugerencias, por favor abre un issue en GitHub.

