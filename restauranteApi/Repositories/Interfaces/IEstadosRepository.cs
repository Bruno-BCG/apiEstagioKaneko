using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{

    public interface IEstadosRepository
    {
        Task<IEnumerable<Estados>> GetAllAsync();
        Task<Estados> GetByIdAsync(int id);
        Task<int> CreateAsync(Estados estado);
        Task<bool> UpdateAsync(Estados estado);
        Task<bool> DeleteAsync(int id);
    }
   
}
