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
            const string sql = @"
                SELECT
                  c.Id            AS id,
                  c.Cidade        AS cidade,
                  c.EstadoId      AS estadoId,
                  c.Ativo         AS ativo,
                  c.DataCadastro  AS dataCadastro,
                  c.DataAlteracao AS dataAlteracao,

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
                FROM Cidades c
                LEFT JOIN Estados e ON e.Id = c.EstadoId
                LEFT JOIN Paises  p ON p.Id = e.PaisId;";

            using var conn = _factory.CreateConnection();

            var rows = await conn.QueryAsync<Cidades, Estados, Paises, Cidades>(
                sql,
                (c, e, p) =>
                {
                    if (e is not null)
                    {
                        e.pais = p;
                        c.estado = e;
                    }
                    return c;
                },
                splitOn: "id,id" // inicia Estados no 1º "id" após os campos de Cidades, e Paises no 2º "id"
            );

            return rows;
        }

        public async Task<Cidades?> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT
                  c.Id            AS id,
                  c.Cidade        AS cidade,
                  c.EstadoId      AS estadoId,
                  c.Ativo         AS ativo,
                  c.DataCadastro  AS dataCadastro,
                  c.DataAlteracao AS dataAlteracao,

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
                FROM Cidades c
                LEFT JOIN Estados e ON e.Id = c.EstadoId
                LEFT JOIN Paises  p ON p.Id = e.PaisId
                WHERE c.Id = @id;";

            using var conn = _factory.CreateConnection();

            var rows = await conn.QueryAsync<Cidades, Estados, Paises, Cidades>(
                sql,
                (c, e, p) =>
                {
                    if (e is not null)
                    {
                        e.pais = p;
                        c.estado = e;
                    }
                    return c;
                },
                new { id },
                splitOn: "id,id"
            );

            return rows.FirstOrDefault();
        }

        public async Task<int> CreateAsync(Cidades cidade)
        {
            const string sql = @"
            INSERT INTO Cidades (Cidade, EstadoId, Ativo, DataCadastro, DataAlteracao)
            VALUES (@cidade, @estadoId, @ativo, GETDATE(), NULL);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var conn = _factory.CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql, cidade);
        }

        public async Task<Cidades?> UpdateAsync(Cidades cidade)
        {
            const string sql = @"
            UPDATE Cidades SET
              Cidade        = @cidade,
              EstadoId      = @estadoId,
              Ativo         = @ativo,
              DataAlteracao = GETDATE()
            WHERE Id = @id;";

            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, cidade);
            if (rows == 0) return null;

            // retorna a cidade completa com estado + pais
            return await GetByIdAsync(cidade.id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = @"DELETE FROM Cidades WHERE Id = @id;";
            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, new { id });
            return rows > 0;
        }
    }
}
