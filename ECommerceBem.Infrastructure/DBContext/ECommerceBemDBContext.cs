using ECommerceBem.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceBem.Infrastructure.DBContext;

public class ECommerceBemDBContext : DbContext
{
    private readonly IMediator _mediator;

    public ECommerceBemDBContext(DbContextOptions<ECommerceBemDBContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<PedidoEntity> Pedidos { get; set; }
    public DbSet<ItemPedidoEntity> ItensPedidos { get; set; }
    public DbSet<ProdutoEntity> Produtos { get; set; }
    public DbSet<NotificacaoEntity> Notificacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ECommerceBemDBContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker
            .Entries<PedidoEntity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var entity in domainEntities)
        {
            entity.Entity.LimparEventosDominio();
        }

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }

        return result;
    }
}

