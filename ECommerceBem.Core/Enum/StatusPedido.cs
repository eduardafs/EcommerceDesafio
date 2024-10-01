using System.ComponentModel;

namespace ECommerceBem.Core.Enum;

public enum StatusPedido
{
    [Description("Aguardando Processamento")]
    AguardandoProcessamento,

    [Description("Processando Pagamento")]
    ProcessandoPagamento,

    [Description("Pagamento Concluído")]
    PagamentoConcluido,

    [Description("Separando Pedido")]
    SeparandoPedido,

    [Description("Concluído")]
    Concluido,

    [Description("Aguardando Estoque")]
    AguardandoEstoque,

    [Description("Cancelado")]
    Cancelado
}
