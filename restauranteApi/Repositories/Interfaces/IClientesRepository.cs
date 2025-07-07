using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IClientesRepository
    {
        Task<IEnumerable<Clientes>> GetAllAsync();
        Task<Clientes> GetByIdAsync(int id);
        Task<int> CreateAsync(Clientes cliente);
        Task<bool> UpdateAsync(Clientes cliente);
        Task<bool> DeleteAsync(int id);
    }
}
