using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface ICondicoesPagamentoRepository
    {
        Task<IEnumerable<CondicoesPagamento>> GetAllAsync();
        Task<CondicoesPagamento> GetByIdAsync(int id);
        Task<int> CreateAsync(CondicoesPagamento condicaoPagamento);
        Task<bool> UpdateAsync(CondicoesPagamento condicaoPagamento);
        Task<bool> DeleteAsync(int id);
    }
}