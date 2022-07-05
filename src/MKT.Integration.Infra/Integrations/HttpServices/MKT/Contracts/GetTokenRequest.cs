namespace MKT.Integration.Infra.Integrations.HttpServices.MKT.Contracts
{
    public class GetTokenRequest
    {
        public string Identificador { get; set; }
        public string Senha { get; set; }
        public long IdTenant { get; set; }
    }
}
