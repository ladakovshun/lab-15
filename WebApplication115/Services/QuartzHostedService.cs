using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;

public class QuartzHostedService : IHostedService
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IJobFactory _jobFactory;
    private readonly ILogger<QuartzHostedService> _logger;
    private IScheduler _scheduler;

    public QuartzHostedService(ISchedulerFactory schedulerFactory, IJobFactory jobFactory, ILogger<QuartzHostedService> logger)
    {
        _schedulerFactory = schedulerFactory;
        _jobFactory = jobFactory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
        _scheduler.JobFactory = _jobFactory;

        var job = JobBuilder.Create<EmailJob>().Build();
        var trigger = TriggerBuilder.Create()
            .WithIdentity("EmailJobTrigger")
            .StartNow()
            .WithSimpleSchedule(x => x.WithIntervalInMinutes(30).RepeatForever())
            .Build();

        await _scheduler.ScheduleJob(job, trigger, cancellationToken);
        await _scheduler.Start(cancellationToken);

        _logger.LogInformation("Quartz Hosted Service started.");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _scheduler?.Shutdown(cancellationToken);
        _logger.LogInformation("Quartz Hosted Service stopped.");
    }
}