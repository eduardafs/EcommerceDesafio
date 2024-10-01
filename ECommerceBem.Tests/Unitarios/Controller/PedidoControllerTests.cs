using ECommerceBem.Application.Dto.Request;
using ECommerceBem.Application.Dto.Response;
using ECommerceBem.Application.Services.Interfaces;
using ECommerceBem.Controllers;
using ECommerceBem.Core.Enum;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ECommerceBem.Tests.Unitarios.Controller;

public class PedidoControllerTests
{
    private readonly Mock<IPedidoService> _pedidoServiceMock;
    private readonly PedidoController _controller;

    public PedidoControllerTests()
    {
        _pedidoServiceMock = new Mock<IPedidoService>();
        _controller = new PedidoController(_pedidoServiceMock.Object);
    }

    [Fact]
    public async Task DeveRetornarCreated_QuandoPedidoValido()
    {
        // Arrange
        var pedidoDto = new RequestCriarPedidoDto
        {
            FormaPagamento = FormaPagamentoEnum.Pix,
            Itens =
            [
                new ItemPedidoRequestDto { ProdutoId = Guid.NewGuid(), Quantidade = 2 }
            ]
        };

        var retornoCriarPedido = new ResponseCriarPedidoDto
        {
            Id = Guid.NewGuid(),
            Status = "AguardandoProcessamento",
            ValorTotal = 100.0m,
            DataPedido = DateTime.Now
        };

        _pedidoServiceMock.Setup(service => service.CriarAsync(pedidoDto))
            .ReturnsAsync(retornoCriarPedido);

        // Act
        var result = await _controller.Criar(pedidoDto) as CreatedResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(201, result.StatusCode);
        var responseValue = result.Value as ResponseCriarPedidoDto;
        Assert.NotNull(responseValue);
        Assert.Equal(retornoCriarPedido.Id, responseValue.Id);
        Assert.Equal(retornoCriarPedido.Status, responseValue.Status);
        Assert.Equal(retornoCriarPedido.ValorTotal, responseValue.ValorTotal);
    }

    [Fact]
    public async Task DeveRetornarNoContente_QuandoPagamentoProcessadoComSucesso()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _pedidoServiceMock.Setup(service => service.ProcessarPagamentoAsync(pedidoId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.ProcessarPagamento(pedidoId) as NoContentResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(204, result.StatusCode);
    }

    [Fact]
    public async Task DeveRetornarNoContente_QuandoDeletadoComSucesso()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _pedidoServiceMock.Setup(service => service.CancelarPedidoAsync(pedidoId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Deletar(pedidoId) as NoContentResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(204, result.StatusCode);
    }
    
    [Fact]
    public async Task DeveOk_QuandoBuscarPedidosComSucesso()
    {
        // Arrange
        var pedidosMock = new List<ResponsePedidoDto>
        {
            pedidoMock(StatusPedido.PagamentoConcluido, FormaPagamentoEnum.Pix),
            pedidoMock(StatusPedido.AguardandoProcessamento, FormaPagamentoEnum.CartaoCredito)
        };

        _pedidoServiceMock.Setup(service => service.BuscarTodosAsync())
            .ReturnsAsync(pedidosMock);

        // Act
        var result = await _controller.Buscar() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var retornoPedidos = result.Value as List<ResponsePedidoDto>;
        Assert.NotNull(retornoPedidos);
        Assert.Equal(2, retornoPedidos.Count);
    }

    private ResponsePedidoDto pedidoMock(StatusPedido statusPedido, FormaPagamentoEnum formaPagamento)
    {
        return new ResponsePedidoDto
        {
            Id = Guid.NewGuid(),
            Itens =
           [
               new() { NomeProduto = "Produto B", Quantidade = 2, PrecoTotal = 10 },
                new() { NomeProduto = "Produto A", Quantidade = 1, PrecoTotal = 20 }
           ],
            ValorTotal = 40,
            Status = statusPedido.GetDescription(),
            DataPedido = DateTime.Parse("2024-09-30T19:00:53.4900659"),
            FormaPagamento = formaPagamento.GetDescription()
        };
    }
}