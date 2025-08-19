using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IVeiculosRepository
    {
        Task<IEnumerable<Veiculos>> GetAllAsync();
        Task<Veiculos?> GetByIdAsync(int id);
        Task<int> CreateAsync(Veiculos v);
        Task<Veiculos?> UpdateAsync(Veiculos v);
        Task<bool> DeleteAsync(int id);
    }
}
