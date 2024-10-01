using ECommerceBem.Core.Entities;

namespace ECommerceBem.Application.Strategies;

public class PagamentoCartaoCreditoStrategy : IPagamentoStrategy
{
    public async Task ProcessarPagamentoAsync(PedidoEntity pedido)
    {
        await Task.Delay(1000);
        pedido.DefinirStatusPagamentoConcluido();
    }
}
