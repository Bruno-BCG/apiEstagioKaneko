
using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IParcelamentosRepository
    {
        Task<Parcelamentos?> GetByAsync(int condicoesPagamentoId, int numeroParcela);
        Task<IEnumerable<Parcelamentos>> GetByCondicaoIdAsync(int condicoesPagamentoId);
        Task<int> CreateAsync(Parcelamentos parcela);
        Task<Parcelamentos?> UpdateAsync(Parcelamentos parcela);

        Task<bool> DeleteAsync(int condicoesPagamentoId, int numeroParcela);
    }
}