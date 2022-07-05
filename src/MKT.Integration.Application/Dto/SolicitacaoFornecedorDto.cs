using System;
using System.ComponentModel;

namespace MKT.Integration.Application.Dto
{
    public class SolicitacaoFornecedorDto
    {
        public long IdSolicitacaoFornecedor { get; set; }
        public StatusSolicitacaoFornecedor Status { get; set; }
        public string CodigoERP { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string Cnpj { get; set; }
        public DateTime? DataAberturaCnpj { get; set; }
        public PorteEmpresa? Porte { get; set; }
        public long IdNaturezaJuridica { get; set; }
        public int NumeroFuncionarios { get; set; }
        public TipoCadastroEmpresa? TipoCadastro { get; set; }
        public string HomePage { get; set; }
        public string Contato { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public PerfilTrib? PerfilTributario { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public decimal PatrimonioLiquido { get; set; }
        public bool OptanteSimplesNacional { get; set; }
        public decimal CapitalSocial { get; set; }
        public decimal CapitalIntegralizado { get; set; }
        public DateTime? DataIntegralizacao { get; set; }
        public long? IdUsuarioResponsavel { get; set; }
        public long? IdSlaSolicitacao { get; set; }
        public string MotivoCancelamento { get; set; }


        public long IdTenant { get; set; }
        public string Empresa { get; set; }
        public string IdIntegracaoSistemaChamado { get; set; }

        public DateTime? DataExclusao { get; set; }
        public Solicitado? SolicitadoPor { get; set; }
        public long IdUsuarioSolicitante { get; set; }
        public long IdEmpresaSolicitante { get; set; }
        public long IdUsuario { get; set; }

        public enum StatusSolicitacaoFornecedor
        {
            [Description("Solicitado")]
            Solicitado = 1,
            [Description("Aprovado")]
            Aprovado = 2,
            [Description("Reprovado")]
            Reprovado = 3,
            [Description("Cancelado")]
            Cancelado = 4,
            [Description("Aguardando Integração")]
            AguardandoIntegracao = 5
        }

        public enum PorteEmpresa
        {
            Pequeno = 1,
            MicroEmpresa = 2,
            Medio = 3,
            Grande = 4
        }

        public enum TipoCadastroEmpresa
        {
            [Description("Cadastro Completo")]
            Completo = 1,
            [Description("Cadastro Simplificado")]
            Simplificado = 2,
            [Description("Cadastro Simplificado")]
            Registrado = 3,
        }

        public enum PerfilTrib
        {
            [Description("Lucro Presumido")]
            LucroPresumido = 1,
            [Description("Lucro Real")]
            LucroReal = 2
        }

        public enum Solicitado
        {
            [Description("Holding")]
            Holding = 1,
            [Description("Franquia/Matriz/Filial")]
            FranquiaMatrizFilial = 2
        }
    }
}
