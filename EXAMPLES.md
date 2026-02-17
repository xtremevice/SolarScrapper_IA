# Ejemplos de Salida - SolarScrapper México

## Ejemplo de Salida JSON

Cuando la aplicación encuentra productos, el archivo JSON tendrá una estructura como esta:

```json
{
  "PanelesSolares": [
    {
      "Marca": "Canadian Solar",
      "Modelo": "HiKu CS3W-400MS",
      "TipoPanel": "Monocristalino",
      "VoltajeMaximo": 48.7,
      "VoltajePromedio": 40.5,
      "AmperajeMaximo": 10.16,
      "AmperajePromedio": 9.87,
      "WattsMaximo": 400,
      "WattsPromedio": 380,
      "Eficiencia": "20.5%",
      "Dimensiones": "2108 x 1048 x 40 mm",
      "Peso": "22.5 kg",
      "Garantia": "25 años",
      "UrlProducto": "https://ejemplo.com.mx/panel-400w",
      "UrlFichaTecnica": "https://ejemplo.com.mx/fichas/panel-400w.pdf",
      "FechaExtraccion": "2026-02-17T09:00:00"
    },
    {
      "Marca": "Jinko Solar",
      "Modelo": "Tiger Pro 550W",
      "TipoPanel": "PERC",
      "VoltajeMaximo": 49.5,
      "VoltajePromedio": 41.2,
      "AmperajeMaximo": 13.96,
      "AmperajePromedio": 13.35,
      "WattsMaximo": 550,
      "WattsPromedio": 520,
      "Eficiencia": "21.2%",
      "Dimensiones": "2278 x 1134 x 35 mm",
      "Peso": "27.8 kg",
      "Garantia": "25 años",
      "UrlProducto": "https://ejemplo.com.mx/panel-550w",
      "UrlFichaTecnica": null,
      "FechaExtraccion": "2026-02-17T09:00:15"
    }
  ],
  "Inversores": [
    {
      "Marca": "Fronius",
      "Modelo": "Primo 5.0-1",
      "TipoInversor": "On-Grid",
      "VoltajeEntradaMaximo": 1000,
      "VoltajeEntradaMinimo": 80,
      "CorrienteEntradaMaxima": 12.5,
      "PotenciaEntradaMaxima": 7500,
      "VoltajeSalidaNominal": 230,
      "PotenciaSalidaNominal": 5000,
      "PotenciaSalidaMaxima": 5000,
      "FrecuenciaSalida": 50,
      "Eficiencia": "98.1%",
      "Dimensiones": "645 x 431 x 204 mm",
      "Peso": "19.5 kg",
      "Garantia": "5 años",
      "NumeroFases": 1,
      "UrlProducto": "https://ejemplo.com.mx/inversor-5kw",
      "UrlFichaTecnica": "https://ejemplo.com.mx/fichas/fronius-5kw.pdf",
      "FechaExtraccion": "2026-02-17T09:01:00"
    },
    {
      "Marca": "Huawei",
      "Modelo": "SUN2000-10KTL-M1",
      "TipoInversor": "Híbrido",
      "VoltajeEntradaMaximo": 1100,
      "VoltajeEntradaMinimo": 200,
      "CorrienteEntradaMaxima": 26,
      "PotenciaEntradaMaxima": 15000,
      "VoltajeSalidaNominal": 400,
      "PotenciaSalidaNominal": 10000,
      "PotenciaSalidaMaxima": 11000,
      "FrecuenciaSalida": 50,
      "Eficiencia": "98.6%",
      "Dimensiones": "525 x 470 x 166 mm",
      "Peso": "25 kg",
      "Garantia": "10 años",
      "NumeroFases": 3,
      "UrlProducto": "https://ejemplo.com.mx/inversor-10kw",
      "UrlFichaTecnica": null,
      "FechaExtraccion": "2026-02-17T09:01:15"
    }
  ],
  "FechaExtraccion": "2026-02-17T09:01:20",
  "SitiosWeb": [
    "https://www.solarmex.com.mx",
    "https://enlight.mx",
    "https://www.solartechmexico.com"
  ],
  "TotalProductos": 4
}
```

## Ejemplo de Salida CSV - Paneles Solares

```csv
Marca,Modelo,Tipo,Voltaje_Max,Voltaje_Prom,Amperaje_Max,Amperaje_Prom,Watts_Max,Watts_Prom,Eficiencia,URL_Producto,Fecha_Extraccion
"Canadian Solar","HiKu CS3W-400MS","Monocristalino",48.7,40.5,10.16,9.87,400,380,"20.5%","https://ejemplo.com.mx/panel-400w",2026-02-17 09:00:00
"Jinko Solar","Tiger Pro 550W","PERC",49.5,41.2,13.96,13.35,550,520,"21.2%","https://ejemplo.com.mx/panel-550w",2026-02-17 09:00:15
"Trina Solar","Vertex S 450W","Monocristalino",41.2,34.8,13.8,12.9,450,430,"21.0%","https://ejemplo.com.mx/panel-450w",2026-02-17 09:00:30
```

