using ECommerceBem.Core.Entities;

namespace ECommerceBem.Core.Interfaces.Repositories;

public interface INotificacaoRepository
{
    Task AdicionarAsync(NotificacaoEntity notificacao);
}
