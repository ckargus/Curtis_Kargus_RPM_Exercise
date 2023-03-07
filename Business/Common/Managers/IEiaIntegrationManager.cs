namespace Business.Common.Managers
{
    using Business.Common.Model;

    public interface IEiaIntegrationManager
    {
        public Task<IList<DatePriceModel>> GetWeeklyAverageUsDieselPrices();
    }
}
