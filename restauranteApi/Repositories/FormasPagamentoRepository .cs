// File: FormasPagamentoRepository.cs
using Dapper;
using Microsoft.Data.SqlClient;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace restauranteApi.Repositories
{
    public class FormasPagamentoRepository : IFormasPagamentoRepository
    {
        private readonly SqlConnectionFactory _factory;

        public FormasPagamentoRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<FormasPagamento>> GetAllAsync()
        {
            const string sql = @"
            SELECT
              Id            AS id,
              Descricao     AS descricao,
              Ativo         AS ativo,
              DataCadastro  AS dataCadastro,
              DataAlteracao AS dataAlteracao
            FROM FormasPagamento;";

            using var conn = _factory.CreateConnection();
            return await conn.QueryAsync<FormasPagamento>(sql);
        }

        public async Task<FormasPagamento?> GetByIdAsync(int id)
        {
            const string sql = @"
            SELECT
              Id            AS id,
              Descricao     AS descricao,
              Ativo         AS ativo,
              DataCadastro  AS dataCadastro,
              DataAlteracao AS dataAlteracao
            FROM FormasPagamento
            WHERE Id = @id;";

            using var conn = _factory.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<FormasPagamento>(sql, new { id });
        }

        public async Task<int> CreateAsync(FormasPagamento forma)
        {
            const string sql = @"
            INSERT INTO FormasPagamento (Descricao, Ativo, DataCadastro, DataAlteracao)
            VALUES (@descricao, @ativo, GETDATE(), NULL);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var conn = _factory.CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql, forma);
        }

        public async Task<FormasPagamento?> UpdateAsync(FormasPagamento forma)
        {
            const string sql = @"
            UPDATE FormasPagamento SET
              Descricao     = @descricao,
              Ativo         = @ativo,
              DataAlteracao = GETDATE()
            WHERE Id = @id;";

            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, forma);
            if (rows == 0) return null;

            return await GetByIdAsync(forma.id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = @"DELETE FROM FormasPagamento WHERE Id = @id;";
            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, new { id });
            return rows > 0;
        }
    }
}