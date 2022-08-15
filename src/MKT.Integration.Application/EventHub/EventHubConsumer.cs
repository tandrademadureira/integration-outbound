using Azure.Messaging.EventHubs.Processor;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Shared.Infra.Event.EventConsumer;
using Shared.Infra.Event.EventConsumer.EventHubConsumer;
using Shared.Infra.Event.EventProducer;
using MKT.Integration.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MKT.Integration.Application.EventHub
{
    public class EventHubConsumer : EventHubConsumeAbstract, IEventHubConsume
    {
        private readonly IMediator _mediator;
        public ILogger<IEventConsume> _logger { get; private set; }
        private readonly IConfiguration _configuration;
        public EventHubConsumer(IConfiguration configuration, IMediator mediator)
            : base(configuration.GetSection("EventHubConnectionString").Value,
                  configuration.GetSection("EventHubName").Value,
                  configuration.GetSection("StoragesConnectionString").Value,
                  configuration.GetSection("StorageBlobName").Value,
                  configuration.GetSection("StorageConsumerGroup").Value)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        public async Task SetLogger(ILogger<IEventConsume> logger)
        {
            _logger = logger;
        }

        public async Task StartProcessingAsync()
        {
            await base.StartProcessingAsync();
        }

        public async Task StopProcessingAsync()
        {
            await base.StopProcessingAsync();
        }

        protected override Task EventProcessorClient_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            return Task.CompletedTask;
        }

        protected override async Task EventProcessorClient_ProcessEventAsync(ProcessEventArgs arg)
        {
            using (LogContext.PushProperty("X-CorrelationId", Guid.NewGuid().ToString()))
            {
                try
                {
                    CatalogDto objEventObj = 
                        JsonConvert.DeserializeObject<CatalogDto>(Encoding.UTF8.GetString(arg.Data.Body.ToArray()));

                    //todo: eFETUAR INTEGRAÇÃO COM O erp
                    //var inProgressRequest = new UpdateCatalogCommand.UpdateCatalogContract {  Id = objEventObj.Id };
                    //await _mediator.Send(inProgressRequest);
                }
                catch (Exception ex) { _logger.LogError(ex, ex.Message); }
                finally
                {
                    await arg.UpdateCheckpointAsync(arg.CancellationToken);
                }
            }
        }
    }
}
