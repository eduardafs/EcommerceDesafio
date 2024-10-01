using ECommerceBem.Core.Entities;
using ECommerceBem.Core.Enum;
using ECommerceBem.Core.Events;
using ECommerceBem.Exception;
using ECommerceBem.Exception.ExceptionsBase;
using FluentAssertions;
using Xunit;

namespace ECommerceBem.Tests.Unitarios.Entities;

public class PedidoEntityTest
{
    [Fact]
    public void DeveAdicionarItemAoPedido()
    {
        // Arrange
        var pedido = new PedidoEntity(FormaPagamentoEnum.Pix);
        var item = new ItemPedidoEntity(new ProdutoEntity("Produto A", 10.0m, 10, 0, 0, 0, 2), 4);

        // Act
        pedido.AdicionarItem(item);

        // Assert
        pedido.Itens.Should().ContainSingle(i => i == item);
    }

    [Fact]
    public void DeveCalcularValorTotalDoPedido()
    {
        // Arrange
        var pedido = new PedidoEntity(FormaPagamentoEnum.CartaoCredito);
        var item1 = new ItemPedidoEntity(new ProdutoEntity("Produto A", 10.0m, 10, 0, 0, 0, 2), 2);
        var item2 = new ItemPedidoEntity(new ProdutoEntity("Produto A", 20.0m, 10, 0, 0, 0, 2), 1);
        
        // Act
        pedido.AdicionarItem(item1);
        pedido.AdicionarItem(item2);

        // Assert
        pedido.ValorTotal.Should().Be(40.0m); // 2 x 10.0 + 1 x 20.0
        pedido.DomainEvents.Should().ContainSingle(e => e is NotificacaoEvent);
    }

    [Fact]
    public void DeveMudarStatusParaProcessandoPagamento()
    {
        // Arrange
        var pedido = new PedidoEntity(FormaPagamentoEnum.Pix);

        // Act
        pedido.DefinirStatusProcessandoPagamento();

        // Assert
        pedido.Status.Should().Be(StatusPedido.ProcessandoPagamento);
        pedido.DomainEvents.Count(e => e is NotificacaoEvent).Should().Be(2);
    }

    [Fact]
    public void DeveAplicarDescontoAoPedido_DescontoPix()
    {
        // Arrange
        var pedido = new PedidoEntity(FormaPagamentoEnum.Pix);
        var item = new ItemPedidoEntity(new ProdutoEntity("Produto A", 100.0m, 10, 100, 1, 2, 2), 1);
        pedido.AdicionarItem(item);

        // Act
        pedido.AplicarDescontoPix(0.05m);

        // Assert
        pedido.ValorTotal.Should().Be(95.0m); // 100 - 5% desconto
    }

    [Fact]
    public void DeveMudarStatusParaPagamentoConcluido()
    {
        // Arrange
        var pedido = new PedidoEntity(FormaPagamentoEnum.Pix);
        pedido.DefinirStatusProcessandoPagamento();

        // Act
        pedido.DefinirStatusPagamentoConcluido();

        // Assert
        pedido.Status.Should().Be(StatusPedido.PagamentoConcluido);
        pedido.DomainEvents.Count(e => e is NotificacaoEvent).Should().Be(3);
    }

    [Fact]
    public void DeveMudarStatusParaConcluido()
    {
        // Arrange
        var pedido = new PedidoEntity(FormaPagamentoEnum.Pix);
        pedido.DefinirStatusProcessandoPagamento();
        pedido.DefinirStatusPagamentoConcluido();
        pedido.DefinirStatusSeparandoPedido();

        // Act
        pedido.DefinirStatusConcluido();

        // Assert
        pedido.Status.Should().Be(StatusPedido.Concluido);
        pedido.DomainEvents.Count(e => e is NotificacaoEvent).Should().Be(5);
    }

    [Fact]
    public void DeveLancarExcecao_SeStatusIncorreto()
    {
        // Arrange
        var pedido = new PedidoEntity(FormaPagamentoEnum.Pix);

        // Act
        Action action = () => pedido.DefinirStatusSeparandoPedido();

        // Assert
        action.Should().Throw<NotFoundException>().WithMessage(string.Format(ResourceErrorsMessages.PedidoStatusErro, StatusPedido.AguardandoProcessamento));
    }

    [Fact]
    public void DeveMudarStatusParaAguardandoEstoque()
    {
        // Arrange
        var pedido = new PedidoEntity(FormaPagamentoEnum.Pix);
        pedido.DefinirStatusProcessandoPagamento();
        pedido.DefinirStatusPagamentoConcluido();
        pedido.DefinirStatusSeparandoPedido();

        // Act
        pedido.DefinirStatusAguardandoEstoque();

        // Assert
        pedido.Status.Should().Be(StatusPedido.AguardandoEstoque);
        pedido.DomainEvents.Count(e => e is NotificacaoEvent).Should().Be(6); // 1 A mais pois envia e-mail para vendas
    }

    [Fact]
    public void DeveLimparEventos()
    {
        // Arrange
        var pedido = new PedidoEntity(FormaPagamentoEnum.Pix);
        var item = new ItemPedidoEntity(new ProdutoEntity("Produto A", 100.0m, 10, 1, 1, 2, 2), 1);
        pedido.AdicionarItem(item);

        // Act
        pedido.LimparEventosDominio();

        // Assert
        pedido.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void DeveInicializarComEstadoAguardandoProcessamento()
    {
        // Arrange & Act
        var pedido = new PedidoEntity(FormaPagamentoEnum.Pix);

        // Assert
        pedido.Status.Should().Be(StatusPedido.AguardandoProcessamento);
        pedido.DomainEvents.Count(e => e is NotificacaoEvent).Should().Be(1);
    }

    [Fact]
    public void DeveMudarStatusParaCancelado_QuandoStatusAguardandoProcessamento()
    {
        // Arrange
        var pedido = new PedidoEntity(FormaPagamentoEnum.Pix);

        // Act
        pedido.Cancelar();

        // Assert
        pedido.Status.Should().Be(StatusPedido.Cancelado);
        pedido.DomainEvents.Count(e => e is NotificacaoEvent).Should().Be(2);
    }

    [Fact]
    public void DeveMudarStatusParaCanceladoComEstorno_QuandoStatusProcessandoPagamento()
    {
        // Arrange
        var pedido = new PedidoEntity(FormaPagamentoEnum.Pix);
        pedido.DefinirStatusProcessandoPagamento();

        // Act
        pedido.Cancelar();

        // Assert
        pedido.Status.Should().Be(StatusPedido.Cancelado);
        pedido.DomainEvents.Count(e => e is NotificacaoEvent).Should().Be(3);
    }

    [Fact]
    public void DeveLancarExcecao_QuandoStatusConcluido()
    {
        // Arrange
        var pedido = new PedidoEntity(FormaPagamentoEnum.Pix);
        pedido.DefinirStatusProcessandoPagamento();
        pedido.DefinirStatusPagamentoConcluido();
        pedido.DefinirStatusSeparandoPedido();
        pedido.DefinirStatusConcluido();

        // Act
        Action action = () => pedido.Cancelar();

        // Assert
        action.Should().Throw<NotFoundException>().WithMessage(ResourceErrorsMessages.ErroCancelamento);
    }
}
