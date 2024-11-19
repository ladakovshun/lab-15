using Quartz;

public class EmailJob : IJob
{
    private readonly ILogger<EmailJob> _logger;
    private readonly EmailService _emailService;

    public EmailJob(ILogger<EmailJob> logger, EmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Executing EmailJob at {Time}", DateTime.Now);

        try
        {
            _emailService.SendEmail(
                "Заплановане повідомлення",
                "Повідомлення Quartz.NET за певний час."
            );

            _logger.LogInformation("Email sent successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while sending email.");
        }

        return Task.CompletedTask;
    }
}
