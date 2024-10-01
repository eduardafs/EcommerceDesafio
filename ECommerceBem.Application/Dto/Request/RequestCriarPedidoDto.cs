using ECommerceBem.Core.Enum;

namespace ECommerceBem.Application.Dto.Request;

public class RequestCriarPedidoDto
{
    public FormaPagamentoEnum FormaPagamento { get; set; }
    public List<ItemPedidoRequestDto> Itens { get; set; } = new List<ItemPedidoRequestDto>();
}

public class ItemPedidoRequestDto
{
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
}