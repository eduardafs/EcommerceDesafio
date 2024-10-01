using ECommerceBem.Application.Dto.Request;
using ECommerceBem.Application.Dto.Response;
using ECommerceBem.Application.Services.Interfaces;
using ECommerceBem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBem.Controllers;

[Route("api/pedido")]
[ApiController]
public class PedidoController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidoController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpPost("criar")]
    [ProducesResponseType(typeof(RequestCriarPedidoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorsDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromBody] RequestCriarPedidoDto pedido)
    { 
        var retorno = await _pedidoService.CriarAsync(pedido);
        return Created(string.Empty, retorno);
    }

    [HttpPost("processar-pagamento/{idPedido}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorsDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ProcessarPagamento(Guid idPedido)
    {
        await _pedidoService.ProcessarPagamentoAsync(idPedido);
        return NoContent(); 
    }

    [HttpDelete("deletar/{idPedido}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorsDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Deletar(Guid idPedido)
    {
        await _pedidoService.CancelarPedidoAsync(idPedido);
        return NoContent();
    }

    [HttpGet("buscar-todos")]
    [ProducesResponseType(typeof(List<ResponsePedidoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorsDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Buscar()
    {
        var pedidos = await _pedidoService.BuscarTodosAsync();
        return Ok(pedidos);
    }
}
