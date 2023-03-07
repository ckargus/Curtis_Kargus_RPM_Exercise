namespace Business.WeeklyAverageUsDieselPrice.Models
{
    public abstract class WeeklyAverageUsDieselPriceBaseModel
    {
        public DateOnly WeekOf { get; set; }

        public double AveragePrice { get; set; }
    }
}
