using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IClientesRepository
    {
        Task<IEnumerable<Clientes>> GetAllAsync();
        Task<Clientes?> GetByIdAsync(int id);
        Task<int> CreateAsync(Clientes cli);
        Task<Clientes?> UpdateAsync(Clientes cli);
        Task<bool> DeleteAsync(int id);
    }
}
