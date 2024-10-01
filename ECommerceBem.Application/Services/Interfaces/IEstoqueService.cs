using ECommerceBem.Core.Entities;

namespace ECommerceBem.Application.Services.Interfaces;

public interface IEstoqueService
{
    Task AtualizarEstoqueAsync(PedidoEntity pedido);
}
