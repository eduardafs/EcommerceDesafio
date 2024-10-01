using ECommerceBem.Application.Dto.Request;
using ECommerceBem.Application.Services;
using ECommerceBem.Application.Services.Interfaces;
using ECommerceBem.Core.Entities;
using ECommerceBem.Core.Enum;
using ECommerceBem.Core.Interfaces.Repositories;
using ECommerceBem.Exception.ExceptionsBase;
using Moq;
using Xunit;

namespace ECommerceBem.Tests.Unitarios.Service;

public class PedidoServiceTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
    private readonly Mock<IPagamentoService> _pagamentoServiceMock;
    private readonly Mock<IEstoqueService> _estoqueServiceMock;
    private readonly Mock<IEntregaService> _entregaServiceMock;
    private readonly PedidoService _pedidoService;

    public PedidoServiceTests()
    {
        _pedidoRepositoryMock = new Mock<IPedidoRepository>();
        _produtoRepositoryMock = new Mock<IProdutoRepository>();
        _pagamentoServiceMock = new Mock<IPagamentoService>();
        _estoqueServiceMock = new Mock<IEstoqueService>();
        _entregaServiceMock = new Mock<IEntregaService>();

        _pedidoService = new PedidoService(
            _pedidoRepositoryMock.Object,
            _produtoRepositoryMock.Object,
            _pagamentoServiceMock.Object,
            _estoqueServiceMock.Object,
            _entregaServiceMock.Object
        );
    }

    [Fact]
    public async Task DeveCriarPedidoComSucesso()
    {
        // Arrange
        var produto = new ProdutoEntity("Produto Teste", 100.0m, 10, 0, 0, 1, 2);
        var requestDto = new RequestCriarPedidoDto
        {
            FormaPagamento = FormaPagamentoEnum.Pix,
            Itens =
            [
                new ItemPedidoRequestDto { ProdutoId = produto.Id, Quantidade = 2 }
            ]
        };

        _produtoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(produto.Id))
            .ReturnsAsync(produto);

        // Act
        var response = await _pedidoService.CriarAsync(requestDto);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FormaPagamentoEnum.Pix.GetDescription(), response.FormaPagamento);
        _pedidoRepositoryMock.Verify(repo => repo.AdicionarAsync(It.IsAny<PedidoEntity>()), Times.Once);
    }

    [Fact]
    public async Task DeveProcessarPagamentoComSucesso()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var pedido = new PedidoEntity(FormaPagamentoEnum.Pix);

        _pedidoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(pedidoId))
            .ReturnsAsync(pedido);

        _pagamentoServiceMock.Setup(service => service.ProcessarPagamentoAsync(pedido))
            .ReturnsAsync(true);

        // Act
        await _pedidoService.ProcessarPagamentoAsync(pedidoId);

        // Assert
        Assert.Equal(StatusPedido.PagamentoConcluido, pedido.Status);
        _pedidoRepositoryMock.Verify(repo => repo.AtualizarAsync(pedido), Times.Once);
    }

    [Fact]
    public async Task DeveCancelarPedido_SePagamentoFalhar()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var pedido = new PedidoEntity(FormaPagamentoEnum.Pix);

        _pedidoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(pedidoId))
            .ReturnsAsync(pedido);

        _pagamentoServiceMock.SetupSequence(service => service.ProcessarPagamentoAsync(pedido))
            .ReturnsAsync(false)
            .ReturnsAsync(false)
            .ReturnsAsync(false);

        // Act
        await _pedidoService.ProcessarPagamentoAsync(pedidoId);

        // Assert
        Assert.Equal(StatusPedido.Cancelado, pedido.Status);
        _pedidoRepositoryMock.Verify(repo => repo.AtualizarAsync(pedido), Times.Once);
    }

    [Fact]
    public async Task DeveCancelarPedidoComSucesso()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var pedido = new PedidoEntity(FormaPagamentoEnum.Pix);

        _pedidoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(pedidoId))
            .ReturnsAsync(pedido);

        // Act
        await _pedidoService.CancelarPedidoAsync(pedidoId);

        // Assert
        Assert.Equal(StatusPedido.Cancelado, pedido.Status);
        _pedidoRepositoryMock.Verify(repo => repo.AtualizarAsync(pedido), Times.Once);
    }

    [Fact]
    public async Task DeveRetornarListaPedidos()
    {
        // Arrange
        var pedidos = new List<PedidoEntity>
        {
            new(FormaPagamentoEnum.Pix),
            new(FormaPagamentoEnum.CartaoCredito)
        };

        _pedidoRepositoryMock.Setup(repo => repo.ObterTodosAsync())
            .ReturnsAsync(pedidos);

        // Act
        var result = await _pedidoService.BuscarTodosAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        _pedidoRepositoryMock.Verify(repo => repo.ObterTodosAsync(), Times.Once);
    }

    [Fact]
    public async Task DeveLancarExcecaoSeProdutoNaoExistir()
    {
        // Arrange
        var requestDto = new RequestCriarPedidoDto
        {
            FormaPagamento = FormaPagamentoEnum.Pix,
            Itens =
            [
                new ItemPedidoRequestDto { ProdutoId = Guid.NewGuid(), Quantidade = 2 }
            ]
        };

        _produtoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((ProdutoEntity)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _pedidoService.CriarAsync(requestDto));
    }
}
