// File: ParcelamentosRepository.cs
using Dapper;
using Microsoft.Data.SqlClient;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace restauranteApi.Repositories
{
    public class ParcelamentosRepository : IParcelamentosRepository
    {
        private readonly SqlConnectionFactory _factory;

        public ParcelamentosRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Parcelamentos>> GetAllAsync()
        {
            const string sql = "SELECT p.*, cp.Id AS CPId, cp.Descricao AS CPDescricao, fp.Id AS FPId, fp.Descricao AS FPDescricao FROM Parcelamentos p JOIN CondicoesPagamento cp ON p.CondicaoPagamentoId = cp.Id JOIN FormasPagamento fp ON p.FormaPagamentoId = fp.Id";
            using var connection = _factory.CreateConnection();
            return await connection.QueryAsync<Parcelamentos, CondicoesPagamento, FormasPagamento, Parcelamentos>(
                sql,
                (parcelamento, condicaoPagamento, formaPagamento) => {
                    parcelamento.oCondicaoPagamento = condicaoPagamento;
                    parcelamento.oFormaPagamento = formaPagamento;
                    return parcelamento;
                },
                splitOn: "CPId,FPId"
            );
        }

        public async Task<Parcelamentos> GetByIdAsync(int numeroParcela, int condicaoPagamentoId)
        {
            const string sql = @"
            SELECT p.*, 
                   cp.Id AS CPId, cp.Descricao AS CPDescricao, 
                   fp.Id AS FPId, fp.Descricao AS FPDescricao 
            FROM Parcelamentos p 
            JOIN CondicoesPagamento cp ON p.CondicaoPagamentoId = cp.Id 
            JOIN FormasPagamento fp ON p.FormaPagamentoId = fp.Id 
            WHERE p.NumeroParcela = @NumeroParcela AND p.CondicaoPagamentoId = @CondicaoPagamentoId";

            using var connection = _factory.CreateConnection();

            var results = await connection.QueryAsync<Parcelamentos, CondicoesPagamento, FormasPagamento, Parcelamentos>(
                sql,
                (parcelamento, condicaoPagamento, formaPagamento) =>
                {
                    parcelamento.oCondicaoPagamento = condicaoPagamento;
                    parcelamento.oFormaPagamento = formaPagamento;
                    return parcelamento;
                },
                new { NumeroParcela = numeroParcela, CondicaoPagamentoId = condicaoPagamentoId },
                splitOn: "CPId,FPId");
            return results.SingleOrDefault();
        }
        public async Task<bool> CreateAsync(Parcelamentos parcelamento)
        {
            const string sql = @"
                INSERT INTO Parcelamentos
                    (NumeroParcela, CondicaoPagamentoId, FormaPagamentoId, PrazoDias, PorcentagemValor)
                VALUES
                    (@NumeroParcela, @CondicaoPagamentoId, @FormaPagamentoId, @PrazoDias, @PorcentagemValor);";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new
            {
                parcelamento.NumeroParcela,
                CondicaoPagamentoId = parcelamento.oCondicaoPagamento?.Id,
                FormaPagamentoId = parcelamento.oFormaPagamento?.Id,
                parcelamento.PrazoDias,
                parcelamento.PorcentagemValor
            });
            return affected > 0;
        }

        public async Task<bool> UpdateAsync(Parcelamentos parcelamento)
        {
            const string sql = @"
                UPDATE Parcelamentos SET
                    FormaPagamentoId = @FormaPagamentoId,
                    PrazoDias = @PrazoDias,
                    PorcentagemValor = @PorcentagemValor
                WHERE NumeroParcela = @NumeroParcela AND CondicaoPagamentoId = @CondicaoPagamentoId";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new
            {
                FormaPagamentoId = parcelamento.oFormaPagamento?.Id,
                parcelamento.PrazoDias,
                parcelamento.PorcentagemValor,
                parcelamento.NumeroParcela,
                CondicaoPagamentoId = parcelamento.oCondicaoPagamento?.Id
            });
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int numeroParcela, int condicaoPagamentoId)
        {
            const string sql = "DELETE FROM Parcelamentos WHERE NumeroParcela = @NumeroParcela AND CondicaoPagamentoId = @CondicaoPagamentoId";
            using var connection = _factory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { NumeroParcela = numeroParcela, CondicaoPagamentoId = condicaoPagamentoId });
            return rowsAffected > 0;
        }
    }
}