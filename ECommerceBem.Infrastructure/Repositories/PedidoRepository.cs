using ECommerceBem.Core.Entities;
using ECommerceBem.Core.Interfaces.Repositories;
using ECommerceBem.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ECommerceBem.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly ECommerceBemDBContext _context;

    public PedidoRepository(ECommerceBemDBContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(PedidoEntity pedido)
    {
        await _context.Pedidos.AddAsync(pedido);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(PedidoEntity pedido)
    {
        _context.Pedidos.Update(pedido);
        await _context.SaveChangesAsync();
        
    }

    public async Task<PedidoEntity?> ObterPorIdAsync(Guid id)
    {
        return await _context.Pedidos
            .Include(pedido => pedido.Itens)
            .FirstOrDefaultAsync(pedido => pedido.Id == id);
    }

    public async Task RemoverAsync(Guid id)
    {
        var pedido = await ObterPorIdAsync (id);
        if(pedido != null)
        {
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<PedidoEntity>> ObterPedidosPorDataAsync(DateTime data)
    {
        var pedidos =  await _context.Pedidos
                .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
                .Where(p => p.DataPedido.Date == data.Date)
                .ToListAsync();
        return pedidos;
    }

    public async Task<IEnumerable<PedidoEntity>> ObterTodosAsync()
    {
        var pedidos = await _context.Pedidos
               .Include(p => p.Itens)
               .ThenInclude(i => i.Produto)
               .ToListAsync();
        return pedidos;
    }
}
