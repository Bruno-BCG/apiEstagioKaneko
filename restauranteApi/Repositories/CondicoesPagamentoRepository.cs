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
            // Uma query para condições + uma para suas parcelas
            const string condSql = @"
            SELECT
                Id                 AS id,
                Descricao          AS descricao,
                QuantidadeParcelas AS quantidadeParcelas,
                Juros              AS juros,
                Multa              AS multa,
                Desconto           AS desconto,
                Ativo              AS ativo,
                DataCadastro       AS dataCadastro,
                DataAlteracao      AS dataAlteracao
            FROM CondicoesPagamento;";

                        const string parSql = @"
            SELECT
                p.NumeroParcela       AS numeroParcela,
                p.CondicaoPagamentoId AS condicoesPagamentoId,
                p.PrazoDias           AS prazoDias,
                p.PorcentagemValor    AS porcentagemValor,

                fp.Id                 AS id,
                fp.Descricao          AS descricao,
                fp.Ativo              AS ativo,
                fp.DataCadastro       AS dataCadastro,
                fp.DataAlteracao      AS dataAlteracao
            FROM Parcelamentos p
            LEFT JOIN FormasPagamento fp ON fp.Id = p.FormaPagamentoId;";

            using var conn = _factory.CreateConnection();

            var condicoes = (await conn.QueryAsync<CondicoesPagamento>(condSql)).ToList();

            var parcelas = await conn.QueryAsync<Parcelamentos, FormasPagamento, Parcelamentos>(
                parSql,
                (par, fp) =>
                {
                    par.formaPagamento = fp;
                    return par;
                },
                splitOn: "id"
            );

            // agrupar por condicoesPagamentoId
            var dict = condicoes.ToDictionary(c => c.id, c => c);
            foreach (var p in parcelas)
            {
                if (dict.TryGetValue(p.condicoesPagamentoId, out var cond))
                {
                    cond.parcelamentos ??= new List<Parcelamentos>();
                    cond.parcelamentos.Add(p);
                }
            }

            return condicoes;
        }

        public async Task<CondicoesPagamento?> GetByIdAsync(int id)
        {
            const string condSql = @"
            SELECT
              Id                 AS id,
              Descricao          AS descricao,
              QuantidadeParcelas AS quantidadeParcelas,
              Juros              AS juros,
              Multa              AS multa,
              Desconto           AS desconto,
              Ativo              AS ativo,
              DataCadastro       AS dataCadastro,
              DataAlteracao      AS dataAlteracao
            FROM CondicoesPagamento
            WHERE Id = @id;";

                        const string parSql = @"
            SELECT
              p.NumeroParcela       AS numeroParcela,
              p.CondicaoPagamentoId AS condicoesPagamentoId,
              p.PrazoDias           AS prazoDias,
              p.PorcentagemValor    AS porcentagemValor,

              fp.Id                 AS id,
              fp.Descricao          AS descricao,
              fp.Ativo              AS ativo,
              fp.DataCadastro       AS dataCadastro,
              fp.DataAlteracao      AS dataAlteracao
            FROM Parcelamentos p
            LEFT JOIN FormasPagamento fp ON fp.Id = p.FormaPagamentoId
            WHERE p.CondicaoPagamentoId = @id
            ORDER BY p.NumeroParcela;";

            using var conn = _factory.CreateConnection();

            var cond = await conn.QueryFirstOrDefaultAsync<CondicoesPagamento>(condSql, new { id });
            if (cond is null) return null;

            var parcelas = await conn.QueryAsync<Parcelamentos, FormasPagamento, Parcelamentos>(
                parSql,
                (par, fp) =>
                {
                    par.formaPagamento = fp;
                    return par;
                },
                new { id },
                splitOn: "id"
            );

            cond.parcelamentos = parcelas.ToList();
            return cond;
        }

        public async Task<int> CreateAsync(CondicoesPagamento condicao)
        {
            const string sql = @"
            INSERT INTO CondicoesPagamento
            (Descricao, QuantidadeParcelas, Juros, Multa, Desconto, Ativo, DataCadastro, DataAlteracao)
            VALUES (@descricao, @quantidadeParcelas, @juros, @multa, @desconto, @ativo, GETDATE(), NULL);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var conn = _factory.CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql, condicao);
        }

        public async Task<CondicoesPagamento?> UpdateAsync(CondicoesPagamento condicao)
        {
            const string sql = @"
            UPDATE CondicoesPagamento SET
              Descricao          = @descricao,
              QuantidadeParcelas = @quantidadeParcelas,
              Juros              = @juros,
              Multa              = @multa,
              Desconto           = @desconto,
              Ativo              = @ativo,
              DataAlteracao      = GETDATE()
            WHERE Id = @id;";

            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, condicao);
            if (rows == 0) return null;

            return await GetByIdAsync(condicao.id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string delParcelas = @"DELETE FROM Parcelamentos WHERE CondicaoPagamentoId = @id;";
            const string delCond = @"DELETE FROM CondicoesPagamento WHERE Id = @id;";

            using var conn = _factory.CreateConnection();
            await conn.OpenAsync(); // <-- IMPORTANTE

            await using var tx = await conn.BeginTransactionAsync();
            try
            {
                await conn.ExecuteAsync(delParcelas, new { id }, tx);
                var rows = await conn.ExecuteAsync(delCond, new { id }, tx);

                await tx.CommitAsync();
                return rows > 0;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

    }
}