# SolarScrapper_IA

Aplicación de consola en C# .NET 8 para buscar y recopilar datos de paneles solares e inversores del mercado mexicano.

## 📋 Descripción

SolarScrapper_IA es una herramienta avanzada de scraping web diseñada para recopilar información técnica de equipos solares (paneles e inversores) de sitios web mexicanos que venden productos en México.

### Características v2.0

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

- 🔍 **Detección Inteligente con Palabras Clave**: 
  - Sistema avanzado que identifica automáticamente secciones relevantes
  - Puntuación de relevancia para priorizar información importante
  - Más de 30 palabras clave en español e inglés
  - Reduce falsos positivos y mejora la precisión

- 💾 **Almacenamiento en Base de Datos MySQL**: 
  - Persistencia de datos en MySQL
  - Detección automática de duplicados
  - Claves únicas por marca, modelo y potencia
  - Configuración flexible (variables de entorno)

- 📊 **Exportación de Datos Múltiple**: 
  - Formato JSON (datos completos)
  - Formato CSV (paneles e inversores separados)
  - Base de datos MySQL (opcional)

- ⏱️ **Control de Velocidad y Cortesía Web**:
  - Limitación de tasa automática (2-3 segundos entre solicitudes)
  - Control por dominio para evitar sobrecargas
  - Respeto a los servidores web

## 🚀 Instalación y Uso

### Requisitos Previos

- .NET 8.0 SDK o superior
- Conexión a Internet
- MySQL Server (opcional, para almacenamiento en BD)

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

### Base de Datos MySQL (Opcional)

La aplicación puede almacenar datos en MySQL para persistencia y detección de duplicados.

**Configuración por Variables de Entorno:**
```bash
export DB_SERVER="localhost"
export DB_PORT="3306"
export DB_NAME="solar_scraper_db"
export DB_USER="root"
export DB_PASSWORD="tu_password"
```

**Uso:**
- Al ejecutar, se te preguntará si deseas usar la base de datos
- Presiona Enter o 's' para usar MySQL
- Escribe 'n' para solo usar archivos JSON/CSV

Ver [DATABASE.md](DATABASE.md) para más detalles sobre configuración de MySQL.

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

### Personalizar Palabras Clave

El sistema usa detección inteligente de palabras clave. Para personalizarlas, edita:
`Services/KeywordDetectorService.cs`

Ver [KEYWORDS.md](KEYWORDS.md) para documentación completa sobre el sistema de detección.

## 📁 Estructura del Proyecto

```
SolarScrapperApp/
├── Config/
│   ├── MexicanSolarWebsites.cs    # Configuración de sitios web
│   └── DatabaseConfig.cs           # Configuración de base de datos
├── Models/
│   ├── SolarPanel.cs               # Modelo de panel solar
│   ├── Inverter.cs                 # Modelo de inversor
│   └── ScrapedData.cs             # Modelo de datos recopilados
├── Services/
│   ├── WebScraperService.cs       # Servicio base de scraping
│   ├── DataExportService.cs       # Servicio de exportación
│   ├── DatabaseService.cs         # Servicio de base de datos
│   ├── KeywordDetectorService.cs  # Detección inteligente
│   └── RateLimiterService.cs      # Control de velocidad
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
  "PanelesSolares": [...],
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

### MySQL
Base de datos con tablas:
- `paneles_solares` - Datos de paneles con detección de duplicados
- `inversores` - Datos de inversores con detección de duplicados

## 🔧 Tecnologías Utilizadas

- **C# .NET 8**: Framework principal
- **HtmlAgilityPack 1.12.4**: Parsing de HTML
- **Newtonsoft.Json 13.0.4**: Serialización JSON
- **MySqlConnector 2.5.0**: Conectividad con MySQL

## 🆕 Novedades en v2.0

### Detección Inteligente con Palabras Clave
- Sistema avanzado que identifica automáticamente secciones relevantes
- Más de 30 palabras clave específicas para equipos solares
- Puntuación de relevancia para priorizar información
- Reduce ruido y falsos positivos significativamente

### Base de Datos MySQL
- Almacenamiento persistente de datos
- Detección automática de duplicados por marca, modelo y potencia
- Consultas SQL para análisis de datos
- Opcional - funciona también sin base de datos

### Control de Velocidad Inteligente
- Limitación automática de tasa (2-3 segundos entre solicitudes)
- Control por dominio individual
- Previene sobrecargas en servidores web
- Cumple con buenas prácticas de web scraping

## ⚠️ Consideraciones

- Los sitios web configurados por defecto son ejemplos ilustrativos
- Para uso en producción, actualiza las URLs con sitios web mexicanos reales
- Respeta las políticas de uso y términos de servicio de cada sitio web
- **La aplicación implementa control de velocidad automático** para no sobrecargar servidores
- Algunos sitios pueden requerir autenticación o tener protecciones anti-scraping
- La detección de duplicados requiere MySQL; sin él, pueden guardarse duplicados en archivos

## 📚 Documentación Adicional

- **[DATABASE.md](DATABASE.md)** - Guía completa de configuración y uso de MySQL
- **[KEYWORDS.md](KEYWORDS.md)** - Documentación del sistema de detección de palabras clave
- **[USAGE.md](USAGE.md)** - Guía de uso detallada
- **[EXAMPLES.md](EXAMPLES.md)** - Ejemplos de salida y datos

## 🛠️ Desarrollo Futuro

- [x] Base de datos MySQL para almacenamiento persistente ✅
- [x] Detección inteligente con palabras clave ✅
- [x] Control de velocidad y cortesía web ✅
- [ ] Soporte para más tipos de equipos (baterías, controladores de carga)
- [ ] Interfaz gráfica de usuario (GUI)
- [ ] API REST para consultas
- [ ] Notificaciones de nuevos productos o cambios de precio
- [ ] Comparador de precios
- [ ] Machine Learning para mejorar detección

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

