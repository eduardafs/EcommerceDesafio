using ECommerceBem.Core.Entities;
using ECommerceBem.Core.Events;
using ECommerceBem.Core.Interfaces.Repositories;
using MediatR;

namespace ECommerceBem.Application.Events;

public class NotificacaoEventHandler : INotificationHandler<NotificacaoEvent>
{
    private readonly INotificacaoRepository _notificacaoRepository;

    public NotificacaoEventHandler(INotificacaoRepository notificacaoRepository)
    {
        _notificacaoRepository = notificacaoRepository;
    }

    public async Task Handle(NotificacaoEvent notification, CancellationToken cancellationToken)
    {
        var notificacao = new NotificacaoEntity(notification.PedidoId, notification.Mensagem, notification.TipoNotificacao, notification.Data);
        await _notificacaoRepository.AdicionarAsync(notificacao);
    }
}
