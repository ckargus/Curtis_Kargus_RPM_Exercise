namespace Business.WeeklyAverageUsDieselPrice.Managers
{
    using System.Threading.Tasks;

    public interface IWeeklyAverageUsDieselPriceExtractionManager
    {
        Task ExtractData();
    }
}
