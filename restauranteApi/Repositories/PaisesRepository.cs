using Dapper;
using Microsoft.Data.SqlClient;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace restauranteApi.Repositories
{
    public class PaisesRepository : IPaisesRepository
    {
        private readonly SqlConnectionFactory _factory;

        public PaisesRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Paises>> GetAllAsync()
        {
            const string sql = @"
                SELECT
                    Id            AS id,
                    Pais          AS pais,
                    Ativo         AS ativo,
                    DataCadastro  AS dataCadastro,
                    DataAlteracao AS dataAlteracao
                FROM Paises;";

            using var connection = _factory.CreateConnection();
            return await connection.QueryAsync<Paises>(sql);
        }

        public async Task<Paises?> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT
                    Id            AS id,
                    Pais          AS pais,
                    Ativo         AS ativo,
                    DataCadastro  AS dataCadastro,
                    DataAlteracao AS dataAlteracao
                FROM Paises
                WHERE Id = @id;";

            using var connection = _factory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Paises>(sql, new { id });
        }

        public async Task<int> CreateAsync(Paises pais)
        {
            const string sql = @"
                INSERT INTO Paises (Pais, Ativo, DataCadastro, DataAlteracao)
                VALUES (@pais, @ativo, GETDATE(), NULL);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = _factory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, pais);
        }

        public async Task<bool> UpdateAsync(Paises pais)
        {
            const string sql = @"
                UPDATE Paises SET
                    Pais          = @pais,
                    Ativo         = @ativo,
                    DataAlteracao = GETDATE()
                WHERE Id = @id;";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, pais);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Paises WHERE Id = @id;";
            using var connection = _factory.CreateConnection();
            var rows = await connection.ExecuteAsync(sql, new { id });
            return rows > 0;
        }
    }
}
