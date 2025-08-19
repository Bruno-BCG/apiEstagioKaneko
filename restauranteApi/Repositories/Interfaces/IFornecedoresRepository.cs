using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IFornecedoresRepository
    {
        Task<IEnumerable<Fornecedores>> GetAllAsync();
        Task<Fornecedores?> GetByIdAsync(int id);
        Task<int> CreateAsync(Fornecedores forn);
        Task<Fornecedores?> UpdateAsync(Fornecedores forn);
        Task<bool> DeleteAsync(int id);
    }
}
