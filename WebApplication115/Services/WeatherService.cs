using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "dc7817ffdcb43207f35bfdc10ccca76f";

    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetWeatherAsync(string region)
    {
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={region}&appid={_apiKey}&units=metric";
        var response = await _httpClient.GetFromJsonAsync<WeatherResponse>(url);

        return response != null ?
               $"Temp: {response.Main.Temp}°C, Value: {response.Weather[0].Description}" :
               "No rechable information";
    }
}

public class WeatherResponse
{
    public Main Main { get; set; }
    public Weather[] Weather { get; set; }
}

public class Main
{
    public float Temp { get; set; }
}

public class Weather
{
    public string Description { get; set; }
}
