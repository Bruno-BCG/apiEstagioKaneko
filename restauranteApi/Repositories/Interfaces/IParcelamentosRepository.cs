
using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IParcelamentosRepository
    {
        Task<IEnumerable<Parcelamentos>> GetAllAsync();
        Task<Parcelamentos> GetByIdAsync(int numeroParcela, int condicaoPagamentoId); 
        Task<bool> CreateAsync(Parcelamentos parcelamento); 
        Task<bool> UpdateAsync(Parcelamentos parcelamento);
        Task<bool> DeleteAsync(int numeroParcela, int condicaoPagamentoId); 
    }
}