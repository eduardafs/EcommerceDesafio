using ECommerceBem.Core.Entities;

namespace ECommerceBem.Application.Services.Interfaces;

public interface IPagamentoService
{
    Task<bool> ProcessarPagamentoAsync(PedidoEntity pedido);
}
