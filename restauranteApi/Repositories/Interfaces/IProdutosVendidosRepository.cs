using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IProdutosVendidosRepository
    {
        Task<IEnumerable<ProdutosVendidos>> GetAllAsync();
        Task<ProdutosVendidos> GetByIdAsync(int numeroItem, int vendaId); // Composite Key
        Task<bool> CreateAsync(ProdutosVendidos produtoVendido);
        Task<bool> UpdateAsync(ProdutosVendidos produtoVendido);
        Task<bool> DeleteAsync(int numeroItem, int vendaId);
    }
}
