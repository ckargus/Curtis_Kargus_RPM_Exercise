namespace Business.WeeklyAverageUsDieselPrice.Managers
{
    using Business.WeeklyAverageUsDieselPrice.Models;
    using System.Collections.Generic;

    public interface IWeeklyAverageUsDieselPriceCreateManager
    {
        Task CreateWeeklyAverageUsDieselPricesAsync(IList<WeeklyAverageUsDieselPriceCreateModel> createModels);
    }
}
