﻿using Microsoft.Extensions.Configuration;
using Shared.Infra.Cqrs;
using Shared.Util.Result;
using MKT.Integration.Application.Dto;
using MKT.Integration.Infra.Integrations.HttpServices.ServiceDesk;
using MKT.Integration.Infra.Integrations.HttpServices.ServiceDesk.Contracts.ServiceNow;
using System.Threading;
using System.Threading.Tasks;

namespace MKT.Integration.Application.Commands
{
    public class CancelServiceNowCommand
    {
        public class CancelServiceNowContract : BaseCommand<Result>
        {
            public SolicitacaoFornecedorDto solicitacaoFornecedor { get; set; }
        }

        public class Handler : BaseHandler<CancelServiceNowContract, Result>
        {
            private readonly IServiceNowIntegration _serviceNowIntegration;
            private readonly IConfiguration _configuration;

            public Handler(IServiceNowIntegration serviceNowIntegration, IConfiguration configuration)
            {
                _serviceNowIntegration = serviceNowIntegration;
                _configuration = configuration;
            }

            public override async Task<Result> Handle(CancelServiceNowContract request, CancellationToken cancellationToken)
            {
                var auth = $"Basic {Shared.Util.Common.Attributes.Base64Attribute.Base64Encode(_configuration.GetSection($"{_configuration.GetSection("CompanyImage").Value}:ServiceNow:UserPassword").Value)}";
                UpdateRequest updateRequest = new UpdateRequest(request.solicitacaoFornecedor.IdIntegracaoSistemaChamado, 4,
                                                                _configuration.GetSection($"{_configuration.GetSection("CompanyImage").Value}:ServiceNow:IdSmarkets").Value, request.solicitacaoFornecedor.MotivoCancelamento);

                updateRequest.AddVariaveis(_configuration.GetSection($"{_configuration.GetSection("CompanyImage").Value}:ServiceNow:Business").Value,
                                          _configuration.GetSection($"{_configuration.GetSection("CompanyImage").Value}:ServiceNow:CompanyId").Value,
                                          _configuration.GetSection($"{_configuration.GetSection("CompanyImage").Value}:ServiceNow:ImmediateManager").Value);

                await _serviceNowIntegration.UpdateRequest(updateRequest, auth);

                return Result.Ok();
            }
        }
    }
}
