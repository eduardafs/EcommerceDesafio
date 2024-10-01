using ECommerceBem.Application.Services;
using ECommerceBem.Core.Entities;
using ECommerceBem.Infrastructure.Repositories;
using NetArchTest.Rules;
using Xunit;

namespace ECommerceBem.Tests.Arquitetura;

public class ArquiteturaTests
{
    [Fact]
    public void Dominio_DependeDeCamadasExternas_AoExecutar_RetornaErroSeHouverDependencia()
    {
        // Arrange
        var dominioAssembly = typeof(PedidoEntity).Assembly;

        // Act
        var resultadoApp = Types.InAssembly(dominioAssembly)
                             .ShouldNot()
                             .HaveDependencyOn("ECommerceBem.Application")
                             .GetResult();

        var resultadoInfra = Types.InAssembly(dominioAssembly)
                             .ShouldNot()
                             .HaveDependencyOn("ECommerceBem.Infrastructure")
                             .GetResult();

        // Assert
        Assert.True(resultadoApp.IsSuccessful, "A camada Domain não deve depender da camada Application.");
        Assert.True(resultadoInfra.IsSuccessful, "A camada Domain não deve depender da camada Infrastructure.");
    }

    [Fact]
    public void Application_DependeDeInfrastructure_AoExecutar_RetornaErroSeHouverDependencia()
    {
        // Arrange
        var applicationAssembly = typeof(PedidoService).Assembly;

        // Act
        var resultado = Types.InAssembly(applicationAssembly)
                             .ShouldNot()
                             .HaveDependencyOn("ECommerceBem.Infrastructure")
                             .GetResult();

        // Assert
        Assert.True(resultado.IsSuccessful, "A camada Application não deve depender da camada Infrastructure.");
    }

    [Fact]
    public void Infrastructure_DependeApenasDeDomain_AoExecutar_RetornaErroSeHouverDependenciaIncorreta()
    {
        // Arrange
        var infrastructureAssembly = typeof(PedidoRepository).Assembly;

        // Act
        var resultado = Types.InAssembly(infrastructureAssembly)
                             .ShouldNot()
                             .HaveDependencyOn("ECommerceBem.Application")
                             .GetResult();

        // Assert
        Assert.True(resultado.IsSuccessful, "A camada Infrastructure deve depender apenas da camada Domain.");
    }
}