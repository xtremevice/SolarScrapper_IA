namespace SolarScrapperApp.Config;

public class DatabaseConfig
{
    public string Server { get; set; } = "localhost";
    public int Port { get; set; } = 3306;
    public string Database { get; set; } = "solar_scraper_db";
    public string UserId { get; set; } = "root";
    public string Password { get; set; } = "";
    
    public string GetConnectionString()
    {
        return $"Server={Server};Port={Port};Database={Database};Uid={UserId};Pwd={Password};";
    }
    
    // Valores por defecto para desarrollo
    public static DatabaseConfig GetDefault()
    {
        return new DatabaseConfig
        {
            Server = Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost",
            Port = int.TryParse(Environment.GetEnvironmentVariable("DB_PORT"), out int port) ? port : 3306,
            Database = Environment.GetEnvironmentVariable("DB_NAME") ?? "solar_scraper_db",
            UserId = Environment.GetEnvironmentVariable("DB_USER") ?? "root",
            Password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? ""
        };
    }
}
