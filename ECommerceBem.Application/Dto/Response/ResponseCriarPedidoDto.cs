using ECommerceBem.Core.Enum;

namespace ECommerceBem.Application.Dto.Response;

public class ResponseCriarPedidoDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public string FormaPagamento { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public DateTime DataPedido { get; set; }
}