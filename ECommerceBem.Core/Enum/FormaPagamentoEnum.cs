using System.ComponentModel;

namespace ECommerceBem.Core.Enum;

public enum FormaPagamentoEnum
{
    [Description("Pix")]
    Pix = 1,

    [Description("Cartão de Credito")]
    CartaoCredito = 2,

    [Description("Dinheiro")]
    Dinheiro = 3
}
