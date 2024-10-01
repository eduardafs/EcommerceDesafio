using ECommerceBem.Core.Entities;

namespace ECommerceBem.Application.Strategies;

public interface IPagamentoStrategy
{
    Task ProcessarPagamentoAsync(PedidoEntity pedido);
}
