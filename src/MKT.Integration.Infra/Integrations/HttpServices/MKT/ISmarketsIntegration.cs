using Refit;
using MKT.Integration.Infra.Integrations.HttpServices.MKT.Contracts;
using System.Threading.Tasks;

namespace MKT.Integration.Infra.Integrations.HttpServices.MKT
{
    public interface IMktIntegration
    {
        [Post("/api/autenticacoes")]
        Task<GetTokenResponse> GetToken(GetTokenRequest model);
        Task UpdateStatus();
        [Patch("/api/solicitacoesfornecedores/{id}/numrequisicaosistemachamado")]
        Task UpdateIntegrationId([AliasAs("id")] long idSolicitacaoFornecedor, UpdateSolicitacaoFornecedorIdIntegracaoServiceDeskRequest model, [Header("Authorization")] string auth);
    }
}
