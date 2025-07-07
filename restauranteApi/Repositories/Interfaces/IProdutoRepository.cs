using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produtos>> GetAllAsync();
        Task<Produtos> GetByIdAsync(int id);
        Task<int> CreateAsync(Produtos produto);
        Task<bool> UpdateAsync(Produtos produto);
        Task<bool> DeleteAsync(int id);
    }
}
