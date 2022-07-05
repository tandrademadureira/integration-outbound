using Azure.Messaging.EventHubs.Processor;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Shared.Infra.Event.EventConsumer;
using Shared.Infra.Event.EventConsumer.EventHubConsumer;
using Shared.Infra.Event.EventProducer;
using MKT.Integration.Application.Commands;
using MKT.Integration.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MKT.Integration.Application.EventHub
{
    public class EventHubConsumer : EventHubConsumeAbstract, IEventHubConsume
    {
        private readonly IMediator _mediator;
        public ILogger<IEventConsume> _logger { get; private set; }
        private readonly IConfiguration _configuration;
        public EventHubConsumer(IConfiguration configuration, IMediator mediator)
            : base(configuration.GetSection($"{configuration.GetSection("CompanyImage").Value}:EventHub:Outbound:Seller:CustomerSupport:Consumer:ConnectionString").Value,
                  configuration.GetSection($"{configuration.GetSection("CompanyImage").Value}:EventHub:Outbound:Seller:CustomerSupport:Consumer:Name").Value,
                  configuration.GetSection($"{configuration.GetSection("CompanyImage").Value}:EventHub:StorageAccount:ConnectionString").Value,
                  configuration.GetSection($"{configuration.GetSection("CompanyImage").Value}:EventHub:StorageAccount:BlobName:Outbound:Seller:CustomerSupport").Value,
                  configuration.GetSection($"{configuration.GetSection("CompanyImage").Value}:EventHub:Outbound:Seller:CustomerSupport:Consumer:ConsumerGroup").Value)
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
                    EventObject<object> eventObjHheader = Newtonsoft.Json.JsonConvert.DeserializeObject<EventObject<object>>(Encoding.UTF8.GetString(arg.Data.Body.ToArray()));
                    string assumir = eventObjHheader.Headers["ToAssume"];

                    if (assumir == "True")
                    {
                        _logger.LogInformation(Encoding.UTF8.GetString(arg.Data.Body.ToArray()));
                        EventObject<SolicitacaoFornecedorDto> objEventObj = Newtonsoft.Json.JsonConvert.DeserializeObject<EventObject<SolicitacaoFornecedorDto>>(Encoding.UTF8.GetString(arg.Data.Body.ToArray()));

                        var inProgressRequest = new InProgressServiceNowCommand.InProgressServiceNowContract { solicitacaoFornecedor = objEventObj.Object };
                        await _mediator.Send(inProgressRequest);
                    }
                    else
                    {
                        _logger.LogInformation(Encoding.UTF8.GetString(arg.Data.Body.ToArray()));
                        EventObject<SolicitacaoFornecedorDto> eventObj = Newtonsoft.Json.JsonConvert.DeserializeObject<EventObject<SolicitacaoFornecedorDto>>(Encoding.UTF8.GetString(arg.Data.Body.ToArray()));

                        switch (eventObj.Object.Status)
                        {
                            case SolicitacaoFornecedorDto.StatusSolicitacaoFornecedor.Solicitado:
                                if (string.IsNullOrWhiteSpace(eventObj.Object.IdIntegracaoSistemaChamado))
                                {
                                    var createRequest = new CreateServiceNowCommand.CreateServiceNowContract();
                                    createRequest.solicitacaoFornecedor = eventObj.Object;
                                    foreach (KeyValuePair<string, string> item in eventObj.Headers)
                                    {
                                        createRequest.AddHeader(item.Key, item.Value);
                                    }
                                    await _mediator.Send(createRequest);
                                }
                                else
                                {
                                    var inProgressRequest = new InProgressServiceNowCommand.InProgressServiceNowContract();
                                    inProgressRequest.solicitacaoFornecedor = eventObj.Object;
                                    await _mediator.Send(inProgressRequest);
                                }
                                break;
                            case SolicitacaoFornecedorDto.StatusSolicitacaoFornecedor.Reprovado:
                                var waitInformationRequest = new WaitInformaticonServiceNowCommand.WaitInformaticonServiceNowContract();
                                waitInformationRequest.solicitacaoFornecedor = eventObj.Object;
                                await _mediator.Send(waitInformationRequest);
                                break;
                            case SolicitacaoFornecedorDto.StatusSolicitacaoFornecedor.Cancelado:
                                var cancelRequest = new CancelServiceNowCommand.CancelServiceNowContract();
                                cancelRequest.solicitacaoFornecedor = eventObj.Object;
                                await _mediator.Send(cancelRequest);
                                break;
                            case SolicitacaoFornecedorDto.StatusSolicitacaoFornecedor.AguardandoIntegracao:
                                var aproveRequest = new AproveServiceNowCommand.AproveServiceNowContract();
                                aproveRequest.solicitacaoFornecedor = eventObj.Object;
                                await _mediator.Send(aproveRequest);
                                break;
                            default:
                                break;
                        }
                    }
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
