namespace ECommerceBem.Core.Entities;

public class ItemPedidoEntity
{
    public Guid Id { get; private set; }
    public Guid PedidoId { get; private set; }
    public Guid ProdutoId { get; private set; }
    public ProdutoEntity Produto { get; private set; }
    public int Quantidade { get; set; }
    public decimal PrecoTotal { get; set; }

    protected ItemPedidoEntity() { }

    public ItemPedidoEntity(ProdutoEntity produto, int quantidade)
    {
        Id = Guid.NewGuid();
        Produto = produto;
        ProdutoId = produto.Id;
        Quantidade = quantidade;

        CalcularPrecoComDesconto();
    }

    public void CalcularPrecoComDesconto()
    {
        PrecoTotal = Produto.PrecoUnitario * Quantidade;

        // Aplicar desconto por quantidade
        if (Quantidade >= Produto.QuantidadeMinimaParaDesconto)
            PrecoTotal -= Produto.DescontoPorQuantidade * Quantidade;

        // Aplicar desconto sazonal
        if (DateTime.Now.Month == Produto.MesComDescontoSazonal)
            PrecoTotal -= PrecoTotal * Produto.DescontoSazonalPercentual;
    }
}
