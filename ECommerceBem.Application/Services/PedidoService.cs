using ECommerceBem.Application.Dto.Request;
using ECommerceBem.Application.Dto.Response;
using ECommerceBem.Application.Services.Interfaces;
using ECommerceBem.Application.Validators;
using ECommerceBem.Core.Entities;
using ECommerceBem.Core.Enum;
using ECommerceBem.Core.Interfaces.Repositories;
using ECommerceBem.Exception;
using ECommerceBem.Exception.ExceptionsBase;

namespace ECommerceBem.Application.Services;

public class PedidoService : IPedidoService
{
    private readonly int MAX_TENTATIVAS = 3;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IPagamentoService _pagamentoService;
    private readonly IEstoqueService _estoqueService;
    private readonly IEntregaService _entregaService;

    public PedidoService(IPedidoRepository pedidoRepository,
                         IProdutoRepository produtoRepository,
                         IPagamentoService pagamentoService,
                         IEstoqueService estoqueService,
                         IEntregaService entregaService)
    {
        _pedidoRepository = pedidoRepository;
        _produtoRepository = produtoRepository;
        _pagamentoService = pagamentoService;
        _estoqueService = estoqueService;
        _entregaService = entregaService;
    }

    private void validarCriarPedido(RequestCriarPedidoDto pedido)
    {
        var validar = new CriarPedidoValidator();
        var resultado = validar.Validate(pedido);

        if (resultado.IsValid == false) 
        {
            var errorMessage = resultado.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessage);
        }
    }
    public async Task<ResponseCriarPedidoDto> CriarAsync(RequestCriarPedidoDto pedidoDto)
    {
        validarCriarPedido(pedidoDto);

        var pedido = new PedidoEntity(pedidoDto.FormaPagamento);

        foreach (var itemDto in pedidoDto.Itens)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(itemDto.ProdutoId)
                ?? throw new NotFoundException(string.Format(ResourceErrorsMessages.ProdutoNaoEncontrado, itemDto.ProdutoId));

            var itemPedido = new ItemPedidoEntity(produto, itemDto.Quantidade);
            pedido.AdicionarItem(itemPedido);
        }

        await _pedidoRepository.AdicionarAsync(pedido);

        var response = new ResponseCriarPedidoDto
        {
            Id = pedido.Id,
            Status = pedido.Status.GetDescription(),
            FormaPagamento = pedido.FormaPagamento.GetDescription(),
            ValorTotal = pedido.ValorTotal,
            DataPedido = pedido.DataPedido
        };

        return response;
    }

    public async Task ProcessarPagamentoAsync(Guid pedidoId)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(pedidoId)
            ?? throw new NotFoundException(string.Format(ResourceErrorsMessages.PedidoNaoEncontrado, pedidoId));

        pedido.DefinirStatusProcessandoPagamento();

        int tentativas = 0;
        bool pagamentoProcessadoComSucesso = false;

        while (tentativas < MAX_TENTATIVAS && !pagamentoProcessadoComSucesso)
        {
            tentativas++;
            pagamentoProcessadoComSucesso = await _pagamentoService.ProcessarPagamentoAsync(pedido);

            if (pagamentoProcessadoComSucesso)
            {
                pedido.DefinirStatusPagamentoConcluido();
            }
        }

        if (!pagamentoProcessadoComSucesso)
        {
            pedido.Cancelar();
            await _pedidoRepository.AtualizarAsync(pedido);
            return;
        }

        await separarPedido(pedido);
        await _pedidoRepository.AtualizarAsync(pedido);
    }

    private async Task separarPedido(PedidoEntity pedido)
    {
        await _estoqueService.AtualizarEstoqueAsync(pedido);
        await _entregaService.ProcessarEntregaAsync(pedido);
    }

    public async Task CancelarPedidoAsync(Guid pedidoId)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(pedidoId)
            ?? throw new NotFoundException(string.Format(ResourceErrorsMessages.PedidoNaoEncontrado, pedidoId));
       
        pedido.Cancelar();

        await _pedidoRepository.AtualizarAsync(pedido);
    }

    public async Task<List<ResponsePedidoDto>> BuscarTodosAsync()
    {
        var pedidos = await _pedidoRepository.ObterTodosAsync()
            ?? throw new NotFoundException(string.Format(ResourceErrorsMessages.PedidoNaoEncontrado));

        return pedidos.Select(pedido => new ResponsePedidoDto
        {
            Id = pedido.Id,
            Status = pedido.Status.GetDescription(),
            FormaPagamento = pedido.FormaPagamento.GetDescription(),
            DataPedido = pedido.DataPedido,
            ValorTotal = pedido.ValorTotal,
            Itens = pedido.Itens.Select(item => new ItemPedidoDto
            {
                NomeProduto = item.Produto.Nome,
                PrecoTotal = item.PrecoTotal,
                Quantidade = item.Quantidade,
            }).ToList(),
        }).ToList();
    }
}
