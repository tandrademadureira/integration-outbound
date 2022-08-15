using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Infra.Event.EventConsumer.EventHubConsumer;
using MKT.Integration.Application.EventHub;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MKT.Client.Outbound
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<EventHubConsumer> _logger;
        private readonly IEventHubConsume _eventHubConsume;

        public Worker(ILogger<EventHubConsumer> logger, IEventHubConsume eventHubConsume)
        {
            _logger = logger;
            _eventHubConsume = eventHubConsume;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);

            await _eventHubConsume.SetLogger(_logger);
            await _eventHubConsume.StartProcessingAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"Worker running at: {DateTimeOffset.Now}");

                await Task.Delay(TimeSpan.FromSeconds(10));
                await _eventHubConsume.StartProcessingAsync();
            }

            await _eventHubConsume.StopProcessingAsync();
        }
    }
}
