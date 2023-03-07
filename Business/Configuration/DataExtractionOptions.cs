namespace Business.Configuration
{
    public class DataExtractionOptions
    {
        public string EiaApiKey { get; set; }

        public int MaximumNumberOfDaysToGoBack { get; set; }

        public int FrequencyOfLoadingWeeklyAverageDieselPricesInDays { get; set; }
    }
}
