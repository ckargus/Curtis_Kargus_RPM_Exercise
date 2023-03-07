namespace Business.WeeklyAverageUsDieselPrice.Validators
{
    using Business.Common.Validation;
    using Business.WeeklyAverageUsDieselPrice.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IWeeklyAverageUsDieselPriceCreateValidationManager
    {
        Task<ValidationResultModel<DataAccess.Entities.WeeklyAverageUsDieselPrice>> PerformValidation(IList<WeeklyAverageUsDieselPriceCreateModel> createModels);
    }
}
