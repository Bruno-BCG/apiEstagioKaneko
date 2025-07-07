using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IFornecedoresRepository
    {
        Task<IEnumerable<Fornecedores>> GetAllAsync();
        Task<Fornecedores> GetByIdAsync(int id);
        Task<int> CreateAsync(Fornecedores fornecedor);
        Task<bool> UpdateAsync(Fornecedores fornecedor);
        Task<bool> DeleteAsync(int id);
    }
}
