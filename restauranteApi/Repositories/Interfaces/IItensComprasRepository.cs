using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IItensComprasRepository
    {
        Task<IEnumerable<ItensCompras>> GetAllAsync();
        Task<ItensCompras> GetByIdAsync(int id);
        Task<int> CreateAsync(ItensCompras itemCompra);
        Task<bool> UpdateAsync(ItensCompras itemCompra);
        Task<bool> DeleteAsync(int id);
    }
}