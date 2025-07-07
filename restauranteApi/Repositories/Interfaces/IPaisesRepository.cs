using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{

    public interface IPaisesRepository
    {
        Task<IEnumerable<Paises>> GetAllAsync();
        Task<Paises> GetByIdAsync(int id);
        Task<int> CreateAsync(Paises pais);
        Task<bool> UpdateAsync(Paises pais);
        Task<bool> DeleteAsync(int id);
    }
}
