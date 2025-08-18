// File: IFormasPagamentoRepository.cs
using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IFormasPagamentoRepository
    {
        Task<IEnumerable<FormasPagamento>> GetAllAsync();
        Task<FormasPagamento?> GetByIdAsync(int id);
        Task<int> CreateAsync(FormasPagamento forma);
        Task<FormasPagamento?> UpdateAsync(FormasPagamento forma);
        Task<bool> DeleteAsync(int id);
    }
}