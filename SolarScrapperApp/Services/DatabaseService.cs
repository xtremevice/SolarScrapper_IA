using MySqlConnector;
using SolarScrapperApp.Config;
using SolarScrapperApp.Models;

namespace SolarScrapperApp.Services;

public class DatabaseService
{
    private readonly string _connectionString;
    
    public DatabaseService(DatabaseConfig config)
    {
        _connectionString = config.GetConnectionString();
    }
    
    public async Task InitializeDatabaseAsync()
    {
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            
            // Crear tabla de paneles solares
            var createPanelsTable = @"
                CREATE TABLE IF NOT EXISTS paneles_solares (
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
                    UNIQUE KEY unique_panel (marca, modelo, watts_maximo),
                    INDEX idx_marca (marca),
                    INDEX idx_watts (watts_maximo)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
            ";
            
            using (var cmd = new MySqlCommand(createPanelsTable, connection))
            {
                await cmd.ExecuteNonQueryAsync();
            }
            
            // Crear tabla de inversores
            var createInvertersTable = @"
                CREATE TABLE IF NOT EXISTS inversores (
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
                    UNIQUE KEY unique_inverter (marca, modelo, potencia_salida_nominal),
                    INDEX idx_marca (marca),
                    INDEX idx_potencia (potencia_salida_nominal)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
            ";
            
            using (var cmd = new MySqlCommand(createInvertersTable, connection))
            {
                await cmd.ExecuteNonQueryAsync();
            }
            
            Console.WriteLine("✓ Base de datos inicializada correctamente");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error inicializando base de datos: {ex.Message}");
            throw;
        }
    }
    
    public async Task<bool> PanelExistsAsync(SolarPanel panel)
    {
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            
            var query = @"
                SELECT COUNT(*) FROM paneles_solares 
                WHERE marca = @marca AND modelo = @modelo AND watts_maximo = @watts
            ";
            
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@marca", panel.Marca ?? "");
            cmd.Parameters.AddWithValue("@modelo", panel.Modelo ?? "");
            cmd.Parameters.AddWithValue("@watts", panel.WattsMaximo ?? 0);
            
            var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return count > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error verificando panel duplicado: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> InverterExistsAsync(Inverter inverter)
    {
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            
            var query = @"
                SELECT COUNT(*) FROM inversores 
                WHERE marca = @marca AND modelo = @modelo AND potencia_salida_nominal = @potencia
            ";
            
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@marca", inverter.Marca ?? "");
            cmd.Parameters.AddWithValue("@modelo", inverter.Modelo ?? "");
            cmd.Parameters.AddWithValue("@potencia", inverter.PotenciaSalidaNominal ?? 0);
            
            var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return count > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error verificando inversor duplicado: {ex.Message}");
            return false;
        }
    }
    
    public async Task<int> SavePanelAsync(SolarPanel panel)
    {
        try
        {
            // Verificar si ya existe
            if (await PanelExistsAsync(panel))
            {
                Console.WriteLine($"⚠️  Panel duplicado: {panel.Marca} {panel.Modelo} - No se guardará");
                return 0;
            }
            
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            
            var query = @"
                INSERT INTO paneles_solares (
                    marca, modelo, tipo_panel,
                    voltaje_maximo, voltaje_promedio,
                    amperaje_maximo, amperaje_promedio,
                    watts_maximo, watts_promedio,
                    eficiencia, dimensiones, peso, garantia,
                    url_producto, url_ficha_tecnica, fecha_extraccion
                ) VALUES (
                    @marca, @modelo, @tipo_panel,
                    @voltaje_max, @voltaje_prom,
                    @amperaje_max, @amperaje_prom,
                    @watts_max, @watts_prom,
                    @eficiencia, @dimensiones, @peso, @garantia,
                    @url_producto, @url_ficha, @fecha_extraccion
                )
            ";
            
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@marca", panel.Marca ?? "");
            cmd.Parameters.AddWithValue("@modelo", panel.Modelo ?? "");
            cmd.Parameters.AddWithValue("@tipo_panel", panel.TipoPanel ?? "");
            cmd.Parameters.AddWithValue("@voltaje_max", panel.VoltajeMaximo.HasValue ? (object)panel.VoltajeMaximo.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@voltaje_prom", panel.VoltajePromedio.HasValue ? (object)panel.VoltajePromedio.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@amperaje_max", panel.AmperajeMaximo.HasValue ? (object)panel.AmperajeMaximo.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@amperaje_prom", panel.AmperajePromedio.HasValue ? (object)panel.AmperajePromedio.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@watts_max", panel.WattsMaximo.HasValue ? (object)panel.WattsMaximo.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@watts_prom", panel.WattsPromedio.HasValue ? (object)panel.WattsPromedio.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@eficiencia", panel.Eficiencia ?? "");
            cmd.Parameters.AddWithValue("@dimensiones", panel.Dimensiones ?? "");
            cmd.Parameters.AddWithValue("@peso", panel.Peso ?? "");
            cmd.Parameters.AddWithValue("@garantia", panel.Garantia ?? "");
            cmd.Parameters.AddWithValue("@url_producto", panel.UrlProducto ?? "");
            cmd.Parameters.AddWithValue("@url_ficha", panel.UrlFichaTecnica ?? "");
            cmd.Parameters.AddWithValue("@fecha_extraccion", panel.FechaExtraccion);
            
            return await cmd.ExecuteNonQueryAsync();
        }
        catch (MySqlException ex) when (ex.Number == 1062) // Duplicate entry
        {
            Console.WriteLine($"⚠️  Panel duplicado detectado por clave única: {panel.Marca} {panel.Modelo}");
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error guardando panel: {ex.Message}");
            return 0;
        }
    }
    
    public async Task<int> SaveInverterAsync(Inverter inverter)
    {
        try
        {
            // Verificar si ya existe
            if (await InverterExistsAsync(inverter))
            {
                Console.WriteLine($"⚠️  Inversor duplicado: {inverter.Marca} {inverter.Modelo} - No se guardará");
                return 0;
            }
            
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            
            var query = @"
                INSERT INTO inversores (
                    marca, modelo, tipo_inversor,
                    voltaje_entrada_maximo, voltaje_entrada_minimo,
                    corriente_entrada_maxima, potencia_entrada_maxima,
                    voltaje_salida_nominal, potencia_salida_nominal,
                    potencia_salida_maxima, frecuencia_salida,
                    eficiencia, dimensiones, peso, garantia, numero_fases,
                    url_producto, url_ficha_tecnica, fecha_extraccion
                ) VALUES (
                    @marca, @modelo, @tipo_inversor,
                    @voltaje_ent_max, @voltaje_ent_min,
                    @corriente_ent_max, @potencia_ent_max,
                    @voltaje_sal_nom, @potencia_sal_nom,
                    @potencia_sal_max, @frecuencia_sal,
                    @eficiencia, @dimensiones, @peso, @garantia, @numero_fases,
                    @url_producto, @url_ficha, @fecha_extraccion
                )
            ";
            
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@marca", inverter.Marca ?? "");
            cmd.Parameters.AddWithValue("@modelo", inverter.Modelo ?? "");
            cmd.Parameters.AddWithValue("@tipo_inversor", inverter.TipoInversor ?? "");
            cmd.Parameters.AddWithValue("@voltaje_ent_max", inverter.VoltajeEntradaMaximo.HasValue ? (object)inverter.VoltajeEntradaMaximo.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@voltaje_ent_min", inverter.VoltajeEntradaMinimo.HasValue ? (object)inverter.VoltajeEntradaMinimo.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@corriente_ent_max", inverter.CorrienteEntradaMaxima.HasValue ? (object)inverter.CorrienteEntradaMaxima.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@potencia_ent_max", inverter.PotenciaEntradaMaxima.HasValue ? (object)inverter.PotenciaEntradaMaxima.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@voltaje_sal_nom", inverter.VoltajeSalidaNominal.HasValue ? (object)inverter.VoltajeSalidaNominal.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@potencia_sal_nom", inverter.PotenciaSalidaNominal.HasValue ? (object)inverter.PotenciaSalidaNominal.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@potencia_sal_max", inverter.PotenciaSalidaMaxima.HasValue ? (object)inverter.PotenciaSalidaMaxima.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@frecuencia_sal", inverter.FrecuenciaSalida.HasValue ? (object)inverter.FrecuenciaSalida.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@eficiencia", inverter.Eficiencia ?? "");
            cmd.Parameters.AddWithValue("@dimensiones", inverter.Dimensiones ?? "");
            cmd.Parameters.AddWithValue("@peso", inverter.Peso ?? "");
            cmd.Parameters.AddWithValue("@garantia", inverter.Garantia ?? "");
            cmd.Parameters.AddWithValue("@numero_fases", inverter.NumeroFases.HasValue ? (object)inverter.NumeroFases.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@url_producto", inverter.UrlProducto ?? "");
            cmd.Parameters.AddWithValue("@url_ficha", inverter.UrlFichaTecnica ?? "");
            cmd.Parameters.AddWithValue("@fecha_extraccion", inverter.FechaExtraccion);
            
            return await cmd.ExecuteNonQueryAsync();
        }
        catch (MySqlException ex) when (ex.Number == 1062) // Duplicate entry
        {
            Console.WriteLine($"⚠️  Inversor duplicado detectado por clave única: {inverter.Marca} {inverter.Modelo}");
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error guardando inversor: {ex.Message}");
            return 0;
        }
    }
    
    public async Task<int> SavePanelsAsync(List<SolarPanel> panels)
    {
        int savedCount = 0;
        foreach (var panel in panels)
        {
            var result = await SavePanelAsync(panel);
            if (result > 0)
                savedCount++;
        }
        return savedCount;
    }
    
    public async Task<int> SaveInvertersAsync(List<Inverter> inverters)
    {
        int savedCount = 0;
        foreach (var inverter in inverters)
        {
            var result = await SaveInverterAsync(inverter);
            if (result > 0)
                savedCount++;
        }
        return savedCount;
    }
    
    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            Console.WriteLine("✓ Conexión a la base de datos exitosa");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error conectando a la base de datos: {ex.Message}");
            return false;
        }
    }
}
