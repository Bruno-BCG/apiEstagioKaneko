// File: GrupoRepository.cs
using Dapper;
using Microsoft.Data.SqlClient;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace restauranteApi.Repositories
{
    public class GrupoRepository : IGrupoRepository
    {
        private readonly SqlConnectionFactory _factory;

        public GrupoRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Grupos>> GetAllAsync()
        {
            const string sql = "SELECT * FROM Grupo";
            using var connection = _factory.CreateConnection();
            return await connection.QueryAsync<Grupos>(sql);
        }

        public async Task<Grupos> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM Grupo WHERE Id = @Id";
            using var connection = _factory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Grupos>(sql, new { Id = id });
        }

        public async Task<int> CreateAsync(Grupos grupo)
        {
            const string sql = @"
                INSERT INTO Grupo
                    (Nome, Descricao, IpImpressora, Ativo, DataCadastro, DataAlteracao)
                VALUES
                    (@Nome, @Descricao, @IpImpressora, @Ativo, GETDATE(), NULL);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = _factory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, grupo);
        }

        public async Task<bool> UpdateAsync(Grupos grupo)
        {
            const string sql = @"
                UPDATE Grupo SET
                    Nome = @Nome,
                    Descricao = @Descricao,
                    IpImpressora = @IpImpressora,
                    Ativo = @Ativo,
                    DataAlteracao = GETDATE()
                WHERE Id = @Id";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, grupo);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Grupo WHERE Id = @Id";
            using var connection = _factory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}