namespace Business.Common.Managers
{
    using Business.Common.Model;
    using Business.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class EiaIntegrationManager : IEiaIntegrationManager
    {
        private const string UsNationalWeelkyDieselPriceId = "&series_id=PET.EMD_EPD2D_PTE_NUS_DPG.W";
        
        private HttpClient httpClient;
        private ILogger<EiaIntegrationManager> logger;
        private readonly DataExtractionOptions options;

        public EiaIntegrationManager(
            HttpClient httpClient,
            ILogger<EiaIntegrationManager> logger,
            IOptions<DataExtractionOptions> options)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.options = options.Value;
        }

        public async Task<IList<DatePriceModel>> GetWeeklyAverageUsDieselPrices()
        {
            IList<DatePriceModel> result = new List<DatePriceModel>();
            try
            {
                this.logger.LogInformation("Getting weekly average us diesel prices from EIA Endpoint");

                var httpResponse = await this.httpClient.GetAsync($"?api_key={this.options.EiaApiKey}{UsNationalWeelkyDieselPriceId}").ConfigureAwait(false);
                if (httpResponse.IsSuccessStatusCode)
                { 
                    result = await DeserializeWeeklyAverageUsDieselPrices(httpResponse).ConfigureAwait(false);
                    this.logger.LogInformation("Finished getting weekly average us diesel prices from EIA Endpoint");
                }
                else
                {
                    this.logger.LogError($"Unable to get data during {nameof(this.GetWeeklyAverageUsDieselPrices)}");
                }
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, $"Exception occurred in {nameof(this.GetWeeklyAverageUsDieselPrices)}");
                throw;
            }
            return result;
        }

        private async Task<IList<DatePriceModel>> DeserializeWeeklyAverageUsDieselPrices(HttpResponseMessage httpResponse)
        {
            IList<DatePriceModel> result = new List<DatePriceModel>();
            string responseContent = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            JObject? jsonParsedObject = null;
            try
            {
                jsonParsedObject = JObject.Parse(responseContent);
            }
            catch(JsonReaderException ex)
            {
                this.logger.LogError(ex, $" Exception occurred while deserializing json object in {nameof(this.DeserializeWeeklyAverageUsDieselPrices)}");
                throw;
            }
            
            var weeklyAverageUsDieselPrices = jsonParsedObject?["series"]?[0]?["data"]?.Children();
            if (weeklyAverageUsDieselPrices != null)
            {
                foreach (var weeklyAverageUsDieselPrice in weeklyAverageUsDieselPrices)
                {
                    double price;
                    DateOnly date;
                    if (!DateOnly.TryParseExact(weeklyAverageUsDieselPrice[0]?.ToString(), "yyyyMMdd", out date))
                    {
                        this.logger.LogError($"Unable to parse date: {weeklyAverageUsDieselPrice[0]?.ToString()} for Weekly Average Us Diesel Price, skipping record");
                        continue;
                    }
                    if (!double.TryParse(weeklyAverageUsDieselPrice[1]?.ToString(), out price))
                    {
                        this.logger.LogError($"Unable to parse price: {weeklyAverageUsDieselPrice[1]?.ToString()} for Weekly Average Us Diesel Price, skipping record");
                        continue;
                    }
                    result.Add(new DatePriceModel
                    {
                        Date = date,
                        Price = price
                    }); ;
                }
            }
            else
            {
                this.logger.LogError($"Unable to find series data for Weekly Average Us Diesel Prices");
            } 

            return result;
        }
    }
}
