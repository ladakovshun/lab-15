using Microsoft.Extensions.Caching.Memory;

public class WeatherBackgroundService : BackgroundService
{
    private readonly WeatherService _weatherService;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<WeatherBackgroundService> _logger;

    public WeatherBackgroundService(
        WeatherService weatherService,
        IMemoryCache memoryCache,
        ILogger<WeatherBackgroundService> logger)
    {
        _weatherService = weatherService;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var weatherData = await _weatherService.GetWeatherAsync("Mykolaiv");

                _memoryCache.Set("MykolaivWeather", weatherData, TimeSpan.FromMinutes(10));

                _logger.LogInformation($"Weather updated: {weatherData}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating weather data.");
            }

            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
        }
    }
}
