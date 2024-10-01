using ECommerceBem.Core.Entities;

namespace ECommerceBem.Core.Interfaces.Repositories;

public interface IPedidoRepository
{
    Task<PedidoEntity?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<PedidoEntity>> ObterTodosAsync();
    Task AdicionarAsync(PedidoEntity pedido);
    Task AtualizarAsync(PedidoEntity pedido);
    Task RemoverAsync(Guid id);
    Task<IEnumerable<PedidoEntity>> ObterPedidosPorDataAsync(DateTime data);
}
