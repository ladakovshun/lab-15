using Microsoft.EntityFrameworkCore;

public class EmailNotificationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EmailNotificationService> _logger;

    public EmailNotificationService(
        IServiceProvider serviceProvider,
        ILogger<EmailNotificationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();

                var recentRecords = await dbContext.Records
                    .Where(r => r.CreatedAt >= DateTime.UtcNow.AddMinutes(-1))
                    .ToListAsync(stoppingToken);

                foreach (var record in recentRecords)
                {
                    emailService.SendEmail(
                        "Element added",
                        $"New value has been added to db: {record.Name} (ID: {record.Id})"
                    );

                    _logger.LogInformation($"Email sent for record ID: {record.Id}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking for new records.");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
