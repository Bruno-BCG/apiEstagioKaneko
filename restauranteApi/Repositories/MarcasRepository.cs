using Dapper;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;

namespace restauranteApi.Repositories
{
    public class MarcasRepository : IMarcasRepository
    {
        private readonly SqlConnectionFactory _factory;
        public MarcasRepository(SqlConnectionFactory factory) => _factory = factory;

        public async Task<IEnumerable<Marcas>> GetAllAsync()
        {
            const string sql = @"
            SELECT Id AS id, Marca AS marca, Ativo AS ativo, DataCadastro AS dataCadastro, DataAlteracao AS dataAlteracao
            FROM Marcas;";
            using var conn = _factory.CreateConnection();
            return await conn.QueryAsync<Marcas>(sql);
        }

        public async Task<Marcas?> GetByIdAsync(int id)
        {
            const string sql = @"
            SELECT Id AS id, Marca AS marca, Ativo AS ativo, DataCadastro AS dataCadastro, DataAlteracao AS dataAlteracao
            FROM Marcas WHERE Id=@id;";
            using var conn = _factory.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Marcas>(sql, new { id });
        }

        public async Task<int> CreateAsync(Marcas marca)
        {
            const string sql = @"
            INSERT INTO Marcas (Marca, Ativo, DataCadastro, DataAlteracao)
            VALUES (@marca, @ativo, GETDATE(), NULL);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";
            using var conn = _factory.CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql, marca);
        }

        public async Task<Marcas?> UpdateAsync(Marcas marca)
        {
            const string sql = @"
            UPDATE Marcas SET Marca=@marca, Ativo=@ativo, DataAlteracao=GETDATE()
            WHERE Id=@id;";
            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, marca);
            if (rows == 0) return null;
            return await GetByIdAsync(marca.id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Marcas WHERE Id=@id;";
            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, new { id });
            return rows > 0;
        }
    }
}

