using ECommerceBem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceBem.Infrastructure.EntityTypeConfiguration
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<ProdutoEntity>
    {
        public void Configure(EntityTypeBuilder<ProdutoEntity> builder)
        {
            builder.ToTable("PRODUTO");

            builder
                .Property(p => p.Nome)
                .HasColumnType("VARCHAR(40)")
                .IsRequired();

            builder.Property(p => p.PrecoUnitario)
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(p => p.QuantidadeEmEstoque)
               .HasColumnType("INT")
               .IsRequired();

            builder.Property(p => p.QuantidadeMinimaParaDesconto)
               .HasColumnType("INT")
               .IsRequired();

            builder.Property(p => p.DescontoPorQuantidade)
               .HasColumnType("decimal(5, 2)")
               .IsRequired();

            builder.Property(p => p.MesComDescontoSazonal)
               .HasColumnType("INT")
               .IsRequired();

            builder.Property(p => p.DescontoSazonalPercentual)
               .HasColumnType("decimal(5, 2)")
               .IsRequired();
        }
    }
}
