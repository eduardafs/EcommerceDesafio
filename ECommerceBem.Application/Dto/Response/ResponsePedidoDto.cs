namespace ECommerceBem.Application.Dto.Response;

public class ResponsePedidoDto
{
    public Guid Id { get; set; }
    public List<ItemPedidoDto> Itens { get; set; } = [];
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DataPedido { get; set; }
    public string FormaPagamento { get; set; } = string.Empty;
}

public class ItemPedidoDto
{
    public string NomeProduto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoTotal { get; set; }
}

