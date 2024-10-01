using ECommerceBem.Application.Services.Interfaces;
using ECommerceBem.Core.Entities;

namespace ECommerceBem.Application.Services;

public class EntregaService : IEntregaService
{
    public Task ProcessarEntregaAsync(PedidoEntity pedido)
    {
        pedido.DefinirStatusConcluido();
        return Task.CompletedTask;
    }
}
