using System.ComponentModel;

namespace MKT.Integration.Application.Enum
{
    public enum StatusServiceNowEnum
    {
        [Description("Aguardando Informações")]
        AguardandoInformacoes = -5,

        [Description("Aberto")]
        Aberto = 1,

        [Description("Em Andamento")]
        EmAndamento = 2,

        [Description("Encerrado")]
        Encerrado = 3,

        [Description("Cancelado")]
        Cancelado = 4,

        [Description("Em aprovação")]
        EmAprovacao = 5,

        [Description("Solucionado")]
        Solucionado = 6,

        [Description("Aguardando Atendimento")]
        AguardandoAtendimento = 9,

        [Description("Expirado")]
        Expirado = 10,

        [Description("Agendado")]
        Agendado = 11
    }
}
