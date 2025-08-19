using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IGruposRepository
    {
        Task<IEnumerable<Grupos>> GetAllAsync();
        Task<Grupos?> GetByIdAsync(int id);
        Task<int> CreateAsync(Grupos grupo);
        Task<Grupos?> UpdateAsync(Grupos grupo);
        Task<bool> DeleteAsync(int id);
    }
}