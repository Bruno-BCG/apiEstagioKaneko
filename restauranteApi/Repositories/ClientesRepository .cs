// File: ClientesRepository.cs
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace restauranteApi.Repositories
{
    public class ClientesRepository : IClientesRepository
    {
        private readonly SqlConnectionFactory _factory;
        public ClientesRepository(SqlConnectionFactory factory) => _factory = factory;

        private const string BaseJoin = @"
LEFT JOIN Cidades  c  ON c.Id  = cl.CidadeId
LEFT JOIN Estados  e  ON e.Id  = c.EstadoId
LEFT JOIN Paises   p  ON p.Id  = e.PaisId
LEFT JOIN CondicoesPagamento cp ON cp.Id = cl.IdCondicaoPagamento";

        private const string SelectCliente = @"
SELECT
  -- Clientes
  cl.Id               AS id,
  cl.Cliente          AS cliente,
  cl.Apelido          AS apelido,
  cl.Genero           AS genero,
  cl.CpfCnpj          AS cpfCnpj,
  cl.RG               AS rg,
  cl.DataNascimento   AS dataNascimento,
  cl.CidadeId         AS cidadeId,
  cl.IdCondicaoPagamento AS condicoesPagamentoId,
  cl.Telefone         AS telefone,
  cl.Endereco         AS endereco,
  cl.NumeroEndereco   AS numeroEndereco,
  cl.Bairro           AS bairro,
  cl.Complemento      AS complemento,
  cl.CEP              AS cep,
  cl.Anotacao         AS anotacao,
  cl.PratoPreferencial AS pratoPreferencial,
  cl.Ativo            AS ativo,
  cl.DataCadastro     AS dataCadastro,
  cl.DataAlteracao    AS dataAlteracao,

  -- Cidades
  c.Id                AS id,
  c.Cidade            AS cidade,
  c.EstadoId          AS estadoId,
  c.Ativo             AS ativo,
  c.DataCadastro      AS dataCadastro,
  c.DataAlteracao     AS dataAlteracao,

  -- marcador + Estados
  e.Id                AS estadoIdSplit,
  e.Id                AS id,
  e.Estado            AS estado,
  e.UF                AS uf,
  e.PaisId            AS paisId,
  e.Ativo             AS ativo,
  e.DataCadastro      AS dataCadastro,
  e.DataAlteracao     AS dataAlteracao,

  -- marcador + Paises
  p.Id                AS paisIdSplit,
  p.Id                AS id,
  p.Pais              AS pais,
  p.Ativo             AS ativo,
  p.DataCadastro      AS dataCadastro,
  p.DataAlteracao     AS dataAlteracao,

  -- marcador + CondicoesPagamento
  cp.Id               AS condicaoIdSplit,
  cp.Id               AS id,
  cp.Descricao        AS descricao,
  cp.QuantidadeParcelas AS quantidadeParcelas,
  cp.Juros            AS juros,
  cp.Multa            AS multa,
  cp.Desconto         AS desconto,
  cp.Ativo            AS ativo,
  cp.DataCadastro     AS dataCadastro,
  cp.DataAlteracao    AS dataAlteracao
FROM Clientes cl
";

        public async Task<IEnumerable<Clientes>> GetAllAsync()
        {
            var sql = $@"{SelectCliente} {BaseJoin};";
            using var conn = _factory.CreateConnection();

            var list = await conn.QueryAsync<Clientes, Cidades, Estados, Paises, CondicoesPagamento, Clientes>(
                sql,
                (cl, c, e, p, cond) => { e.pais = p; c.estado = e; cl.cidade = c; cl.condicaoPagamento = cond; return cl; },
                splitOn: "id,estadoIdSplit,paisIdSplit,condicaoIdSplit"
            );
            return list;
        }

        public async Task<Clientes?> GetByIdAsync(int id)
        {
            var sql = $@"{SelectCliente} {BaseJoin} WHERE cl.Id=@id;";
            using var conn = _factory.CreateConnection();

            var rows = await conn.QueryAsync<Clientes, Cidades, Estados, Paises, CondicoesPagamento, Clientes>(
                sql,
                (cl, c, e, p, cond) => { e.pais = p; c.estado = e; cl.cidade = c; cl.condicaoPagamento = cond; return cl; },
                new { id },
                splitOn: "id,estadoIdSplit,paisIdSplit,condicaoIdSplit"
            );
            return rows.FirstOrDefault();
        }

        public async Task<int> CreateAsync(Clientes cli)
        {
            const string sql = @"
INSERT INTO Clientes
(Cliente, Apelido, Genero, CpfCnpj, RG, DataNascimento, CidadeId, IdCondicaoPagamento, Telefone, Endereco, NumeroEndereco, Bairro, Complemento, CEP, Anotacao, PratoPreferencial, Ativo, DataCadastro, DataAlteracao)
VALUES
(@cliente, @apelido, @genero, @cpfCnpj, @rg, @dataNascimento, @cidadeId, @condicoesPagamentoId, @telefone, @endereco, @numeroEndereco, @bairro, @complemento, @cep, @anotacao, @pratoPreferencial, @ativo, GETDATE(), NULL);
SELECT CAST(SCOPE_IDENTITY() AS INT);";
            using var conn = _factory.CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql, cli);
        }

        public async Task<Clientes?> UpdateAsync(Clientes cli)
        {
            const string sql = @"
UPDATE Clientes SET
  Cliente=@cliente, Apelido=@apelido, Genero=@genero, CpfCnpj=@cpfCnpj, RG=@rg, DataNascimento=@dataNascimento,
  CidadeId=@cidadeId, IdCondicaoPagamento=@condicoesPagamentoId, Telefone=@telefone, Endereco=@endereco, NumeroEndereco=@numeroEndereco,
  Bairro=@bairro, Complemento=@complemento, CEP=@cep, Anotacao=@anotacao, PratoPreferencial=@pratoPreferencial,
  Ativo=@ativo, DataAlteracao=GETDATE()
WHERE Id=@id;";
            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, cli);
            if (rows == 0) return null;
            return await GetByIdAsync(cli.id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Clientes WHERE Id=@id;";
            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, new { id });
            return rows > 0;
        }
    }
}