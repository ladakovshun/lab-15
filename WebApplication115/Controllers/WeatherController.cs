using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

public class WeatherController : ControllerBase
{
    private readonly IMemoryCache _memoryCache;

    public WeatherController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    [HttpGet]
    public IActionResult GetWeather()
    {
        if (_memoryCache.TryGetValue("KyivWeather", out string weatherData))
        {
            return Ok(weatherData);
        }

        return NotFound("Weather data is not available.");
    }
}
