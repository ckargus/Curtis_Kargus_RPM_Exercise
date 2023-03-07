namespace Business.WeeklyAverageUsDieselPrice.Managers
{
    using Business.WeeklyAverageUsDieselPrice.Models;
    using Business.WeeklyAverageUsDieselPrice.Validators;
    using DataAccess;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    public class WeeklyAverageUsDieselPriceCreateManager : IWeeklyAverageUsDieselPriceCreateManager
    {
        private readonly IWeeklyAverageUsDieselPriceCreateValidationManager createModelValidationManager;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ILogger<WeeklyAverageUsDieselPriceCreateManager> logger;

        public WeeklyAverageUsDieselPriceCreateManager(
            IWeeklyAverageUsDieselPriceCreateValidationManager createModelValidationManager,
            ApplicationDbContext applicationDbContext,
            ILogger<WeeklyAverageUsDieselPriceCreateManager> logger)
        {
            this.createModelValidationManager = createModelValidationManager;
            this.applicationDbContext = applicationDbContext;
            this.logger = logger;

        }

        public async Task CreateWeeklyAverageUsDieselPricesAsync(IList<WeeklyAverageUsDieselPriceCreateModel> createModels)
        {
            this.logger.LogInformation("Adding weekly average us diesel prices to database");
            var validationResult = await this.createModelValidationManager.PerformValidation(createModels).ConfigureAwait(false);
            if(validationResult.isValid && !validationResult.entities.IsNullOrEmpty())
            {
                this.applicationDbContext.AddRange(validationResult.entities);
                try
                {
                    await this.applicationDbContext.SaveChangesAsync().ConfigureAwait(false);
                    this.logger.LogInformation("Finished adding weekly average us diesel prices to database");
                }
                catch(Exception ex)
                {
                    this.logger.LogError(ex, $"Exception occurred in {nameof(this.CreateWeeklyAverageUsDieselPricesAsync)}");
                    throw;
                }
            }

        }
    }
}
