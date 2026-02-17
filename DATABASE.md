# Base de Datos MySQL - Configuración y Uso

## Características de la Base de Datos

La aplicación ahora incluye almacenamiento en base de datos MySQL con las siguientes características:

### ✅ Funcionalidades

1. **Detección de Duplicados**: Verifica automáticamente si un producto ya existe antes de guardarlo
2. **Claves Únicas**: Usa combinaciones de marca, modelo y potencia para identificar duplicados
3. **Persistencia**: Los datos se almacenan permanentemente en MySQL
4. **Configuración Flexible**: Puede usar variables de entorno o valores por defecto

## Configuración de la Base de Datos

### Opción 1: Variables de Entorno

Configure las siguientes variables de entorno:

```bash
export DB_SERVER="localhost"
export DB_PORT="3306"
export DB_NAME="solar_scraper_db"
export DB_USER="root"
export DB_PASSWORD="tu_password"
```

### Opción 2: Modificar DatabaseConfig.cs

Edite el archivo `Config/DatabaseConfig.cs` para cambiar los valores por defecto.

## Estructura de Tablas

### Tabla: paneles_solares

```sql
CREATE TABLE paneles_solares (
    id INT AUTO_INCREMENT PRIMARY KEY,
    marca VARCHAR(100),
    modelo VARCHAR(200),
    tipo_panel VARCHAR(50),
    voltaje_maximo DECIMAL(10,2),
    voltaje_promedio DECIMAL(10,2),
    amperaje_maximo DECIMAL(10,2),
    amperaje_promedio DECIMAL(10,2),
    watts_maximo DECIMAL(10,2),
    watts_promedio DECIMAL(10,2),
    eficiencia VARCHAR(20),
    dimensiones VARCHAR(100),
    peso VARCHAR(50),
    garantia VARCHAR(100),
    url_producto TEXT,
    url_ficha_tecnica TEXT,
    fecha_extraccion DATETIME,
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY unique_panel (marca, modelo, watts_maximo)
);
```

### Tabla: inversores

```sql
CREATE TABLE inversores (
    id INT AUTO_INCREMENT PRIMARY KEY,
    marca VARCHAR(100),
    modelo VARCHAR(200),
    tipo_inversor VARCHAR(50),
    voltaje_entrada_maximo DECIMAL(10,2),
    voltaje_entrada_minimo DECIMAL(10,2),
    corriente_entrada_maxima DECIMAL(10,2),
    potencia_entrada_maxima DECIMAL(10,2),
    voltaje_salida_nominal DECIMAL(10,2),
    potencia_salida_nominal DECIMAL(10,2),
    potencia_salida_maxima DECIMAL(10,2),
    frecuencia_salida DECIMAL(10,2),
    eficiencia VARCHAR(20),
    dimensiones VARCHAR(100),
    peso VARCHAR(50),
    garantia VARCHAR(100),
    numero_fases INT,
    url_producto TEXT,
    url_ficha_tecnica TEXT,
    fecha_extraccion DATETIME,
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY unique_inverter (marca, modelo, potencia_salida_nominal)
);
```

## Instalación de MySQL

### Windows

1. Descarga MySQL Community Server desde [mysql.com](https://dev.mysql.com/downloads/mysql/)
2. Ejecuta el instalador y sigue las instrucciones
3. Durante la instalación, configura la contraseña de root

### Linux (Ubuntu/Debian)

```bash
sudo apt update
sudo apt install mysql-server
sudo mysql_secure_installation
```

### macOS

```bash
brew install mysql
brew services start mysql
mysql_secure_installation
```

## Crear la Base de Datos

Una vez instalado MySQL, crea la base de datos:

```bash
mysql -u root -p
```

Luego ejecuta:

```sql
CREATE DATABASE solar_scraper_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE solar_scraper_db;
```

La aplicación creará automáticamente las tablas la primera vez que se ejecute.

## Uso de la Aplicación

### Con Base de Datos

Cuando ejecutes la aplicación, se te preguntará si deseas usar la base de datos:

```
❓ ¿Desea usar almacenamiento en base de datos MySQL? (s/n)
```

- Presiona **Enter** o escribe **s** para usar la base de datos
- Escribe **n** para solo usar archivos JSON/CSV

### Sin Base de Datos

Si no tienes MySQL instalado o no deseas usarlo, simplemente responde "n" y la aplicación funcionará normalmente guardando solo en archivos.

## Consultas Útiles

### Ver todos los paneles

```sql
SELECT marca, modelo, watts_maximo, tipo_panel 
FROM paneles_solares 
ORDER BY watts_maximo DESC;
```

### Ver todos los inversores

```sql
SELECT marca, modelo, potencia_salida_nominal, tipo_inversor 
FROM inversores 
ORDER BY potencia_salida_nominal DESC;
```

### Contar productos por marca

```sql
-- Paneles por marca
SELECT marca, COUNT(*) as total 
FROM paneles_solares 
GROUP BY marca 
ORDER BY total DESC;

-- Inversores por marca
SELECT marca, COUNT(*) as total 
FROM inversores 
GROUP BY marca 
ORDER BY total DESC;
```

### Buscar paneles por potencia

```sql
SELECT marca, modelo, watts_maximo, url_producto 
FROM paneles_solares 
WHERE watts_maximo >= 400 AND watts_maximo <= 600;
```

## Detección de Duplicados

La aplicación detecta duplicados usando dos métodos:

1. **Verificación Previa**: Antes de insertar, consulta si ya existe un producto con la misma marca, modelo y potencia
2. **Clave Única**: Si la verificación falla, la base de datos rechaza duplicados por la restricción UNIQUE KEY

Cuando se detecta un duplicado:
- Se muestra un mensaje de advertencia
- El producto **no se guarda** nuevamente
- El contador de productos guardados no se incrementa
- El proceso continúa con el siguiente producto

## Mantenimiento

### Backup de la Base de Datos

```bash
mysqldump -u root -p solar_scraper_db > backup_solar_$(date +%Y%m%d).sql
```

### Restaurar desde Backup

```bash
mysql -u root -p solar_scraper_db < backup_solar_20240101.sql
```

### Limpiar datos antiguos

```sql
-- Eliminar paneles más antiguos de 6 meses
DELETE FROM paneles_solares 
WHERE fecha_creacion < DATE_SUB(NOW(), INTERVAL 6 MONTH);

-- Eliminar inversores más antiguos de 6 meses
DELETE FROM inversores 
WHERE fecha_creacion < DATE_SUB(NOW(), INTERVAL 6 MONTH);
```

## Solución de Problemas

### Error: "Can't connect to MySQL server"

- Verifica que MySQL esté ejecutándose: `sudo systemctl status mysql` (Linux)
- Verifica el servidor y puerto en la configuración
- Verifica las credenciales de usuario y contraseña

### Error: "Access denied for user"

- Verifica el usuario y contraseña
- Asegúrate de que el usuario tenga permisos: 
  ```sql
  GRANT ALL PRIVILEGES ON solar_scraper_db.* TO 'root'@'localhost';
  FLUSH PRIVILEGES;
  ```

### Error: "Unknown database"

- Crea la base de datos manualmente:
  ```sql
  CREATE DATABASE solar_scraper_db;
  ```

### Error: "Duplicate entry"

- Esto es normal, significa que el producto ya existe
- La aplicación maneja esto automáticamente y continúa
