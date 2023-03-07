namespace Business.WeeklyAverageUsDieselPrice.Managers
{
    using Business.WeeklyAverageUsDieselPrice.Models;
    using DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class WeeklyAverageUsDieselPriceQueryManager : IWeeklyAverageUsDieselPriceQueryManager
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ILogger<WeeklyAverageUsDieselPriceQueryManager> logger;

        public WeeklyAverageUsDieselPriceQueryManager(
            ApplicationDbContext applicationDbContext,
            ILogger<WeeklyAverageUsDieselPriceQueryManager> logger)
        {
            this.applicationDbContext = applicationDbContext;
            this.logger = logger;
        }

        public IList<WeeklyAverageUsDieselPriceReadModel> GetWeeklyAverageDieselPrices()
        {
            try
            {
                return this.applicationDbContext.Set<DataAccess.Entities.WeeklyAverageUsDieselPrice>()
                    .AsNoTracking()
                    .Select(x => new WeeklyAverageUsDieselPriceReadModel
                    {
                        Id = x.Id,
                        AveragePrice = x.AveragePrice,
                        WeekOf = DateOnly.FromDateTime(x.WeekOf)
                    }).ToList();
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, $"Exception occured in {nameof(this.GetWeeklyAverageDieselPrices)}");
                throw;
            }
        }
    }
}
