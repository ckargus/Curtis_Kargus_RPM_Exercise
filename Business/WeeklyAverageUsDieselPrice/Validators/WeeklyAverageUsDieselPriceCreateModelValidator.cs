namespace Business.WeeklyAverageUsDieselPrice.Validators
{
    using Business.Common.Validation;
    using Business.WeeklyAverageUsDieselPrice.Models;
    using FluentValidation;

    public class WeeklyAverageUsDieselPriceCreateModelValidator : AbstractValidator<WeeklyAverageUsDieselPriceCreateModel>
    {
        public WeeklyAverageUsDieselPriceCreateModelValidator()
        {
            RuleFor(x => x.WeekOf).NotEmpty().WithErrorCode(ValidationErrorCodes.WeeklyAverageUsDieselPriceMustHaveDate);
            RuleFor(x => x.AveragePrice).GreaterThan(0).WithErrorCode(ValidationErrorCodes.WeeklyAverageUsDieselPriceAveragePriceMustBeGreaterThanZero);
        }
    }
}
