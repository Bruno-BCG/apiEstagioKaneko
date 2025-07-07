// File: CondicoesPagamentoRepository.cs
using Dapper;
using Microsoft.Data.SqlClient;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace restauranteApi.Repositories
{
    public class CondicoesPagamentoRepository : ICondicoesPagamentoRepository
    {
        private readonly SqlConnectionFactory _factory;

        public CondicoesPagamentoRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<CondicoesPagamento>> GetAllAsync()
        {
            const string sql = "SELECT * FROM CondicoesPagamento";
            using var connection = _factory.CreateConnection();
            return await connection.QueryAsync<CondicoesPagamento>(sql);
        }

        public async Task<CondicoesPagamento> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM CondicoesPagamento WHERE Id = @Id";
            using var connection = _factory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<CondicoesPagamento>(sql, new { Id = id });
        }

        public async Task<int> CreateAsync(CondicoesPagamento condicaoPagamento)
        {
            const string sql = @"
                INSERT INTO CondicoesPagamento
                    (Descricao, QuantidadeParcelas, Juros, Multa, Desconto, Ativo, DataCadastro, DataAlteracao)
                VALUES
                    (@Descricao, @QuantidadeParcelas, @Juros, @Multa, @Desconto, @Ativo, GETDATE(), NULL);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = _factory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, condicaoPagamento);
        }

        public async Task<bool> UpdateAsync(CondicoesPagamento condicaoPagamento)
        {
            const string sql = @"
                UPDATE CondicoesPagamento SET
                    Descricao = @Descricao,
                    QuantidadeParcelas = @QuantidadeParcelas,
                    Juros = @Juros,
                    Multa = @Multa,
                    Desconto = @Desconto,
                    Ativo = @Ativo,
                    DataAlteracao = GETDATE()
                WHERE Id = @Id";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, condicaoPagamento);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM CondicoesPagamento WHERE Id = @Id";
            using var connection = _factory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}