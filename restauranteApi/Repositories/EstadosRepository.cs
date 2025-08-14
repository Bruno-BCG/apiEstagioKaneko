
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
            const string sql = @"
            SELECT
                e.Id            AS id,
                e.Estado        AS estado,
                e.UF            AS uf,
                e.PaisId        AS paisId,
                e.Ativo         AS ativo,
                e.DataCadastro  AS dataCadastro,
                e.DataAlteracao AS dataAlteracao,

                p.Id            AS id,
                p.Pais          AS pais,
                p.Ativo         AS ativo,
                p.DataCadastro  AS dataCadastro,
                p.DataAlteracao AS dataAlteracao
            FROM Estados e
            LEFT JOIN Paises p ON p.Id = e.PaisId;";

            using var conn = _factory.CreateConnection();

            // Multi-mapping: Estados (primeiro bloco), Paises (segundo bloco)
            var list = await conn.QueryAsync<Estados, Paises, Estados>(
                sql,
                (e, p) =>
                {
                    e.pais = p;
                    return e;
                },
                splitOn: "id" // divide quando encontrar o segundo "id" (o do País)
            );

            return list;
        }

        public async Task<Estados?> GetByIdAsync(int id)
        {
            const string sql = @"
            SELECT
                e.Id            AS id,
                e.Estado        AS estado,
                e.UF            AS uf,
                e.PaisId        AS paisId,
                e.Ativo         AS ativo,
                e.DataCadastro  AS dataCadastro,
                e.DataAlteracao AS dataAlteracao,

                p.Id            AS id,
                p.Pais          AS pais,
                p.Ativo         AS ativo,
                p.DataCadastro  AS dataCadastro,
                p.DataAlteracao AS dataAlteracao
            FROM Estados e
            LEFT JOIN Paises p ON p.Id = e.PaisId
            WHERE e.Id = @id;";

            using var conn = _factory.CreateConnection();

            var result = await conn.QueryAsync<Estados, Paises, Estados>(
                sql,
                (e, p) =>
                {
                    e.pais = p;
                    return e;
                },
                new { id },
                splitOn: "id"
            );

            return result.FirstOrDefault();
        }

        public async Task<int> CreateAsync(Estados estado)
        {
            const string sql = @"
                INSERT INTO Estados (Estado, UF, PaisId, Ativo, DataCadastro, DataAlteracao)
                VALUES (@estado, @uf, @paisId, @ativo, GETDATE(), NULL);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var conn = _factory.CreateConnection();
            var newId = await conn.ExecuteScalarAsync<int>(sql, estado);
            return newId;
        }

        public async Task<Estados?> UpdateAsync(Estados estado)
        {
            const string sql = @"
            UPDATE Estados SET
                Estado        = @estado,
                UF            = @uf,
                PaisId        = @paisId,
                Ativo         = @ativo,
                DataAlteracao = GETDATE()
            WHERE Id = @id;";

            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, estado);
            if (rows == 0) return null;

            // retorna o registro completo (com pais aninhado)
            return await GetByIdAsync(estado.id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Estados WHERE Id = @id;";
            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, new { id });
            return rows > 0;
        }
    }
}