using Panpipe.Sheduler.Job;
using Quartz;
using Quartz.Impl;

namespace Panpipe.Sheduler;

public static class QuartzSchedulerExtension
{
    private const int DailyExecutionDays = 1;
    
    public static IServiceCollection AddQuartzSchedulerExtension(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            Guid guidJob = Guid.NewGuid();
            JobKey jobKey = new($"{nameof(SetMarkHabitsAsyncJob)}-{guidJob}");
            
            q.AddJob<SetMarkHabitsAsyncJob>(opts => opts.WithIdentity(jobKey));
            
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity($"{jobKey.Name}-trigger")
                .StartNow()
                .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromSeconds(10)).RepeatForever())
            );
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }
}