using Dapper;
using Microsoft.Data.SqlClient;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace restauranteApi.Repositories
{
    public class CidadesRepository : ICidadesRepository
    {
        private readonly SqlConnectionFactory _factory;

        public CidadesRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Cidades>> GetAllAsync()
        {
            const string sql = "SELECT c.*, e.Id AS EId, e.Nome AS ENome, e.Ativo AS EAtivo, e.DataCadastro AS EDataCadastro, e.DataAlteracao AS EDataAlteracao FROM Cidades c JOIN Estados e ON c.EstadoId = e.Id";
            using var connection = _factory.CreateConnection();
            return await connection.QueryAsync<Cidades, Estados, Cidades>(sql, (cidade, estado) =>
            {
                cidade.oEstado = estado;
                return cidade;
            }, splitOn: "EId");
        }

        public async Task<Cidades> GetByIdAsync(int id)
        {
            const string sql = "SELECT c.*, e.Id AS EId, e.Nome AS ENome, e.Ativo AS EAtivo, e.DataCadastro AS EDataCadastro, e.DataAlteracao AS EDataAlteracao FROM Cidades c JOIN Estados e ON c.EstadoId = e.Id WHERE c.Id = @Id";
            using var connection = _factory.CreateConnection();
            var result = await connection.QueryAsync<Cidades, Estados, Cidades>(sql, (cidade, estado) =>
            {
                cidade.oEstado = estado;
                return cidade;
            }, new { Id = id }, splitOn: "EId");

            return result.SingleOrDefault();
        }

        public async Task<int> CreateAsync(Cidades cidade)
        {
            const string sql = @"
                INSERT INTO Cidades
                    (Nome, EstadoId, Ativo, DataCadastro, DataAlteracao)
                VALUES
                    (@Nome, @EstadoId, @Ativo, GETDATE(), NULL);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = _factory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                cidade.Nome,
                EstadoId = cidade.oEstado?.Id,
                cidade.Ativo
            });
        }

        public async Task<bool> UpdateAsync(Cidades cidade)
        {
            const string sql = @"
                UPDATE Cidades SET
                    Nome = @Nome,
                    EstadoId = @EstadoId,
                    Ativo = @Ativo,
                    DataAlteracao = GETDATE()
                WHERE Id = @Id";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new
            {
                cidade.Nome,
                EstadoId = cidade.oEstado?.Id,
                cidade.Ativo,
                cidade.Id
            });
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Cidades WHERE Id = @Id";
            using var connection = _factory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}
