using ECommerceBem.Core.Enum;

namespace ECommerceBem.Core.Entities;

public class NotificacaoEntity
{
    public Guid Id { get; private set; }
    public Guid PedidoId { get; private set; }
    public string Descricao { get; private set; } = string.Empty;
    public NotificacaoEnum TipoNotificacao { get; private set; }
    public DateTime Data { get; private set; }

    public NotificacaoEntity(Guid pedidoId, string descricao, NotificacaoEnum tipoNotificacao, DateTime data)
    {
        Id = Guid.NewGuid();
        PedidoId = pedidoId;
        Descricao = descricao;
        TipoNotificacao = tipoNotificacao;
        Data = data;
    }
}
