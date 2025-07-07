using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IItensPedidosRepository
    {
        Task<IEnumerable<ItensPedidos>> GetAllAsync();
        Task<ItensPedidos> GetByIdAsync(int pedidosId, int numeroItem); // Composite Key
        Task<bool> CreateAsync(ItensPedidos itemPedido);
        Task<bool> UpdateAsync(ItensPedidos itemPedido);
        Task<bool> DeleteAsync(int pedidosId, int numeroItem);
    }
}