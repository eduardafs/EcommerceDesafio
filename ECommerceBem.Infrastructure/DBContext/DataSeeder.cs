using ECommerceBem.Core.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceBem.Infrastructure.DBContext;

public class DataSeeder
{
    public static List<ProdutoEntity> ProdutosInseridos { get; private set; } = [];

    public static void Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ECommerceBemDBContext>();

        if (context.Produtos.Any()) return;

        var produtos = new[]
        {
            new ProdutoEntity("Produto A", 10.0m, 100, 10, 1.0m, 12, 0.10m), // Desconto de R$1 por unidade se comprar 10 ou mais, e 10% de desconto em dezembro
            new ProdutoEntity("Produto B", 20.0m, 50, 15, 2.0m, 6, 0.15m),   // Desconto de R$2 por unidade se comprar 15 ou mais, e 15% de desconto em junho
            new ProdutoEntity("Produto C", 30.0m, 20, 20, 3.0m, 12, 0.20m)   // Desconto de R$3 por unidade se comprar 20 ou mais, e 20% de desconto em dezembro
        };

        context.Produtos.AddRange(produtos);
        context.SaveChanges();

        ProdutosInseridos = produtos.ToList();
    }
}
