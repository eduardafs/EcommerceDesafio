using ECommerceBem.Application.Services.Interfaces;
using ECommerceBem.Application.Strategies;
using ECommerceBem.Core.Entities;
using ECommerceBem.Core.Enum;
using ECommerceBem.Exception.ExceptionsBase;

namespace ECommerceBem.Application.Services;

public class PagamentoService : IPagamentoService
{
    public async Task<bool> ProcessarPagamentoAsync(PedidoEntity pedido)
    {
        IPagamentoStrategy strategy = pedido.FormaPagamento switch
        {
            FormaPagamentoEnum.Pix => new PagamentoPixStrategy(),
            FormaPagamentoEnum.CartaoCredito => new PagamentoCartaoCreditoStrategy(),
            _ => throw new NotFoundException("Forma de pagamento inválida.")
        };

        await strategy.ProcessarPagamentoAsync(pedido);
        return true;
    }
}
