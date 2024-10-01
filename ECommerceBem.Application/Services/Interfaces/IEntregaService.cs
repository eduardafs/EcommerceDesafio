using ECommerceBem.Core.Entities;

namespace ECommerceBem.Application.Services.Interfaces;

public interface IEntregaService
{
    Task ProcessarEntregaAsync(PedidoEntity pedido);
}
