namespace ConsoleApp.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ServiceOptions
    {
        public Uri EiaEndpoint { get; set; }

        public string EiaApiKey { get; set; }

        public ServiceOptions(Uri eiaEndpoint, string eiaApiKey)
        {
            EiaEndpoint = eiaEndpoint;
            EiaApiKey = eiaApiKey;
        }
    }
}
