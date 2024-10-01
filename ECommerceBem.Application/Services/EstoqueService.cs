using ECommerceBem.Application.Services.Interfaces;
using ECommerceBem.Core.Entities;
using ECommerceBem.Core.Interfaces.Repositories;
using ECommerceBem.Exception;
using ECommerceBem.Exception.ExceptionsBase;

namespace ECommerceBem.Application.Services;

public class EstoqueService : IEstoqueService
{
    private readonly IProdutoRepository _produtoRepository;

    public EstoqueService(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task AtualizarEstoqueAsync(PedidoEntity pedido)
    {
        pedido.DefinirStatusSeparandoPedido();
        foreach (var item in pedido.Itens)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(item.ProdutoId)
                          ?? throw new NotFoundException(string.Format(ResourceErrorsMessages.ProdutoNaoEncontrado, item.ProdutoId));

            var estoqueAtualizado = produto.DiminuirEstoque(item.Quantidade);

            if (!estoqueAtualizado)
            {
                pedido.DefinirStatusAguardandoEstoque();
                throw new NotFoundException(ResourceErrorsMessages.ProdutoEstoqueErro);
            }
            await _produtoRepository.AtualizarAsync(produto);
        }
    }
}
