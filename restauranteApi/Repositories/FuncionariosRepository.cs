// File: FuncionariosRepository.cs
using Dapper;
using Microsoft.Data.SqlClient;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace restauranteApi.Repositories
{
    public class FuncionariosRepository : IFuncionariosRepository
    {
        private readonly SqlConnectionFactory _factory;
        public FuncionariosRepository(SqlConnectionFactory factory) => _factory = factory;

        // Cidades -> Estados -> Paises
        private const string BaseJoin = @"
        LEFT JOIN Cidades  c ON c.Id = f.CidadeId
        LEFT JOIN Estados  e ON e.Id = c.EstadoId
        LEFT JOIN Paises   p ON p.Id = e.PaisId";

        private const string SelectFuncionario = @"
        SELECT
          -- Funcionarios
          f.Id              AS id,
          f.Foto            AS foto,
          f.Funcionario     AS funcionario,
          f.Apelido         AS apelido,
          f.Genero          AS genero,
          f.CidadeId        AS cidadeId,
          f.Endereco        AS endereco,
          f.Numero          AS numero,
          f.Bairro          AS bairro,
          f.CEP             AS cep,
          f.Complemento     AS complemento,
          f.CpfCnpj         AS cpfCnpj,
          f.RG              AS rg,
          f.DataNascimento  AS dataNascimento,
          f.Telefone        AS telefone,
          f.Email           AS email,
          f.Matricula       AS matricula,
          f.Cargo           AS cargo,
          f.Salario         AS salario,
          f.Turno           AS turno,
          f.CargaHoraria    AS cargaHoraria,
          f.DataAdmissao    AS dataAdmissao,
          f.DataDemissao    AS dataDemissao,
          f.PorcentagemComissao AS porcentagemComissao,
          f.EhAdministrador AS ehAdministrador,
          f.Ativo           AS ativo,
          f.DataCadastro    AS dataCadastro,
          f.DataAlteracao   AS dataAlteracao,

          -- Cidades
          c.Id              AS id,
          c.Cidade          AS cidade,
          c.EstadoId        AS estadoId,
          c.Ativo           AS ativo,
          c.DataCadastro    AS dataCadastro,
          c.DataAlteracao   AS dataAlteracao,

          -- marcador p/ split e Estados
          e.Id              AS estadoIdSplit,
          e.Id              AS id,
          e.Estado          AS estado,
          e.UF              AS uf,
          e.PaisId          AS paisId,
          e.Ativo           AS ativo,
          e.DataCadastro    AS dataCadastro,
          e.DataAlteracao   AS dataAlteracao,

          -- marcador p/ split e Paises
          p.Id              AS paisIdSplit,
          p.Id              AS id,
          p.Pais            AS pais,
          p.Ativo           AS ativo,
          p.DataCadastro    AS dataCadastro,
          p.DataAlteracao   AS dataAlteracao
        FROM Funcionarios f
        ";

        public async Task<IEnumerable<Funcionarios>> GetAllAsync()
        {
            var sql = $@"{SelectFuncionario} {BaseJoin};";
            using var conn = _factory.CreateConnection();

            var list = await conn.QueryAsync<Funcionarios, Cidades, Estados, Paises, Funcionarios>(
                sql,
                (f, c, e, p) => { e.pais = p; c.estado = e; f.cidade = c; return f; },
                splitOn: "id,estadoIdSplit,paisIdSplit"
            );
            return list;
        }

        public async Task<Funcionarios?> GetByIdAsync(int id)
        {
            var sql = $@"{SelectFuncionario} {BaseJoin} WHERE f.Id = @id;";
            using var conn = _factory.CreateConnection();

            var rows = await conn.QueryAsync<Funcionarios, Cidades, Estados, Paises, Funcionarios>(
                sql,
                (f, c, e, p) => { e.pais = p; c.estado = e; f.cidade = c; return f; },
                new { id },
                splitOn: "id,estadoIdSplit,paisIdSplit"
            );
            return rows.FirstOrDefault();
        }

        public async Task<int> CreateAsync(Funcionarios func)
        {
            const string sql = @"
            INSERT INTO Funcionarios
            (Foto, Funcionario, Apelido, Genero, CidadeId, Endereco, Numero, Bairro, CEP, Complemento, CpfCnpj, RG, DataNascimento, Telefone, Email, Matricula, Cargo, Salario, Turno, CargaHoraria, DataAdmissao, DataDemissao, PorcentagemComissao, EhAdministrador, Ativo, DataCadastro, DataAlteracao)
            VALUES
            (@foto, @funcionario, @apelido, @genero, @cidadeId, @endereco, @numero, @bairro, @cep, @complemento, @cpfCnpj, @rg, @dataNascimento, @telefone, @email, @matricula, @cargo, @salario, @turno, @cargaHoraria, @dataAdmissao, @dataDemissao, @porcentagemComissao, @ehAdministrador, @ativo, GETDATE(), NULL);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";
            using var conn = _factory.CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql, func);
        }

        public async Task<Funcionarios?> UpdateAsync(Funcionarios func)
        {
            const string sql = @"
            UPDATE Funcionarios SET
              Foto=@foto, Funcionario=@funcionario, Apelido=@apelido, Genero=@genero, CidadeId=@cidadeId,
              Endereco=@endereco, Numero=@numero, Bairro=@bairro, CEP=@cep, Complemento=@complemento,
              CpfCnpj=@cpfCnpj, RG=@rg, DataNascimento=@dataNascimento, Telefone=@telefone, Email=@email,
              Matricula=@matricula, Cargo=@cargo, Salario=@salario, Turno=@turno, CargaHoraria=@cargaHoraria,
              DataAdmissao=@dataAdmissao, DataDemissao=@dataDemissao, PorcentagemComissao=@porcentagemComissao,
              EhAdministrador=@ehAdministrador, Ativo=@ativo, DataAlteracao=GETDATE()
            WHERE Id=@id;";
            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, func);
            if (rows == 0) return null;
            return await GetByIdAsync(func.id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Funcionarios WHERE Id=@id;";
            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, new { id });
            return rows > 0;
        }
    }
}