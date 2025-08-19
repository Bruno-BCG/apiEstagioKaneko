using Dapper;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;

namespace restauranteApi.Repositories
{
    public class TransportadorasRepository : ITransportadorasRepository
    {
        private readonly SqlConnectionFactory _factory;
        public TransportadorasRepository(SqlConnectionFactory factory) => _factory = factory;

        private const string BaseJoin = @"
        LEFT JOIN Cidades  c  ON c.Id  = t.CidadeId
        LEFT JOIN Estados  e  ON e.Id  = c.EstadoId
        LEFT JOIN Paises   p  ON p.Id  = e.PaisId
        LEFT JOIN CondicoesPagamento cp ON cp.Id = t.IdCondicaoPagamento";

        private const string SelectT = @"
        SELECT
          -- Transportadoras
          t.Id                  AS id,
          t.Transportadora      AS transportadora,
          t.CpfCnpj             AS cpfCnpj,
          t.InscricaoEstadual   AS inscricaoEstadual,
          t.InscricaoEstadualSubtrib AS inscricaoEstadualSubtrib,
          t.CidadeId            AS cidadeId,
          t.IdCondicaoPagamento AS condicoesPagamentoId,
          t.Endereco            AS endereco,
          t.NumeroEndereco      AS numeroEndereco,
          t.Bairro              AS bairro,
          t.Complemento         AS complemento,
          t.CEP                 AS cep,
          t.Telefone            AS telefone,
          t.Email               AS email,
          t.Tipo                AS tipo,
          t.Ativo               AS ativo,
          t.DataCadastro        AS dataCadastro,
          t.DataAlteracao       AS dataAlteracao,

          -- Cidades
          c.Id                  AS id,
          c.Cidade              AS cidade,
          c.EstadoId            AS estadoId,
          c.Ativo               AS ativo,
          c.DataCadastro        AS dataCadastro,
          c.DataAlteracao       AS dataAlteracao,

          -- marcador + Estados
          e.Id                  AS estadoIdSplit,
          e.Id                  AS id,
          e.Estado              AS estado,
          e.UF                  AS uf,
          e.PaisId              AS paisId,
          e.Ativo               AS ativo,
          e.DataCadastro        AS dataCadastro,
          e.DataAlteracao       AS dataAlteracao,

          -- marcador + Paises
          p.Id                  AS paisIdSplit,
          p.Id                  AS id,
          p.Pais                AS pais,
          p.Ativo               AS ativo,
          p.DataCadastro        AS dataCadastro,
          p.DataAlteracao       AS dataAlteracao,

          -- marcador + CondicoesPagamento
          cp.Id                 AS condicaoIdSplit,
          cp.Id                 AS id,
          cp.Descricao          AS descricao,
          cp.QuantidadeParcelas AS quantidadeParcelas,
          cp.Juros              AS juros,
          cp.Multa              AS multa,
          cp.Desconto           AS desconto,
          cp.Ativo              AS ativo,
          cp.DataCadastro       AS dataCadastro,
          cp.DataAlteracao      AS dataAlteracao
        FROM Transportadoras t
        ";

        public async Task<IEnumerable<Transportadoras>> GetAllAsync()
        {
            var sql = $@"{SelectT} {BaseJoin};";
            using var conn = _factory.CreateConnection();

            var list = await conn.QueryAsync<Transportadoras, Cidades, Estados, Paises, CondicoesPagamento, Transportadoras>(
                sql,
                (t, c, e, p, cond) => { e.pais = p; c.estado = e; t.cidade = c; t.condicaoPagamento = cond; return t; },
                splitOn: "id,estadoIdSplit,paisIdSplit,condicaoIdSplit"
            );
            return list;
        }

        public async Task<Transportadoras?> GetByIdAsync(int id)
        {
            var sql = $@"{SelectT} {BaseJoin} WHERE t.Id = @id;";
            using var conn = _factory.CreateConnection();

            var rows = await conn.QueryAsync<Transportadoras, Cidades, Estados, Paises, CondicoesPagamento, Transportadoras>(
                sql,
                (t, c, e, p, cond) => { e.pais = p; c.estado = e; t.cidade = c; t.condicaoPagamento = cond; return t; },
                new { id },
                splitOn: "id,estadoIdSplit,paisIdSplit,condicaoIdSplit"
            );
            return rows.FirstOrDefault();
        }

        public async Task<int> CreateAsync(Transportadoras t)
        {
            const string sql = @"
            INSERT INTO Transportadoras
            (Transportadora, CpfCnpj, InscricaoEstadual, InscricaoEstadualSubtrib, CidadeId, IdCondicaoPagamento, Endereco, NumeroEndereco, Bairro, Complemento, CEP, Telefone, Email, Tipo, Ativo, DataCadastro, DataAlteracao)
            VALUES
            (@transportadora, @cpfCnpj, @inscricaoEstadual, @inscricaoEstadualSubtrib, @cidadeId, @condicoesPagamentoId, @endereco, @numeroEndereco, @bairro, @complemento, @cep, @telefone, @email, @tipo, @ativo, GETDATE(), NULL);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";
            using var conn = _factory.CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql, t);
        }

        public async Task<Transportadoras?> UpdateAsync(Transportadoras t)
        {
            const string sql = @"
            UPDATE Transportadoras SET
              Transportadora=@transportadora, CpfCnpj=@cpfCnpj, InscricaoEstadual=@inscricaoEstadual, InscricaoEstadualSubtrib=@inscricaoEstadualSubtrib,
              CidadeId=@cidadeId, IdCondicaoPagamento=@condicoesPagamentoId, Endereco=@endereco, NumeroEndereco=@numeroEndereco, Bairro=@bairro,
              Complemento=@complemento, CEP=@cep, Telefone=@telefone, Email=@email, Tipo=@tipo, Ativo=@ativo, DataAlteracao=GETDATE()
            WHERE Id=@id;";
            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, t);
            if (rows == 0) return null;
            return await GetByIdAsync(t.id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Transportadoras WHERE Id=@id;";
            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, new { id });
            return rows > 0;
        }
    }
}
