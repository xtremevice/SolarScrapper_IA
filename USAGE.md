# Guía de Uso - SolarScrapper México

## 🚀 Inicio Rápido

### Opción 1: Ejecutar desde el código fuente

1. Asegúrate de tener .NET 8 SDK instalado
2. Navega a la carpeta del proyecto:
   ```bash
   cd SolarScrapperApp
   ```
3. Ejecuta la aplicación:
   ```bash
   dotnet run
   ```

### Opción 2: Usar el ejecutable compilado

#### Windows:
1. Navega a: `SolarScrapperApp/bin/Release/net8.0/win-x64/publish/`
2. Ejecuta: `SolarScrapperApp.exe`

#### Linux:
1. Navega a: `SolarScrapperApp/bin/Release/net8.0/linux-x64/publish/`
2. Ejecuta: `./SolarScrapperApp`

## ⚙️ Configuración para Sitios Web Reales

Los sitios web configurados por defecto son ejemplos ilustrativos. Para usar la aplicación con sitios web reales:

1. Abre el archivo `Config/MexicanSolarWebsites.cs`
2. Modifica o agrega nuevos sitios web con sus URLs reales
3. Ejemplo:

```csharp
new WebsiteConfig
{
    Nombre = "Tienda Solar Real",
    Url = "https://www.ejemplo-real.com.mx",
    UrlsPaneles = new List<string> 
    { 
        "https://www.ejemplo-real.com.mx/productos/paneles"
    },
    UrlsInversores = new List<string> 
    { 
        "https://www.ejemplo-real.com.mx/productos/inversores"
    },
    Activo = true
}
```

## 📊 Resultados

La aplicación genera automáticamente archivos con timestamp:

### Archivos JSON:
- `solar_data_YYYYMMDD_HHMMSS.json` - Datos completos estructurados

### Archivos CSV:
- `solar_data_YYYYMMDD_HHMMSS_paneles.csv` - Solo paneles solares
- `solar_data_YYYYMMDD_HHMMSS_inversores.csv` - Solo inversores

## 🔍 Datos Extraídos

### Paneles Solares:
- Marca y modelo
- Tipo de panel (monocristalino, policristalino, PERC, bifacial)
- Voltaje máximo y promedio (V)
- Amperaje máximo y promedio (A)
- Potencia máxima y promedio (W)
- Eficiencia, dimensiones, peso
- URLs de producto y ficha técnica

### Inversores:
- Marca y modelo
- Tipo (On-grid, Off-grid, Híbrido)
- Especificaciones de entrada DC
- Especificaciones de salida AC
- Número de fases
- Eficiencia, dimensiones, peso
- URLs de producto y ficha técnica

## 💡 Consejos

1. **Respeta los términos de servicio**: Verifica que los sitios web permitan web scraping
2. **No sobrecargues los servidores**: La aplicación incluye pausas de 2 segundos entre solicitudes
3. **Actualiza las URLs**: Los sitios web cambian frecuentemente, mantén las URLs actualizadas
4. **Verifica los datos**: Siempre revisa y valida los datos extraídos

## 🛠️ Solución de Problemas

### Error de conexión
- Verifica tu conexión a Internet
- Confirma que las URLs sean correctas y accesibles
- Algunos sitios pueden bloquear solicitudes automatizadas

### Sin datos extraídos
- El sitio puede usar JavaScript para cargar contenido (requiere soluciones avanzadas)
- La estructura HTML del sitio puede ser diferente a los selectores configurados
- El sitio puede tener protección anti-scraping

### Compilación fallida
- Asegúrate de tener .NET 8 SDK instalado
- Ejecuta `dotnet restore` para restaurar paquetes
- Verifica que todos los archivos estén presentes

## 📈 Mejoras Futuras

Para extender la funcionalidad:

1. **Agregar más sitios**: Edita `Config/MexicanSolarWebsites.cs`
2. **Personalizar selectores CSS**: Agrega selectores específicos por sitio
3. **Agregar más campos**: Modifica los modelos en `Models/`
4. **Cambiar formato de salida**: Modifica `Services/DataExportService.cs`

## 📞 Soporte

Si encuentras problemas:
1. Revisa los mensajes de error en la consola
2. Verifica que las URLs sean correctas
3. Abre un issue en GitHub con detalles del problema
