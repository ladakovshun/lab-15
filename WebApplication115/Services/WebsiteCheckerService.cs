using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class WebsiteCheckerService : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<WebsiteCheckerService> _logger;

    public WebsiteCheckerService(IHttpClientFactory httpClientFactory, ILogger<WebsiteCheckerService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.GetAsync("https://laba.com/", stoppingToken);
                _logger.LogInformation("Website status: {StatusCode}", response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error checking website: {Message}", ex.Message);
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
