using ECommerceBem.Core.Entities;

namespace ECommerceBem.Core.Interfaces.Repositories;

public interface IProdutoRepository
{
    Task AtualizarAsync(ProdutoEntity produto);
    Task<ProdutoEntity?> ObterPorIdAsync(Guid id);
}
