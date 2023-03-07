namespace Business.WeeklyAverageUsDieselPrice.Managers
{
    using Business.Common.Managers;
    using Business.Configuration;
    using Business.WeeklyAverageUsDieselPrice.Models;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public class WeeklyAverageUsDieselPriceExtractionManager : IWeeklyAverageUsDieselPriceExtractionManager
    {
        private IEiaIntegrationManager eiaIntegrationManager;
        private IWeeklyAverageUsDieselPriceCreateManager weeklyAverageUsDieselPriceCreateManager;
        private IWeeklyAverageUsDieselPriceQueryManager weeklyAverageDieselPriceQueryManager;
        private ILogger<WeeklyAverageUsDieselPriceExtractionManager> logger;
        private readonly DataExtractionOptions dataExtractionOptions;
        
        public WeeklyAverageUsDieselPriceExtractionManager(
            IEiaIntegrationManager eiaIntegrationManager,
            IWeeklyAverageUsDieselPriceCreateManager weeklyAverageUsDieselPriceCreateManager,
            IWeeklyAverageUsDieselPriceQueryManager weeklyAverageDieselPriceQueryManager,
            ILogger<WeeklyAverageUsDieselPriceExtractionManager> logger,
            IOptions<DataExtractionOptions> options)
        {
            this.eiaIntegrationManager = eiaIntegrationManager;
            this.weeklyAverageUsDieselPriceCreateManager = weeklyAverageUsDieselPriceCreateManager;
            this.weeklyAverageDieselPriceQueryManager = weeklyAverageDieselPriceQueryManager;
            this.logger = logger;
            this.dataExtractionOptions = options.Value;
        }

        public async Task ExtractData()
        {
            this.logger.LogInformation("Starting extraction of weekly average us diesel prices");
            var weeklyAverageUsDieselPrices = await this.eiaIntegrationManager.GetWeeklyAverageUsDieselPrices();
            var cutOffDate = DateOnly.FromDateTime(DateTime.Now).AddDays(this.dataExtractionOptions.MaximumNumberOfDaysToGoBack * -1);
            if (!weeklyAverageUsDieselPrices.IsNullOrEmpty())
            {
                var weeklyAverageDieselPricesByDate = this.weeklyAverageDieselPriceQueryManager
                    .GetWeeklyAverageDieselPrices()
                    .ToDictionary(x => x.WeekOf);

                var weeklyAverageDieselPriceCreateModels = weeklyAverageUsDieselPrices
                    .Where(x => x.Date >= cutOffDate &&
                        !weeklyAverageDieselPricesByDate.TryGetValue(x.Date, out WeeklyAverageUsDieselPriceReadModel _))
                    .Select(x => new WeeklyAverageUsDieselPriceCreateModel
                    {
                        WeekOf = x.Date,
                        AveragePrice = x.Price
                    }).ToList();

                if (!weeklyAverageDieselPriceCreateModels.IsNullOrEmpty())
                {
                    await this.weeklyAverageUsDieselPriceCreateManager.CreateWeeklyAverageUsDieselPricesAsync(weeklyAverageDieselPriceCreateModels).ConfigureAwait(false);
                }
                else
                {
                    this.logger.LogWarning("No new weekly average us diesel price to add to database");
                }
                
            }

            this.logger.LogInformation("Finished extraction of weekly average us diesel prices");
        }
    }
}
