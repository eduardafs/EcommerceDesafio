using ECommerceBem.Core.Entities;
using ECommerceBem.Core.Interfaces.Repositories;
using ECommerceBem.Infrastructure.DBContext;

namespace ECommerceBem.Infrastructure.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly ECommerceBemDBContext _context;

    public ProdutoRepository(ECommerceBemDBContext context)
    {
        _context = context;
    }

    public async Task AtualizarAsync(ProdutoEntity produto)
    {
        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync();
    }

    public async Task<ProdutoEntity?> ObterPorIdAsync(Guid id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        return produto;
    }
}
