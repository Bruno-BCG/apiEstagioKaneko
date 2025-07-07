using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IGrupoRepository
    {
        Task<IEnumerable<Grupo>> GetAllAsync();
        Task<Grupo> GetByIdAsync(int id);
        Task<int> CreateAsync(Grupo grupo);
        Task<bool> UpdateAsync(Grupo grupo);
        Task<bool> DeleteAsync(int id);
    }
}