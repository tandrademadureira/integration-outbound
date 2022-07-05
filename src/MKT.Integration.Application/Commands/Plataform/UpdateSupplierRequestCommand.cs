using Microsoft.Extensions.Configuration;
using Shared.Infra.Cqrs;
using Shared.Util.Result;
using MKT.Integration.Infra.Integrations.HttpServices.MKT;
using MKT.Integration.Infra.Integrations.HttpServices.MKT.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace MKT.Integration.Application.Commands.Plataform
{
    public class UpdateSupplierRequestCommand
    {
        public class UpdateSupplierRequestContract : BaseCommand<Result>
        {
            public string IdIntegrationServiceDesk { get; set; }
            public long IdSupplierRequest { get; set; }
        }

        public class Handler : BaseHandler<UpdateSupplierRequestContract, Result>
        {
            private readonly ISmarketsIntegration _smarketsIntegration;
            private readonly IConfiguration _configuration;

            public Handler(ISmarketsIntegration smarketsIntegration, IConfiguration configuration)
            {
                _smarketsIntegration = smarketsIntegration;
                _configuration = configuration;
            }

            public override async Task<Result> Handle(UpdateSupplierRequestContract request, CancellationToken cancellationToken)
            {
                var tokenResult = await _smarketsIntegration.GetToken(new GetTokenRequest
                {
                    Identificador = _configuration.GetSection("Platform:User:Identifier").Value,
                    Senha = _configuration.GetSection("Platform:User:Password").Value,
                    IdTenant = long.Parse(_configuration.GetSection("Platform:User:IdTenant").Value)
                });

                await _smarketsIntegration.UpdateIntegrationId(request.IdSupplierRequest,
                                                                new UpdateSolicitacaoFornecedorIdIntegracaoServiceDeskRequest { NumRequisicaoSistemaChamado = request.IdIntegrationServiceDesk },
                                                                $"Bearer {tokenResult.token}");
                return Result.Ok();
            }
        }
    }
}
