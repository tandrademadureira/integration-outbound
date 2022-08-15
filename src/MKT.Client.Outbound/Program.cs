using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using Serilog;
using Shared.Infra.Event.EventConsumer.EventHubConsumer;
using MKT.Integration.Application.EventHub;
using System;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using MKT.Integration.Infra.UnitOfWork;
using MKT.Integration.Infra.Data.Repositories;
using MKT.Integration.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

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
                 .ConfigureAppConfiguration((context, config) =>
                 {
                     var settings = config.Build();


                     var keyVaultEndpoint = settings["VaultURI"];

                     if (!string.IsNullOrEmpty(keyVaultEndpoint))
                     {
                         var azureServiceTokenProvider = new AzureServiceTokenProvider();
                         var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                         config.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
                     }
                 })
                .ConfigureServices((hostContext, services) =>
                {
                    ConfigureServices(services);
                    services.AddHostedService<Worker>();
                });
        }

        private static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var configuration = GetConfiguration();

            services.AddSingleton(configuration);
            services.AddScoped<IEventHubConsume, EventHubConsumer>();

            var assembly = AppDomain.CurrentDomain.Load("Mkt.Integration.Application");
            services.AddMediatR(assembly);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddLogging(builder =>
            {
                builder.AddSerilog(logger: CreateSerilogLogger(GetConfiguration()), dispose: true);
            });

            services.AddDbContext<DbContextCatalog>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionConfiguration"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(3),
                    errorNumbersToAdd: null);
                }));
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
