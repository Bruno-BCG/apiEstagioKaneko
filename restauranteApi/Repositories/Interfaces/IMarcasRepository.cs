using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IMarcasRepository
    {
        Task<IEnumerable<Marcas>> GetAllAsync();
        Task<Marcas?> GetByIdAsync(int id);
        Task<int> CreateAsync(Marcas marca);
        Task<Marcas?> UpdateAsync(Marcas marca);
        Task<bool> DeleteAsync(int id);
    } 
}
