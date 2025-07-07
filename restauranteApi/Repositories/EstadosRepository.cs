
using Dapper;
using Microsoft.Data.SqlClient;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace restauranteApi.Repositories
{
    public class EstadosRepository : IEstadosRepository
    {
        private readonly SqlConnectionFactory _factory;

        public EstadosRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Estados>> GetAllAsync()
        {
            // Dapper multi-mapping to load related Pais object
            const string sql = "SELECT e.*, p.Id AS PId, p.Nome AS PNome, p.Ativo AS PAtivo, p.DataCadastro AS PDataCadastro, p.DataAlteracao AS PDataAlteracao FROM Estados e JOIN Paises p ON e.PaisId = p.Id";
            using var connection = _factory.CreateConnection();
            return await connection.QueryAsync<Estados, Paises, Estados>(sql, (estado, pais) =>
            {
                estado.oPais = pais;
                return estado;
            }, splitOn: "PId"); // "PId" indicates where the "Paises" object starts in the result set
        }

        public async Task<Estados> GetByIdAsync(int id)
        {
            const string sql = "SELECT e.*, p.Id AS PId, p.Nome AS PNome, p.Ativo AS PAtivo, p.DataCadastro AS PDataCadastro, p.DataAlteracao AS PDataAlteracao FROM Estados e JOIN Paises p ON e.PaisId = p.Id WHERE e.Id = @Id";
            using var connection = _factory.CreateConnection();
            var result = await connection.QueryAsync<Estados, Paises, Estados>(sql, (estado, pais) =>
            {
                estado.oPais = pais;
                return estado;
            }, new { Id = id }, splitOn: "PId");

            return result.SingleOrDefault();
        }
        public async Task<int> CreateAsync(Estados estado)
        {
            const string sql = @"
                INSERT INTO Estados
                    (Nome, PaisId, Ativo, DataCadastro, DataAlteracao)
                VALUES
                    (@Nome, @PaisId, @Ativo, GETDATE(), NULL);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = _factory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                estado.Nome,
                PaisId = estado.oPais?.Id, // Access Id from the related object
                estado.Ativo
            });
        }

        public async Task<bool> UpdateAsync(Estados estado)
        {
            const string sql = @"
                UPDATE Estados SET
                    Nome = @Nome,
                    PaisId = @PaisId,
                    Ativo = @Ativo,
                    DataAlteracao = GETDATE()
                WHERE Id = @Id";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new
            {
                estado.Nome,
                PaisId = estado.oPais?.Id, // Access Id from the related object
                estado.Ativo,
                estado.Id
            });
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Estados WHERE Id = @Id";
            using var connection = _factory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}