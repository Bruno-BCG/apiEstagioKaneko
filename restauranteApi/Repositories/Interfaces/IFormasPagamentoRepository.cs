// File: IFormasPagamentoRepository.cs
using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IFormasPagamentoRepository
    {
        Task<IEnumerable<FormasPagamento>> GetAllAsync();
        Task<FormasPagamento> GetByIdAsync(int id);
        Task<int> CreateAsync(FormasPagamento formaPagamento);
        Task<bool> UpdateAsync(FormasPagamento formaPagamento);
        Task<bool> DeleteAsync(int id);
    }
}