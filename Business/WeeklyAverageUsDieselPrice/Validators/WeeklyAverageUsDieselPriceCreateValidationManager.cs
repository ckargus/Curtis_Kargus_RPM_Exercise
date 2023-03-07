namespace Business.WeeklyAverageUsDieselPrice.Validators
{
    using Business.Common.Validation;
    using Business.WeeklyAverageUsDieselPrice.Models;
    using DataAccess;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    public class WeeklyAverageUsDieselPriceCreateValidationManager : IWeeklyAverageUsDieselPriceCreateValidationManager
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly AbstractValidator<WeeklyAverageUsDieselPriceCreateModel> createModelValidator;
        private readonly ILogger<WeeklyAverageUsDieselPriceCreateValidationManager> logger;
        private Dictionary<DateOnly, DataAccess.Entities.WeeklyAverageUsDieselPrice> weeklyAvergeUsDieselPricesFromDbByWeekOf;

        public WeeklyAverageUsDieselPriceCreateValidationManager(
            AbstractValidator<WeeklyAverageUsDieselPriceCreateModel> createModelValidator,
            ApplicationDbContext applicationDbContext,
            ILogger<WeeklyAverageUsDieselPriceCreateValidationManager> logger)
        {
            this.createModelValidator = createModelValidator;
            this.applicationDbContext = applicationDbContext;
            this.logger = logger;

        }

        public async Task<ValidationResultModel<DataAccess.Entities.WeeklyAverageUsDieselPrice>> PerformValidation(IList<WeeklyAverageUsDieselPriceCreateModel> createModels)
        {
            var result = new ValidationResultModel<DataAccess.Entities.WeeklyAverageUsDieselPrice>();
            result.isValid = true;
            var midnight = TimeOnly.MinValue;

            if (createModels.IsNullOrEmpty())
            {
                this.logger.LogError(ValidationErrorCodes.WeeklyAverageUsDieselPriceModelListNotNullOrEmpty);
                result.isValid = false;
                return result;
            }

            if (createModels.Any(x => x == null))
            {
                this.logger.LogError(ValidationErrorCodes.WeeklyAverageUsDieselPriceModelNotNull);
                result.isValid = false;
                return result;
            }

            var duplicateCreateModels = createModels
                .GroupBy(x => x.WeekOf)
                .Where(g => g.Count() > 1)
                .Select(y => y.Key)
                .ToList();

            if(duplicateCreateModels.Any())
            {
                this.logger.LogError(ValidationErrorCodes.WeeklyAverageUsDieselPriceModelListContainsDuplicates);
                result.isValid = false;
                return result;
            }

            await ModelValidationSetup();

            foreach (var model in createModels)
            {
                ValidateModels(result, model);
            }

            return result;
        }

        private void ValidateModels(
            ValidationResultModel<DataAccess.Entities.WeeklyAverageUsDieselPrice> validationResultModel,
            WeeklyAverageUsDieselPriceCreateModel model)
        {
            var modelValidatorResult = this.createModelValidator.Validate(model);

            if (!modelValidatorResult.IsValid)
            {
                this.logger.LogError($"{nameof(WeeklyAverageUsDieselPriceCreateModel)} had the following errors {string.Join(",", modelValidatorResult.Errors.Select(x => x.ErrorCode))}");
                validationResultModel.isValid = false;
                return;
            }

            if (this.weeklyAvergeUsDieselPricesFromDbByWeekOf.TryGetValue(model.WeekOf, out DataAccess.Entities.WeeklyAverageUsDieselPrice _))
            {
                this.logger.LogError($"{nameof(WeeklyAverageUsDieselPriceCreateModel)} has the error {ValidationErrorCodes.WeeklyAverageUsDieselPriceAlreadyExists}");
                validationResultModel.isValid = false;
                return;
            }

            validationResultModel.entities.Add(new DataAccess.Entities.WeeklyAverageUsDieselPrice
            {
                WeekOf = model.WeekOf.ToDateTime(TimeOnly.MinValue),
                AveragePrice = model.AveragePrice
            });
        }

        private async Task ModelValidationSetup()
        {
            try
            {
                this.weeklyAvergeUsDieselPricesFromDbByWeekOf = await this.applicationDbContext
                .Set<DataAccess.Entities.WeeklyAverageUsDieselPrice>()
                .ToDictionaryAsync(x => DateOnly.FromDateTime(x.WeekOf));
               
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Exception occurred in {nameof(this.PerformValidation)}");
                throw;
            }
        }
    }
}
