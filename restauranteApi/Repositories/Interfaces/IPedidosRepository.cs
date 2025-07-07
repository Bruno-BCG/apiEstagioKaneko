using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IPedidosRepository
    {
        Task<IEnumerable<Pedidos>> GetAllAsync();
        Task<Pedidos> GetByIdAsync(int id);
        Task<int> CreateAsync(Pedidos pedido);
        Task<bool> UpdateAsync(Pedidos pedido);
        Task<bool> DeleteAsync(int id);
    }
}
