using MediatR;
using Microsoft.Extensions.Configuration;
using Shared.Infra.Cqrs;
using Shared.Util.Extension;
using Shared.Util.Result;
using MKT.Integration.Application.Dto;
using MKT.Integration.Application.Events;
using MKT.Integration.Infra.Integrations.HttpServices.ServiceDesk;
using MKT.Integration.Infra.Integrations.HttpServices.ServiceDesk.Contracts.ServiceNow;
using MKT.Integration.Infra.Integrations.HttpServices.Smarkets;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MKT.Integration.Application.Commands
{
    public class CreateServiceNowCommand
    {
        public class CreateServiceNowContract : BaseCommand<Result>
        {
            public SolicitacaoFornecedorDto solicitacaoFornecedor { get; set; }
        }

        public class Handler : BaseHandler<CreateServiceNowContract, Result>
        {
            private readonly IServiceNowIntegration _serviceNowIntegration;
            private readonly ISmarketsIntegration _smarketsIntegration;
            private readonly IConfiguration _configuration;
            private readonly IMediator _mediator;

            public Handler(IServiceNowIntegration serviceNowIntegration, ISmarketsIntegration smarketsIntegration, IConfiguration configuration, IMediator mediator)
            {
                _serviceNowIntegration = serviceNowIntegration;
                _smarketsIntegration = smarketsIntegration;
                _configuration = configuration;
                _mediator = mediator;
            }
            public override async Task<Result> Handle(CreateServiceNowContract request, CancellationToken cancellationToken)
            {
                var auth = $"Basic {Shared.Util.Common.Attributes.Base64Attribute.Base64Encode(_configuration.GetSection($"{_configuration.GetSection("CompanyImage").Value}:ServiceNow:UserPassword").Value)}";
                var companies = await _serviceNowIntegration.GetCompanies(auth);
                var company = companies.result.Where(it => _configuration.GetSection("ServiceNow:CompanyIntegration").Value.Contains(it.sys_id));

                if (company == null)
                    return Result.Fail("Empresa Ultra não encontrada.");

                CreateServiceNowRequest createServiceNowDto = new CreateServiceNowRequest(_configuration.GetSection($"{_configuration.GetSection("CompanyImage").Value}:ServiceNow:Catalog:Seller").Value,
                                                                                          request.DefaultRequestHeaders["Email"],
                                                                                          string.Format(_configuration.GetSection("ServiceNow:MessageCreate").Value,
                                                                                          request.solicitacaoFornecedor.Cnpj,
                                                                                          request.solicitacaoFornecedor.RazaoSocial).RemoveAccent());

                foreach (var item in company)
                {
                    CreateServiceNowRequest.Company employ = new CreateServiceNowRequest.Company(item.u_csc_business_area, item.sys_id, _configuration.GetSection("ServiceNow:CompanyText").Value);
                    createServiceNowDto.aoop_solicita_es_erp_baan_cadastro_de_fornecedor_funcion_rio.Add(employ);
                }

                CreateServiceNowResponse resultCreateServiceNow = await _serviceNowIntegration.CreateRequestSeller(createServiceNowDto, auth);

                await _mediator.Publish(new UpdateSupplierRequestEvent(resultCreateServiceNow.Result.idRequisicao, request.solicitacaoFornecedor.IdSolicitacaoFornecedor));

                return Result.Ok();
            }
        }
    }
}
