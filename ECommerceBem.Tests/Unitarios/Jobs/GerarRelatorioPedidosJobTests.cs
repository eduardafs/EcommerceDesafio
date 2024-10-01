using ECommerceBem.Application.Jobs;
using ECommerceBem.Core.Entities;
using ECommerceBem.Core.Enum;
using ECommerceBem.Core.Interfaces.Repositories;
using Moq;
using System.Reflection;
using Xunit;

namespace ECommerceBem.Tests.Unitarios.Jobs;

public class GerarRelatorioPedidosJobTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly Mock<INotificacaoRepository> _notificacaoRepositoryMock;
    private readonly GerarRelatorioPedidosJob _gerarRelatorioPedidosJob;

    public GerarRelatorioPedidosJobTests()
    {
        _pedidoRepositoryMock = new Mock<IPedidoRepository>();
        _notificacaoRepositoryMock = new Mock<INotificacaoRepository>();

        _gerarRelatorioPedidosJob = new GerarRelatorioPedidosJob(_pedidoRepositoryMock.Object, _notificacaoRepositoryMock.Object);
    }
    private PedidoEntity criarPedidoMock(DateTime dataPedido)
    {
        var pedido = new PedidoEntity(FormaPagamentoEnum.Pix);

        typeof(PedidoEntity)
            .GetProperty(nameof(PedidoEntity.DataPedido), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            ?.SetValue(pedido, dataPedido);

        var produtoA = new ProdutoEntity("Produto A", 50m, 20, 0, 0, 0, 0);
        var produtoB = new ProdutoEntity("Produto B", 100m, 10, 0, 0, 0, 0);

        var itemA = new ItemPedidoEntity(produtoA, 2);
        var itemB = new ItemPedidoEntity(produtoB, 1);

        pedido.AdicionarItem(itemA);
        pedido.AdicionarItem(itemB);

        return pedido;
    }

    [Fact]
    public async Task DeveGerarRelatorioEAdicionarNotificacao_QuandoExistemPedidosDoDiaAnterior()
    {
        // Arrange
        var dataAnterior = DateTime.Now.AddDays(-1).Date;

        var pedidoMock = criarPedidoMock(dataAnterior);

        var pedidosMock = new List<PedidoEntity> { pedidoMock };

        _pedidoRepositoryMock.Setup(repo => repo.ObterPedidosPorDataAsync(dataAnterior))
            .ReturnsAsync(pedidosMock);

        // Act
        await _gerarRelatorioPedidosJob.Execute(null);

        // Assert
        _notificacaoRepositoryMock.Verify(repo => repo.AdicionarAsync(It.Is<NotificacaoEntity>(
            n => n.TipoNotificacao == NotificacaoEnum.Email &&
                 n.Descricao.Contains("Relatório de Pedidos do Dia Anterior:") &&
                 n.Descricao.Contains($"Pedido ID: {pedidoMock.Id}") &&
                 n.Descricao.Contains("Produto A") &&
                 n.Descricao.Contains("Produto B")
        )), Times.Once);
    }

    [Fact]
    public async Task NaoDeveAdicionarNotificacao_QuandoNaoExistemPedidosDoDiaAnterior()
    {
        // Arrange
        var dataAnterior = DateTime.Now.AddDays(-1).Date;

        _pedidoRepositoryMock.Setup(repo => repo.ObterPedidosPorDataAsync(dataAnterior))
            .ReturnsAsync(new List<PedidoEntity>());

        // Act
        await _gerarRelatorioPedidosJob.Execute(null);

        // Assert
        _notificacaoRepositoryMock.Verify(repo => repo.AdicionarAsync(It.IsAny<NotificacaoEntity>()), Times.Never);
    }
}
