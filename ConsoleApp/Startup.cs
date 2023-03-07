namespace ConsoleApp
{
    using Business.Common.Managers;
    using Business.Configuration;
    using Business.Jobscheduler;
    using Business.WeeklyAverageUsDieselPrice.Managers;
    using Business.WeeklyAverageUsDieselPrice.Models;
    using Business.WeeklyAverageUsDieselPrice.Validators;
    using ConsoleApp.Configuration;
    using DataAccess;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, false)
                .Build();

            var applicationConfiguration = configurationRoot.Get<ApplicationConfiguration>();

            if(applicationConfiguration == null)
            {
                throw new Exception("Unable to load applcation configuration");
            }

            services.Configure<DataExtractionOptions>((options) =>
            {
                options.MaximumNumberOfDaysToGoBack = applicationConfiguration.ApplicationOptions.MaximumNumberOfDaysToGoBack;
                options.FrequencyOfLoadingWeeklyAverageDieselPricesInDays = applicationConfiguration.ApplicationOptions.FrequencyOfLoadingWeeklyAverageDieselPricesInDays;
                options.EiaApiKey = applicationConfiguration.ServiceOptions.EiaApiKey;
               
            });

            services.AddLogging(builder =>
            {
                builder.AddConfiguration(configurationRoot.GetSection("Logging"));
                builder.AddConsole();
            });

            services.AddSingleton(applicationConfiguration);

            services.AddSingleton<IJobSchedulerManager, JobSchedulerManager>();
            services.AddTransient<IEiaIntegrationManager, EiaIntegrationManager>();

            services.AddTransient<AbstractValidator<WeeklyAverageUsDieselPriceCreateModel>, WeeklyAverageUsDieselPriceCreateModelValidator>();
            services.AddTransient<IWeeklyAverageUsDieselPriceCreateValidationManager, WeeklyAverageUsDieselPriceCreateValidationManager>();

            services.AddTransient<IWeeklyAverageUsDieselPriceQueryManager, WeeklyAverageUsDieselPriceQueryManager>();
            services.AddTransient<IWeeklyAverageUsDieselPriceCreateManager, WeeklyAverageUsDieselPriceCreateManager>();

            services.AddTransient<IWeeklyAverageUsDieselPriceExtractionManager, WeeklyAverageUsDieselPriceExtractionManager>();

            services.AddHttpClient<IEiaIntegrationManager, EiaIntegrationManager>(client =>
            {
                client.BaseAddress = applicationConfiguration.ServiceOptions.EiaEndpoint;
            });

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configurationRoot.GetConnectionString("DatabaseConnection")));
        }
    }
}
