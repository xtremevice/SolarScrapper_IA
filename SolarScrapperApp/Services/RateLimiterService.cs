namespace SolarScrapperApp.Services;

public class RateLimiterService
{
    private readonly Dictionary<string, DateTime> _lastRequestByDomain = new Dictionary<string, DateTime>();
    private readonly int _minimumDelayMilliseconds;
    private readonly int _delayBetweenRequestsMilliseconds;
    private readonly object _lock = new object();
    
    public RateLimiterService(int minimumDelayMs = 2000, int delayBetweenRequestsMs = 3000)
    {
        _minimumDelayMilliseconds = minimumDelayMs;
        _delayBetweenRequestsMilliseconds = delayBetweenRequestsMs;
    }
    
    public async Task WaitIfNeededAsync(string url)
    {
        var domain = ExtractDomain(url);
        
        lock (_lock)
        {
            if (_lastRequestByDomain.TryGetValue(domain, out DateTime lastRequest))
            {
                var timeSinceLastRequest = DateTime.Now - lastRequest;
                var requiredDelay = TimeSpan.FromMilliseconds(_delayBetweenRequestsMilliseconds);
                
                if (timeSinceLastRequest < requiredDelay)
                {
                    var remainingDelay = requiredDelay - timeSinceLastRequest;
                    Console.WriteLine($"⏱️  Esperando {remainingDelay.TotalSeconds:F1}s antes de solicitar {domain}...");
                }
            }
        }
        
        // Esperar el mínimo requerido
        await Task.Delay(_minimumDelayMilliseconds);
        
        lock (_lock)
        {
            if (_lastRequestByDomain.TryGetValue(domain, out DateTime lastRequest))
            {
                var timeSinceLastRequest = DateTime.Now - lastRequest;
                var requiredDelay = TimeSpan.FromMilliseconds(_delayBetweenRequestsMilliseconds);
                
                if (timeSinceLastRequest < requiredDelay)
                {
                    var remainingDelay = (requiredDelay - timeSinceLastRequest).TotalMilliseconds;
                    if (remainingDelay > 0)
                    {
                        Task.Delay((int)remainingDelay).Wait();
                    }
                }
            }
            
            _lastRequestByDomain[domain] = DateTime.Now;
        }
    }
    
    private string ExtractDomain(string url)
    {
        try
        {
            var uri = new Uri(url);
            return uri.Host;
        }
        catch
        {
            return url;
        }
    }
    
    public void Reset()
    {
        lock (_lock)
        {
            _lastRequestByDomain.Clear();
        }
    }
}
