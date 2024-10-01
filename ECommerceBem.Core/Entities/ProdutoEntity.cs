using ECommerceBem.Exception;
using ECommerceBem.Exception.ExceptionsBase;

namespace ECommerceBem.Core.Entities;

public class ProdutoEntity
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public decimal PrecoUnitario { get; private set; }
    public int QuantidadeEmEstoque {  get; private set; }

    public int QuantidadeMinimaParaDesconto { get; private set; }
    public decimal DescontoPorQuantidade { get; private set; }
    public int MesComDescontoSazonal { get; private set; }
    public decimal DescontoSazonalPercentual { get; private set; }

    public ProdutoEntity(string nome, decimal precoUnitario, int quantidadeEmEstoque, int quantidadeMinimaParaDesconto, decimal descontoPorQuantidade, int mesComDescontoSazonal, decimal descontoSazonalPercentual)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        PrecoUnitario = precoUnitario;
        QuantidadeEmEstoque = quantidadeEmEstoque;
        QuantidadeMinimaParaDesconto = quantidadeMinimaParaDesconto;
        DescontoPorQuantidade = descontoPorQuantidade;
        MesComDescontoSazonal = mesComDescontoSazonal;
        DescontoSazonalPercentual = descontoSazonalPercentual;
    }

    public bool DiminuirEstoque(int quantidade)
    {
        if (QuantidadeEmEstoque <= quantidade)
            return false;

        QuantidadeEmEstoque -= quantidade;
        return true;
    }

    public void AdicionarEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new NotFoundException(ResourceErrorsMessages.ProdutoEstoqueErro);

        QuantidadeEmEstoque += quantidade;
    }
}
