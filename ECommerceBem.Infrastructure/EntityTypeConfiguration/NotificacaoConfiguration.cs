using ECommerceBem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceBem.Infrastructure.EntityTypeConfiguration;

public class NotificacaoConfiguration : IEntityTypeConfiguration<NotificacaoEntity>
{
    public void Configure(EntityTypeBuilder<NotificacaoEntity> builder)
    {
        builder.ToTable("NOTIFICACAO");

        builder
          .Property(n => n.PedidoId)
          .HasColumnType("VARCHAR(100)")
          .IsRequired();

        builder
          .Property(n => n.TipoNotificacao)
          .HasColumnType("INT")
          .IsRequired();

        builder
          .Property(n => n.Descricao)
          .HasColumnType("VARCHAR(500)")
          .IsRequired();

        builder
            .Property(n => n.Data)
            .HasColumnType("DATETIME2")
            .IsRequired();
    }
}
