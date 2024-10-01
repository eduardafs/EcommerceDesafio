using ECommerceBem.Core.Entities;
using ECommerceBem.Core.Enum;
using ECommerceBem.Infrastructure.DBContext;
using ECommerceBem.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace ECommerceBem.Tests.Unitarios.Repository;

public class PedidoRepositoryTests
{
    private readonly ECommerceBemDBContext _context;
    private readonly PedidoRepository _pedidoRepository;

    public PedidoRepositoryTests()
    {
        var mediatorMock = new Mock<IMediator>();

        var options = new DbContextOptionsBuilder<ECommerceBemDBContext>()
            .UseInMemoryDatabase(databaseName: "ECommerceBemTestDb")
            .Options;

        _context = new ECommerceBemDBContext(options, mediatorMock.Object);
        _pedidoRepository = new PedidoRepository(_context);

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        // Adicionando produtos para teste
        var produtoA = new ProdutoEntity("Produto A", 100.0m, 50, 10, 5, 1, 2);
        var produtoB = new ProdutoEntity("Produto B", 50.0m, 100, 20, 10, 1, 3);

        // Adicionando um pedido
        var pedido = new PedidoEntity(FormaPagamentoEnum.Pix);
        pedido.AdicionarItem(new ItemPedidoEntity(produtoA, 2));
        pedido.AdicionarItem(new ItemPedidoEntity(produtoB, 1));

        _context.Produtos.AddRange(produtoA, produtoB);
        _context.Pedidos.Add(pedido);
        _context.SaveChanges();
    }

    [Fact]
    public async Task DeveRetornarPedidoQuandoExistir()
    {
        // Arrange
        var pedidoExistente = _context.Pedidos.First();

        // Act
        var pedido = await _pedidoRepository.ObterPorIdAsync(pedidoExistente.Id);

        // Assert
        Assert.NotNull(pedido);
        Assert.Equal(pedidoExistente.Id, pedido.Id);
    }

    [Fact]
    public async Task DeveRetornarTodosOsPedidos()
    {
        // Act
        var pedidos = await _pedidoRepository.ObterTodosAsync();

        // Assert
        Assert.NotNull(pedidos);
        Assert.NotEmpty(pedidos);
    }

    [Fact]
    public async Task DeveAdicionarPedido()
    {
        // Arrange
        var novoPedido = new PedidoEntity(FormaPagamentoEnum.Pix);
        var produto = _context.Produtos.First();
        novoPedido.AdicionarItem(new ItemPedidoEntity(produto, 5));

        // Act
        await _pedidoRepository.AdicionarAsync(novoPedido);
        var pedidoAdicionado = await _pedidoRepository.ObterPorIdAsync(novoPedido.Id);

        // Assert
        Assert.NotNull(pedidoAdicionado);
        Assert.Equal(novoPedido.Id, pedidoAdicionado.Id);
        Assert.Single(pedidoAdicionado.Itens);
    }

    [Fact]
    public async Task DeveAtualizarPedido()
    {
        // Arrange
        var pedido = _context.Pedidos.First();
        pedido.DefinirStatusProcessandoPagamento();

        // Act
        await _pedidoRepository.AtualizarAsync(pedido);
        var pedidoAtualizado = await _pedidoRepository.ObterPorIdAsync(pedido.Id);

        // Assert
        Assert.Equal(StatusPedido.ProcessandoPagamento, pedidoAtualizado.Status);
    }

    [Fact]
    public async Task DeveRemoverPedido()
    {
        // Arrange
        var pedido = _context.Pedidos.First();

        // Act
        await _pedidoRepository.RemoverAsync(pedido.Id);
        var pedidoRemovido = await _pedidoRepository.ObterPorIdAsync(pedido.Id);

        // Assert
        Assert.Null(pedidoRemovido);
    }
}
