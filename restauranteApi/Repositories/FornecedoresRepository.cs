// File: FornecedoresRepository.cs
using Dapper;
using Microsoft.Data.SqlClient;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace restauranteApi.Repositories
{
    public class FornecedoresRepository : IFornecedoresRepository
    {
        private readonly SqlConnectionFactory _factory;
        public FornecedoresRepository(SqlConnectionFactory factory) => _factory = factory;

        private const string BaseJoin = @"
        LEFT JOIN Cidades  c ON c.Id = f.CidadeId
        LEFT JOIN Estados  e ON e.Id = c.EstadoId
        LEFT JOIN Paises   p ON p.Id = e.PaisId
        LEFT JOIN CondicoesPagamento cp ON cp.Id = f.IdCondicaoPagamento";

        private const string SelectFornecedor = @"
        SELECT
            -- Fornecedores
            f.Id                     AS id,
            f.Fornecedor             AS fornecedor,
            f.CpfCnpj                AS cpfCnpj,
            f.InscEstadualRg         AS inscEstadualRg,
            f.DataFundacaoNascimento AS dataFundacaoNascimento,
            f.CidadeId               AS cidadeId,
            f.IdCondicaoPagamento    AS condicoesPagamentoId,
            f.Endereco               AS endereco,
            f.Numero                 AS numero,
            f.Bairro                 AS bairro,
            f.CEP                    AS cep,
            f.Complemento            AS complemento,
            f.Telefone               AS telefone,
            f.Ativo                  AS ativo,
            f.DataCadastro           AS dataCadastro,
            f.DataAlteracao          AS dataAlteracao,

            -- Cidades
            c.Id                     AS id,
            c.Cidade                 AS cidade,
            c.EstadoId               AS estadoId,
            c.Ativo                  AS ativo,
            c.DataCadastro           AS dataCadastro,
            c.DataAlteracao          AS dataAlteracao,

            -- marcador + Estados
            e.Id                     AS estadoIdSplit,
            e.Id                     AS id,
            e.Estado                 AS estado,
            e.UF                     AS uf,
            e.PaisId                 AS paisId,
            e.Ativo                  AS ativo,
            e.DataCadastro           AS dataCadastro,
            e.DataAlteracao          AS dataAlteracao,

            -- marcador + Paises
            p.Id                     AS paisIdSplit,
            p.Id                     AS id,
            p.Pais                   AS pais,
            p.Ativo                  AS ativo,
            p.DataCadastro           AS dataCadastro,
            p.DataAlteracao          AS dataAlteracao,

            -- marcador + CondicoesPagamento
            cp.Id                    AS condicaoIdSplit,
            cp.Id                    AS id,
            cp.Descricao             AS descricao,
            cp.QuantidadeParcelas    AS quantidadeParcelas,
            cp.Juros                 AS juros,
            cp.Multa                 AS multa,
            cp.Desconto              AS desconto,
            cp.Ativo                 AS ativo,
            cp.DataCadastro          AS dataCadastro,
            cp.DataAlteracao         AS dataAlteracao
        FROM Fornecedores f
        ";

        public async Task<IEnumerable<Fornecedores>> GetAllAsync()
        {
            var sql = $@"{SelectFornecedor} {BaseJoin};";
            using var conn = _factory.CreateConnection();

            var list = await conn.QueryAsync<Fornecedores, Cidades, Estados, Paises, CondicoesPagamento, Fornecedores>(
                sql,
                (f, c, e, p, cond) => { e.pais = p; c.estado = e; f.cidade = c; f.condicaoPagamento = cond; return f; },
                splitOn: "id,estadoIdSplit,paisIdSplit,condicaoIdSplit"
            );

            return list;
        }

        public async Task<Fornecedores?> GetByIdAsync(int id)
        {
            var sql = $@"{SelectFornecedor} {BaseJoin} WHERE f.Id=@id;";
            using var conn = _factory.CreateConnection();

            var rows = await conn.QueryAsync<Fornecedores, Cidades, Estados, Paises, CondicoesPagamento, Fornecedores>(
                sql,
                (f, c, e, p, cond) => { e.pais = p; c.estado = e; f.cidade = c; f.condicaoPagamento = cond; return f; },
                new { id },
                splitOn: "id,estadoIdSplit,paisIdSplit,condicaoIdSplit"
            );

            return rows.FirstOrDefault();
        }

        public async Task<int> CreateAsync(Fornecedores forn)
        {
            const string sql = @"
            INSERT INTO Fornecedores
            (Fornecedor, CpfCnpj, InscEstadualRg, DataFundacaoNascimento, CidadeId, IdCondicaoPagamento, Endereco, Numero, Bairro, CEP, Complemento, Telefone, Ativo, DataCadastro, DataAlteracao)
            VALUES
            (@fornecedor, @cpfCnpj, @inscEstadualRg, @dataFundacaoNascimento, @cidadeId, @condicoesPagamentoId, @endereco, @numero, @bairro, @cep, @complemento, @telefone, @ativo, GETDATE(), NULL);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";
            using var conn = _factory.CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql, forn);
        }

        public async Task<Fornecedores?> UpdateAsync(Fornecedores forn)
        {
            const string sql = @"
            UPDATE Fornecedores SET
                Fornecedor=@fornecedor, CpfCnpj=@cpfCnpj, InscEstadualRg=@inscEstadualRg, DataFundacaoNascimento=@dataFundacaoNascimento,
                CidadeId=@cidadeId, IdCondicaoPagamento=@condicoesPagamentoId, Endereco=@endereco, Numero=@numero, Bairro=@bairro, CEP=@cep,
                Complemento=@complemento, Telefone=@telefone, Ativo=@ativo, DataAlteracao=GETDATE()
            WHERE Id=@id;";
            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, forn);
            if (rows == 0) return null;
            return await GetByIdAsync(forn.id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Fornecedores WHERE Id=@id;";
            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, new { id });
            return rows > 0;
        }
    }
}
