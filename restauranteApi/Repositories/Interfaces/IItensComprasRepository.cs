using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IItensComprasRepository
    {
        Task<IEnumerable<itensPedidos>> GetAllAsync();
        Task<itensPedidos> GetByIdAsync(int id);
        Task<int> CreateAsync(itensPedidos itemCompra);
        Task<bool> UpdateAsync(itensPedidos itemCompra);
        Task<bool> DeleteAsync(int id);
    }
}