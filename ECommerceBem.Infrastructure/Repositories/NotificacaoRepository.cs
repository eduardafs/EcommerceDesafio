using ECommerceBem.Core.Entities;
using ECommerceBem.Core.Interfaces.Repositories;
using ECommerceBem.Infrastructure.DBContext;

namespace ECommerceBem.Infrastructure.Repositories;

public class NotificacaoRepository : INotificacaoRepository
{
    private readonly ECommerceBemDBContext _context;

    public NotificacaoRepository(ECommerceBemDBContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(NotificacaoEntity notificacao)
    {
        await _context.AddAsync(notificacao);
        await _context.SaveChangesAsync();
    }
}
