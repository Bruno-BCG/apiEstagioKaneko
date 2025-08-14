using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface ICidadesRepository
    {
        Task<IEnumerable<Cidades>> GetAllAsync();
        Task<Cidades?> GetByIdAsync(int id);
        Task<int> CreateAsync(Cidades cidade);
        Task<Cidades?> UpdateAsync(Cidades cidade);
        Task<bool> DeleteAsync(int id);
    }
}
