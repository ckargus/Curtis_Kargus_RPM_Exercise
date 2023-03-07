namespace ConsoleApp.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ApplicationOptions
    {
        public int MaximumNumberOfDaysToGoBack { get; set; }

        public int FrequencyOfLoadingWeeklyAverageDieselPricesInDays { get; set; }
    }
}
