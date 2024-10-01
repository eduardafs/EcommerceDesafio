using ECommerceBem.Core.Entities;

namespace ECommerceBem.Application.Strategies;

public class PagamentoPixStrategy : IPagamentoStrategy
{
    private readonly decimal DESCONTO_PIX= 0.05m;

    public async Task ProcessarPagamentoAsync(PedidoEntity pedido)
    {
        await Task.Delay(500);
        pedido.AplicarDescontoPix(DESCONTO_PIX);
    }
}
