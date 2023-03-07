namespace Business.Jobscheduler
{
    using Business.Configuration;
    using Business.WeeklyAverageUsDieselPrice.Managers;
    using FluentScheduler;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class JobSchedulerManager : IJobSchedulerManager
    {
        private readonly DataExtractionOptions dataExtractionOptions;
        private readonly IWeeklyAverageUsDieselPriceExtractionManager weeklyAverageUsDieselPriceExtractionManager;
        private readonly ILogger<JobSchedulerManager> logger;

        public JobSchedulerManager(
            IWeeklyAverageUsDieselPriceExtractionManager weeklyAverageUsDieselPriceExtractionManager,
            ILogger<JobSchedulerManager> logger,
            IOptions<DataExtractionOptions> options)
        {
            this.weeklyAverageUsDieselPriceExtractionManager = weeklyAverageUsDieselPriceExtractionManager;
            this.logger = logger;
            this.dataExtractionOptions = options.Value;
        }

        public void ScheduleJobs()
        {
            this.logger.LogInformation("Scheduling jobs");
            var registry = new Registry();

            registry.Schedule(async () => await this.weeklyAverageUsDieselPriceExtractionManager.ExtractData().ConfigureAwait(false))
                .ToRunNow()
                .AndEvery(dataExtractionOptions.FrequencyOfLoadingWeeklyAverageDieselPricesInDays)
                .Days();

            JobManager.Initialize(registry);
            this.logger.LogInformation("Finished scheduling jobs");
        }
    }
}