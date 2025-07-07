using restauranteApi.Models;

namespace restauranteApi.Repositories.Interfaces
{
    public interface IFuncionariosRepository
    {
        Task<IEnumerable<Funcionarios>> GetAllAsync();
        Task<Funcionarios> GetByIdAsync(int id);
        Task<int> CreateAsync(Funcionarios funcionario);
        Task<bool> UpdateAsync(Funcionarios funcionario);
        Task<bool> DeleteAsync(int id);
    }
}
