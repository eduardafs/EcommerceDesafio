using ECommerceBem.Core.Enum;

namespace ECommerceBem.Core.Events;

public class NotificacaoEvent : IDomainEvent
{
    public Guid PedidoId { get; }
    public string Mensagem { get; }
    public NotificacaoEnum TipoNotificacao { get; }
    public DateTime Data { get; }

    public NotificacaoEvent(Guid pedidoId, string mensagem, NotificacaoEnum tipoNotificacao)
    {
        PedidoId = pedidoId;
        Mensagem = mensagem;
        TipoNotificacao = tipoNotificacao;
        Data = DateTime.Now;
    }
}
