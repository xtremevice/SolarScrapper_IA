# Detector de Palabras Clave - Documentación

## Descripción

El sistema de detección de palabras clave (KeywordDetectorService) es una funcionalidad inteligente que identifica automáticamente las secciones relevantes de las páginas web que contienen información sobre paneles solares e inversores.

## ¿Cómo Funciona?

### Estrategia de Búsqueda

1. **Análisis de Contenido**: Examina todo el HTML de la página
2. **Identificación de Palabras Clave**: Busca términos específicos relacionados con equipos solares
3. **Puntuación de Relevancia**: Asigna una puntuación a cada sección según el número de palabras clave encontradas
4. **Selección Inteligente**: Prioriza las secciones con mayor puntuación de relevancia

### Ventajas

- ✅ **Más Preciso**: Encuentra información incluso en sitios con estructura HTML compleja
- ✅ **Adaptable**: No depende de selectores CSS específicos de cada sitio
- ✅ **Eficiente**: Reduce el ruido al filtrar secciones irrelevantes
- ✅ **Multilenguaje**: Incluye términos en español e inglés

## Palabras Clave Utilizadas

### Para Paneles Solares

```
Palabras clave principales:
- panel solar, panel fotovoltaico
- módulo solar, módulo fotovoltaico
- celda solar, placa solar
- monocristalino, policristalino
- perc, bifacial

Especificaciones técnicas:
- watts, potencia
- voltaje, amperaje
- eficiencia
- wp (watt peak)
- voc, isc, vmp, imp
```

### Para Inversores

```
Palabras clave principales:
- inversor, inverter
- convertidor
- on-grid, off-grid, híbrido
- interconectado, aislado
- grid-tied

Características:
- monofásico, trifásico
- mppt
- potencia nominal
- voltaje entrada, voltaje salida
- corriente máxima
- frecuencia
- kw, kilowatt
```

### Para Especificaciones Técnicas

```
Términos generales:
- especificaciones, características
- ficha técnica, datasheet
- datos técnicos
- specifications, technical data
- performance, rendimiento
- garantía, warranty
- certificación, norma
```

## Puntuación de Relevancia

El sistema asigna puntos a cada sección HTML según:

- **+1 punto** por cada palabra clave de producto encontrada
- **+1 punto** por cada término técnico encontrado
- **Bonus** por combinación de palabras clave

Ejemplo de puntuación alta:
```
Texto: "Panel Solar Monocristalino 450W - Especificaciones Técnicas
Voltaje: 48.5V, Corriente: 10.2A, Eficiencia: 21.5%"

Puntuación: 8 puntos
- panel solar (1)
- monocristalino (1)
- 450W/watts (1)
- especificaciones técnicas (1)
- voltaje (1)
- corriente/amperaje (1)
- eficiencia (1)
```

## Uso en el Código

### Integración Automática

El KeywordDetectorService ya está integrado en los scrapers. No necesitas modificar nada para usarlo.

### Personalización de Palabras Clave

Si necesitas agregar más palabras clave, edita el archivo:
`Services/KeywordDetectorService.cs`

```csharp
// Agregar palabras clave para paneles
private readonly List<string> _panelKeywords = new List<string>
{
    "panel solar",
    "tu nueva palabra clave",
    // ... más palabras
};
```

## Proceso de Extracción Mejorado

### Antes (sin detector de palabras clave):

1. Buscar con selectores CSS genéricos
2. Si no encuentra nada, buscar patrones en todo el texto
3. Puede capturar información irrelevante

### Ahora (con detector de palabras clave):

1. **Analizar con palabras clave** - Encuentra secciones relevantes
2. **Puntuación de relevancia** - Ordena por importancia
3. **Extracción focalizada** - Extrae datos de las mejores secciones
4. **Fallback inteligente** - Si no encuentra suficiente, usa selectores genéricos
5. **Última opción** - Búsqueda de patrones en todo el texto

## Ejemplos de Funcionamiento

### Ejemplo 1: Sitio de E-commerce

```html
<div class="product-card">
  <h3>Panel Solar Monocristalino 550W</h3>
  <div class="specs">
    <p>Voltaje máximo: 49.5V</p>
    <p>Corriente máxima: 13.2A</p>
    <p>Eficiencia: 21.2%</p>
  </div>
</div>
```

**Detección:**
- ✅ Palabras clave encontradas: panel solar, monocristalino, voltaje, corriente, eficiencia
- 📊 Puntuación: 6 puntos
- ✅ Extracción exitosa

### Ejemplo 2: Blog/Artículo

```html
<article>
  <h2>Los mejores paneles solares del mercado</h2>
  <p>Te presentamos el nuevo panel fotovoltaico de 450W con 
     tecnología PERC y eficiencia del 20.8%...</p>
</article>
```

**Detección:**
- ✅ Palabras clave: paneles solares, panel fotovoltaico, PERC, eficiencia
- 📊 Puntuación: 5 puntos
- ✅ Extracción exitosa

### Ejemplo 3: Contenido Irrelevante

```html
<div class="footer">
  <p>Contacto: info@ejemplo.com</p>
  <p>Términos y condiciones</p>
</div>
```

**Detección:**
- ❌ Sin palabras clave relevantes
- 📊 Puntuación: 0 puntos
- ❌ Sección descartada

## Configuración Avanzada

### Ajustar el Número de Nodos Analizados

En `Scrapers/SolarPanelScraper.cs` o `Scrapers/InverterScraper.cs`:

```csharp
// Analizar hasta 20 nodos más relevantes (valor por defecto)
var topNodes = _keywordDetector.GetMostRelevantNodes(relevantNodes, isPanel: true, maxNodes: 20);

// Para analizar más nodos (más lento pero más exhaustivo)
var topNodes = _keywordDetector.GetMostRelevantNodes(relevantNodes, isPanel: true, maxNodes: 50);

// Para analizar menos nodos (más rápido pero menos exhaustivo)
var topNodes = _keywordDetector.GetMostRelevantNodes(relevantNodes, isPanel: true, maxNodes: 10);
```

### Verificar Detección en Tiempo Real

Durante la ejecución, verás mensajes como:

```
🔍 Encontrados 45 nodos con palabras clave de paneles
🔍 Encontrados 23 nodos con palabras clave de inversores
```

Esto indica que el detector está funcionando correctamente.

## Ventajas sobre Métodos Tradicionales

| Característica | Sin Keywords | Con Keywords |
|---------------|--------------|--------------|
| Precisión | Media | Alta |
| Adaptabilidad | Baja | Alta |
| Velocidad | Rápida | Media |
| Falsos Positivos | Muchos | Pocos |
| Mantenimiento | Alto | Bajo |

## Mejores Prácticas

1. **Mantener Actualizada la Lista**: Agrega nuevas palabras clave cuando encuentres términos comunes
2. **Balancear Especificidad**: No agregues palabras demasiado genéricas ("producto", "precio")
3. **Incluir Variaciones**: Agrega singular, plural, y diferentes escrituras
4. **Considerar el Idioma**: Incluye términos en inglés si scrapeeas sitios internacionales

## Limitaciones

- No detecta imágenes o contenido JavaScript dinámico
- Puede requerir ajustes para sitios muy específicos
- La puntuación es simple (puede mejorarse con IA/ML)

## Futuras Mejoras

- [ ] Machine Learning para mejorar la puntuación
- [ ] Detección de sinónimos automática
- [ ] Análisis de contexto (palabras cercanas)
- [ ] Soporte para más idiomas
- [ ] Cache de palabras clave exitosas por sitio
