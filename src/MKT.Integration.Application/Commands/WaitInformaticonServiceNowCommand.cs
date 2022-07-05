using Microsoft.Extensions.Configuration;
using Shared.Infra.Cqrs;
using Shared.Util.Result;
using MKT.Integration.Application.Dto;
using System.Threading;
using System.Threading.Tasks;
using MKT.Integration.Infra.Integrations.HttpServices.MKT;

namespace MKT.Integration.Application.Commands
{
    public class WaitInformaticonServiceNowCommand
    {

        public class WaitInformaticonServiceNowContract : BaseCommand<Result>
        {
            public SolicitacaoFornecedorDto solicitacaoFornecedor { get; set; }
        }

        public class Handler : BaseHandler<WaitInformaticonServiceNowContract, Result>
        {
            private readonly IMktIntegration _mktIntegration;
            private readonly IConfiguration _configuration;

            public Handler(IMktIntegration mktIntegration, IConfiguration configuration)
            {
                _mktIntegration = mktIntegration;
                _configuration = configuration;
            }

            public override async Task<Result> Handle(WaitInformaticonServiceNowContract request, CancellationToken cancellationToken)
            {
                var auth = $"Basic {Shared.Util.Common.Attributes.Base64Attribute.Base64Encode(_configuration.GetSection($"{_configuration.GetSection("CompanyImage").Value}:ServiceNow:UserPassword").Value)}";
                //UpdateRequest updateRequest = new UpdateRequest(request.solicitacaoFornecedor.IdIntegracaoSistemaChamado, -5,
                //                                                _configuration.GetSection($"{_configuration.GetSection("CompanyImage").Value}:ServiceNow:IdMkt").Value, string.Format(_configuration.GetSection("ServiceNow:MessageUpdate").Value, request.solicitacaoFornecedor.IdIntegracaoSistemaChamado));

                //updateRequest.AddVariaveis(_configuration.GetSection($"{_configuration.GetSection("CompanyImage").Value}:ServiceNow:Business").Value,
                //                          _configuration.GetSection($"{_configuration.GetSection("CompanyImage").Value}:ServiceNow:CompanyId").Value,
                //                          _configuration.GetSection($"{_configuration.GetSection("CompanyImage").Value}:ServiceNow:ImmediateManager").Value);

                //await _mktIntegration.UpdateRequest(updateRequest, auth);

                return await Task.FromResult(Result.Ok());
            }
        }
    }
}
