using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using Serilog;
using Shared.Infra.Event.EventConsumer.EventHubConsumer;
using MKT.Integration.Application.EventHub;
using MKT.Integration.Infra.Integrations.HttpServices.Sap4Hana;
using MKT.Integration.Infra.Integrations.HttpServices.ServiceDesk;
using MKT.Integration.Infra.Integrations.HttpServices.MKT;
using System;

namespace MKT.Client.Outbound
{
    class Program
    {
        private static IConfiguration _configuration;

        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            _configuration = GetConfiguration();

            Log.Logger = CreateSerilogLogger(_configuration);

            Serilog.Debugging.SelfLog.Enable(msg => System.Diagnostics.Debug.WriteLine(msg));

            return Host.CreateDefaultBuilder(args)
                    .ConfigureServices((hostContext, services) =>
                    {
                        ConfigureServices(services);
                        services.AddHostedService<Worker>();
                    });
        }

        private static IConfiguration GetConfiguration()
        {
            var configAppSettings = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddAzureKeyVault(configAppSettings["AzureKeyVault:DNS"],
                            configAppSettings["AzureKeyVault:ClientId"],
                            configAppSettings["AzureKeyVault:ClientSecret"])
                            .AddEnvironmentVariables();

            return config.Build();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var configuration = GetConfiguration();

            services
                .AddRefitClient<ISap4HaneIntegration>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration.GetValue<string>($"{configuration.GetValue<string>("CompanyImage")}:Erp:BaseUrl")));

            services
                .AddRefitClient<IServiceNowIntegration>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration.GetValue<string>($"{configuration.GetValue<string>("CompanyImage")}:CustomerService:BaseUrl")));

            services
                .AddRefitClient<ISmarketsIntegration>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration.GetValue<string>("Platform:BaseUrl")));

            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<IEventHubConsume, EventHubConsumer>();

            var assembly = AppDomain.CurrentDomain.Load("Smarkets.Integration.Application");
            services.AddMediatR(assembly);

            services.AddLogging(builder =>
            {
                builder.AddSerilog(logger: CreateSerilogLogger(GetConfiguration()), dispose: true);
            });
        }

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .Enrich.WithProperty("ApplicationContext", configuration.GetValue<string>("Properties:Application"))
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}
