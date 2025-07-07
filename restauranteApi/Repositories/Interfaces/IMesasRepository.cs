// File: IMesasRepository.cs
using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IMesasRepository
    {
        Task<IEnumerable<Mesas>> GetAllAsync();
        Task<Mesas> GetByIdAsync(int numeroMesa); // Primary key is NumeroMesa
        Task<bool> CreateAsync(Mesas mesa); // No SCOPE_IDENTITY() as PK is not identity
        Task<bool> UpdateAsync(Mesas mesa);
        Task<bool> DeleteAsync(int numeroMesa);
    }
}
