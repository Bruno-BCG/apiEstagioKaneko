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

        public FuncionariosRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Funcionarios>> GetAllAsync()
        {
            const string sql = "SELECT f.*, c.Id AS CId, c.Nome AS CNome, c.Ativo AS CAtivo, c.DataCadastro AS CDataCadastro, c.DataAlteracao AS CDataAlteracao FROM Funcionarios f JOIN Cidades c ON f.CidadeId = c.Id";
            using var connection = _factory.CreateConnection();
            return await connection.QueryAsync<Funcionarios, Cidades, Funcionarios>(sql, (funcionario, cidade) =>
            {
                funcionario.oCidade = cidade;
                return funcionario;
            }, splitOn: "CId");
        }

        public async Task<Funcionarios> GetByIdAsync(int id)
        {
            const string sql = "SELECT f.*, c.Id AS CId, c.Nome AS CNome, c.Ativo AS CAtivo, c.DataCadastro AS CDataCadastro, c.DataAlteracao AS CDataAlteracao FROM Funcionarios f JOIN Cidades c ON f.CidadeId = c.Id WHERE f.Id = @Id";
            using var connection = _factory.CreateConnection();
            var result = await connection.QueryAsync<Funcionarios, Cidades, Funcionarios>(sql, (funcionario, cidade) =>
            {
                funcionario.oCidade = cidade;
                return funcionario;
            }, new { Id = id }, splitOn: "PId");

            return result.SingleOrDefault();
        }

        public async Task<int> CreateAsync(Funcionarios funcionario)
        {
            const string sql = @"
                INSERT INTO Funcionarios
                    (Foto, Nome, Apelido, Genero, CidadeId, Endereco, Numero, Bairro, CEP, Complemento, CPF, RG, DataNascimento, Telefone, Email, Matricula, Cargo, Salario, Turno, CargaHoraria, DataAdmissao, DataDemissao, PorcentagemComissao, EhAdministrador, Ativo, DataCadastro, DataAlteracao)
                VALUES
                    (@Foto, @Nome, @Apelido, @Genero, @CidadeId, @Endereco, @Numero, @Bairro, @CEP, @Complemento, @CPF, @RG, @DataNascimento, @Telefone, @Email, @Matricula, @Cargo, @Salario, @Turno, @CargaHoraria, @DataAdmissao, @DataDemissao, @PorcentagemComissao, @EhAdministrador, @Ativo, GETDATE(), NULL);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = _factory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                funcionario.Foto,
                funcionario.Nome,
                funcionario.Apelido,
                funcionario.Genero,
                CidadeId = funcionario.oCidade?.Id,
                funcionario.Endereco,
                funcionario.Numero,
                funcionario.Bairro,
                funcionario.CEP,
                funcionario.Complemento,
                funcionario.CPF,
                funcionario.RG,
                funcionario.DataNascimento,
                funcionario.Telefone,
                funcionario.Email,
                funcionario.Matricula,
                funcionario.Cargo,
                funcionario.Salario,
                funcionario.Turno,
                funcionario.CargaHoraria,
                funcionario.DataAdmissao,
                funcionario.DataDemissao,
                funcionario.PorcentagemComissao,
                funcionario.EhAdministrador,
                funcionario.Ativo
            });
        }

        public async Task<bool> UpdateAsync(Funcionarios funcionario)
        {
            const string sql = @"
                UPDATE Funcionarios SET
                    Foto = @Foto, Nome = @Nome, Apelido = @Apelido, Genero = @Genero, CidadeId = @CidadeId,
                    Endereco = @Endereco, Numero = @Numero, Bairro = @Bairro, CEP = @CEP, Complemento = @Complemento,
                    CPF = @CPF, RG = @RG, DataNascimento = @DataNascimento, Telefone = @Telefone, Email = @Email,
                    Matricula = @Matricula, Cargo = @Cargo, Salario = @Salario, Turno = @Turno, CargaHoraria = @CargaHoraria,
                    DataAdmissao = @DataAdmissao, DataDemissao = @DataDemissao, PorcentagemComissao = @PorcentagemComissao,
                    EhAdministrador = @EhAdministrador, Ativo = @Ativo, DataAlteracao = GETDATE()
                WHERE Id = @Id";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new
            {
                funcionario.Foto,
                funcionario.Nome,
                funcionario.Apelido,
                funcionario.Genero,
                CidadeId = funcionario.oCidade?.Id,
                funcionario.Endereco,
                funcionario.Numero,
                funcionario.Bairro,
                funcionario.CEP,
                funcionario.Complemento,
                funcionario.CPF,
                funcionario.RG,
                funcionario.DataNascimento,
                funcionario.Telefone,
                funcionario.Email,
                funcionario.Matricula,
                funcionario.Cargo,
                funcionario.Salario,
                funcionario.Turno,
                funcionario.CargaHoraria,
                funcionario.DataAdmissao,
                funcionario.DataDemissao,
                funcionario.PorcentagemComissao,
                funcionario.EhAdministrador,
                funcionario.Ativo,
                funcionario.Id
            });
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Funcionarios WHERE Id = @Id";
            using var connection = _factory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}