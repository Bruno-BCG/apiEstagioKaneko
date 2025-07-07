
using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IVendasRepository
    {
        Task<IEnumerable<Vendas>> GetAllAsync();
        Task<Vendas> GetByIdAsync(int id);
        Task<int> CreateAsync(Vendas venda);
        Task<bool> UpdateAsync(Vendas venda);
        Task<bool> DeleteAsync(int id);
    }
}