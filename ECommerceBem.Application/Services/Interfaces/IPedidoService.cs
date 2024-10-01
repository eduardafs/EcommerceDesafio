using ECommerceBem.Application.Dto.Request;
using ECommerceBem.Application.Dto.Response;
using ECommerceBem.Core.Entities;

namespace ECommerceBem.Application.Services.Interfaces;

public interface IPedidoService
{
    Task<ResponseCriarPedidoDto> CriarAsync(RequestCriarPedidoDto pedido);
    Task ProcessarPagamentoAsync(Guid idPedido);
    Task CancelarPedidoAsync(Guid pedidoId);
    Task<List<ResponsePedidoDto>> BuscarTodosAsync();
}
