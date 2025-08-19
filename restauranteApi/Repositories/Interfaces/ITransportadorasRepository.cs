using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface ITransportadorasRepository
    {
        Task<IEnumerable<Transportadoras>> GetAllAsync();
        Task<Transportadoras?> GetByIdAsync(int id);
        Task<int> CreateAsync(Transportadoras t);
        Task<Transportadoras?> UpdateAsync(Transportadoras t);
        Task<bool> DeleteAsync(int id);
    }
}
