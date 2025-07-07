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
            const string sql = "SELECT * FROM FormasPagamento";
            using var connection = _factory.CreateConnection();
            return await connection.QueryAsync<FormasPagamento>(sql);
        }

        public async Task<FormasPagamento> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM FormasPagamento WHERE Id = @Id";
            using var connection = _factory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<FormasPagamento>(sql, new { Id = id });
        }

        public async Task<int> CreateAsync(FormasPagamento formaPagamento)
        {
            const string sql = @"
                INSERT INTO FormasPagamento
                    (Descricao, Ativo, DataCadastro, DataAlteracao)
                VALUES
                    (@Descricao, @Ativo, GETDATE(), NULL);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = _factory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, formaPagamento);
        }

        public async Task<bool> UpdateAsync(FormasPagamento formaPagamento)
        {
            const string sql = @"
                UPDATE FormasPagamento SET
                    Descricao = @Descricao,
                    Ativo = @Ativo,
                    DataAlteracao = GETDATE()
                WHERE Id = @Id";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, formaPagamento);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM FormasPagamento WHERE Id = @Id";
            using var connection = _factory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}