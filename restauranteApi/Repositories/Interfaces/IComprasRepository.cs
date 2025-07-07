using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IComprasRepository
    {
        Task<IEnumerable<Compras>> GetAllAsync();
        Task<Compras> GetByIdAsync(char modelo, char serie, int numero, int fornecedorId); 
        Task<bool> CreateAsync(Compras compra); 
        Task<bool> UpdateAsync(Compras compra);
        Task<bool> DeleteAsync(char modelo, char serie, int numero, int fornecedorId); 
    }
}
