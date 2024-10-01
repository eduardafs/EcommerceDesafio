using ECommerceBem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceBem.Infrastructure.EntityTypeConfiguration;

public class PedidoConfiguration : IEntityTypeConfiguration<PedidoEntity>
{
    public void Configure(EntityTypeBuilder<PedidoEntity> builder)
    {
        builder.ToTable("PEDIDO");

        builder
            .Property(p => p.ValorTotal)
            .HasColumnType("decimal(5, 2)")
            .IsRequired();

        builder
            .Property(p => p.Status)
            .HasColumnType("VARCHAR(100)")
            .IsRequired();

        builder
            .Property(p => p.DataPedido)
            .HasColumnType("DATETIME2")
            .IsRequired();

        builder
            .Property(p => p.FormaPagamento)
            .HasColumnType("INT")
            .IsRequired();

        builder.HasMany(p => p.Itens)
               .WithOne() 
               .HasForeignKey(i => i.PedidoId) 
               .OnDelete(DeleteBehavior.Cascade); 
    }
}
