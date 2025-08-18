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

        public async Task<Parcelamentos?> GetByAsync(int condicoesPagamentoId, int numeroParcela)
        {
            const string sql = @"
            SELECT
              -- Parcelamentos
              p.NumeroParcela       AS numeroParcela,
              p.CondicaoPagamentoId AS condicoesPagamentoId,
              p.PrazoDias           AS prazoDias,
              p.PorcentagemValor    AS porcentagemValor,

              -- marcador de split e objeto FormasPagamento
              fp.Id                 AS formaPagamentoId,   -- split marker
              fp.Id                 AS id,
              fp.Descricao          AS descricao,
              fp.Ativo              AS ativo,
              fp.DataCadastro       AS dataCadastro,
              fp.DataAlteracao      AS dataAlteracao
            FROM Parcelamentos p
            LEFT JOIN FormasPagamento fp ON fp.Id = p.FormaPagamentoId
            WHERE p.CondicaoPagamentoId = @condicoesPagamentoId
              AND p.NumeroParcela       = @numeroParcela;";

            using var conn = _factory.CreateConnection();

            var rows = await conn.QueryAsync<Parcelamentos, FormasPagamento, Parcelamentos>(
                sql,
                (p, fp) => { p.formaPagamento = fp; return p; },
                new { condicoesPagamentoId, numeroParcela },
                splitOn: "formaPagamentoId"
            );

            return rows.FirstOrDefault();
        }


        public async Task<IEnumerable<Parcelamentos>> GetByCondicaoIdAsync(int condicoesPagamentoId)
        {
            const string sql = @"
            SELECT
              p.NumeroParcela      AS numeroParcela,
              p.CondicaoPagamentoId AS condicoesPagamentoId,
              p.PrazoDias          AS prazoDias,
              p.PorcentagemValor   AS porcentagemValor,

              fp.Id                AS id,
              fp.Descricao         AS descricao,
              fp.Ativo             AS ativo,
              fp.DataCadastro      AS dataCadastro,
              fp.DataAlteracao     AS dataAlteracao
            FROM Parcelamentos p
            LEFT JOIN FormasPagamento fp ON fp.Id = p.FormaPagamentoId
            WHERE p.CondicaoPagamentoId = @condicoesPagamentoId
            ORDER BY p.NumeroParcela;";

            using var conn = _factory.CreateConnection();

            var list = await conn.QueryAsync<Parcelamentos, FormasPagamento, Parcelamentos>(
                sql,
                (p, fp) => { p.formaPagamento = fp; return p; },
                new { condicoesPagamentoId },
                splitOn: "condicoesPagamentoId"
            );

            return list;
        }

        public async Task<int> CreateAsync(Parcelamentos parcela)
        {
            // precisamos do id da forma de pagamento dentro do objeto
            var formaPagamentoId = parcela.formaPagamento?.id
                ?? throw new ArgumentException("formaPagamento.id é obrigatório.");

            const string sql = @"
            INSERT INTO Parcelamentos (NumeroParcela, CondicaoPagamentoId, FormaPagamentoId, PrazoDias, PorcentagemValor)
            VALUES (@numeroParcela, @condicoesPagamentoId, @formaPagamentoId, @prazoDias, @porcentagemValor);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var conn = _factory.CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql, new
            {
                parcela.numeroParcela,
                parcela.condicoesPagamentoId,
                formaPagamentoId,
                parcela.prazoDias,
                parcela.porcentagemValor
            });
        }

        public async Task<Parcelamentos?> UpdateAsync(Parcelamentos parcela)
        {
            var formaPagamentoId = parcela.formaPagamento?.id
                ?? throw new ArgumentException("formaPagamento.id é obrigatório.");

            const string sql = @"
            UPDATE Parcelamentos SET
                FormaPagamentoId   = @formaPagamentoId,
                PrazoDias          = @prazoDias,
                PorcentagemValor   = @porcentagemValor
            WHERE CondicaoPagamentoId = @condicoesPagamentoId
                AND NumeroParcela       = @numeroParcela;";

            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, new
            {
                parcela.condicoesPagamentoId,
                parcela.numeroParcela,
                formaPagamentoId,
                parcela.prazoDias,
                parcela.porcentagemValor
            });

            if (rows == 0) return null;
            return await GetByAsync(parcela.condicoesPagamentoId, parcela.numeroParcela);
        }


        public async Task<bool> DeleteAsync(int condicoesPagamentoId, int numeroParcela)
        {
            const string sql = @"
            DELETE FROM Parcelamentos
            WHERE CondicaoPagamentoId = @condicoesPagamentoId
              AND NumeroParcela       = @numeroParcela;";

            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, new { condicoesPagamentoId, numeroParcela });
            return rows > 0;
        }

    }
}