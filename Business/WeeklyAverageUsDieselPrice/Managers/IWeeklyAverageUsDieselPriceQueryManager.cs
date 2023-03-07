namespace Business.WeeklyAverageUsDieselPrice.Managers
{
    using Business.WeeklyAverageUsDieselPrice.Models;
    using System.Collections.Generic;

    public interface IWeeklyAverageUsDieselPriceQueryManager
    {
        IList<WeeklyAverageUsDieselPriceReadModel> GetWeeklyAverageDieselPrices();
    }
}