## Ejemplo de Salida CSV - Inversores

```csv
Marca,Modelo,Tipo,Voltaje_Entrada_Max,Voltaje_Entrada_Min,Potencia_Salida_Nominal,Potencia_Salida_Max,Numero_Fases,URL_Producto,Fecha_Extraccion
"Fronius","Primo 5.0-1","On-Grid",1000,80,5000,5000,1,"https://ejemplo.com.mx/inversor-5kw",2026-02-17 09:01:00
"Huawei","SUN2000-10KTL-M1","Híbrido",1100,200,10000,11000,3,"https://ejemplo.com.mx/inversor-10kw",2026-02-17 09:01:15
"SMA","Sunny Tripower 15000TL","On-Grid",1000,320,15000,15000,3,"https://ejemplo.com.mx/inversor-15kw",2026-02-17 09:01:30
```

## Ejemplo de Salida de Consola

```
╔═══════════════════════════════════════════════════════════╗
║                                                           ║
║          ☀️  SOLAR SCRAPPER MÉXICO - v1.0 ☀️              ║
║                                                           ║
║     Buscador de Paneles Solares e Inversores             ║
║     para el Mercado Mexicano                             ║
║                                                           ║
╚═══════════════════════════════════════════════════════════╝

🔍 Iniciando búsqueda de equipos solares en el mercado mexicano...

📍 Sitios web configurados: 3
   • SolarMex - https://www.solarmex.com.mx
   • Enlight - https://enlight.mx
   • SolarTech México - https://www.solartechmexico.com

🌐 Total de URLs a procesar:
   - URLs de paneles: 3
   - URLs de inversores: 3

============================================================
BUSCANDO PANELES SOLARES
============================================================

=== Buscando paneles en: https://www.solarmex.com.mx/paneles-solares ===
Cargando página: https://www.solarmex.com.mx/paneles-solares
Encontrados 15 paneles

=== Buscando paneles en: https://enlight.mx/tienda/paneles-solares ===
Cargando página: https://enlight.mx/tienda/paneles-solares
Encontrados 8 paneles

=== Buscando paneles en: https://www.solartechmexico.com/productos/paneles ===
Cargando página: https://www.solartechmexico.com/productos/paneles
Encontrados 12 paneles

============================================================
BUSCANDO INVERSORES
============================================================

=== Buscando inversores en: https://www.solarmex.com.mx/inversores ===
Cargando página: https://www.solarmex.com.mx/inversores
Encontrados 10 inversores

=== Buscando inversores en: https://enlight.mx/tienda/inversores ===
Cargando página: https://enlight.mx/tienda/inversores
Encontrados 6 inversores

=== Buscando inversores en: https://www.solartechmexico.com/productos/inversores ===
Cargando página: https://www.solartechmexico.com/productos/inversores
Encontrados 9 inversores

============================================================
RESUMEN DE DATOS EXTRAÍDOS
============================================================

📊 Total de productos encontrados: 60
   - Paneles solares: 35
   - Inversores: 25

☀️  PANELES SOLARES:
   • Canadian Solar HiKu CS3W-400MS - 400W - Monocristalino
   • Jinko Solar Tiger Pro 550W - 550W - PERC
   • Trina Solar Vertex S 450W - 450W - Monocristalino
   • LONGi Hi-MO 5 540W - 540W - Monocristalino
   • JA Solar JAM72S30 535W - 535W - PERC
   ... y 30 más

⚡ INVERSORES:
   • Fronius Primo 5.0-1 - 5000W - On-Grid
   • Huawei SUN2000-10KTL-M1 - 10000W - Híbrido
   • SMA Sunny Tripower 15000TL - 15000W - On-Grid
   • Growatt MIN 6000TL-X - 6000W - On-Grid
   • Goodwe GW8K-DT - 8000W - Híbrido
   ... y 20 más

============================================================

💾 Exportando datos...

✓ Datos exportados a JSON: solar_data_20260217_090530.json

✓ Datos exportados a CSV:
  - Paneles: solar_data_20260217_090530_paneles.csv
  - Inversores: solar_data_20260217_090530_inversores.csv

✅ Proceso completado exitosamente!

🔗 NOTA: Los sitios web configurados son ejemplos.
   Para obtener datos reales, actualiza las URLs en Config/MexicanSolarWebsites.cs
   con sitios web mexicanos reales que vendan equipos solares.
```

## Notas

- Los ejemplos mostrados arriba son ilustrativos y muestran cómo se verían los datos cuando se encuentran productos reales
- La aplicación actual está configurada con URLs de ejemplo que no resuelven
- Para obtener datos reales, configura URLs de sitios web mexicanos activos en `Config/MexicanSolarWebsites.cs`
- Los campos pueden estar vacíos (`null`) si la información no se encuentra en la página web
