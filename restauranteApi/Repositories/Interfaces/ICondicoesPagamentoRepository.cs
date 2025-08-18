using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface ICondicoesPagamentoRepository
    {
        Task<IEnumerable<CondicoesPagamento>> GetAllAsync();
        Task<CondicoesPagamento?> GetByIdAsync(int id);
        Task<int> CreateAsync(CondicoesPagamento condicao);
        Task<CondicoesPagamento?> UpdateAsync(CondicoesPagamento condicao);
        Task<bool> DeleteAsync(int id);
    }
}