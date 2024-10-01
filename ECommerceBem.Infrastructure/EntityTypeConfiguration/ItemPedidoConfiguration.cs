using ECommerceBem.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ECommerceBem.Infrastructure.EntityTypeConfiguration;

public class ItemPedidoConfiguration : IEntityTypeConfiguration<ItemPedidoEntity>
{
    public void Configure(EntityTypeBuilder<ItemPedidoEntity> builder)
    {
        builder.ToTable("ITENS_PEDIDO");

        builder
           .Property(i => i.PrecoTotal)
           .HasColumnType("decimal(5, 2)")
           .IsRequired();

        builder
            .Property(i => i.Quantidade)
            .HasColumnType("INT")
            .IsRequired();

        builder.HasOne(i => i.Produto)
              .WithMany()
              .HasForeignKey(i => i.ProdutoId)
              .OnDelete(DeleteBehavior.Restrict);

        // Configurar a relação com o Pedido
        builder.HasOne<PedidoEntity>()
               .WithMany(p => p.Itens)
               .HasForeignKey(i => i.PedidoId) // Atribuir a chave estrangeira corretamente
               .OnDelete(DeleteBehavior.Cascade); // Define o comportamento de exclusão em cascata
    }
}