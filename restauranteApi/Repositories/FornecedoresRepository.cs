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

        public FornecedoresRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Fornecedores>> GetAllAsync()
        {
            const string sql = "SELECT f.*, c.Id AS CId, c.Nome AS CNome, c.Ativo AS CAtivo, c.DataCadastro AS CDataCadastro, c.DataAlteracao AS CDataAlteracao FROM Fornecedores f LEFT JOIN Cidades c ON f.CidadeId = c.Id";
            using var connection = _factory.CreateConnection();
            return await connection.QueryAsync<Fornecedores, Cidades, Fornecedores>(sql, (fornecedor, cidade) => {
                fornecedor.oCidade = cidade;
                return fornecedor;
            }, splitOn: "CId");
        }

        public async Task<Fornecedores> GetByIdAsync(int id)
        {
            const string sql = "SELECT f.*, c.Id AS CId, c.Nome AS CNome, c.Ativo AS CAtivo, c.DataCadastro AS CDataCadastro, c.DataAlteracao AS CDataAlteracao FROM Fornecedores f LEFT JOIN Cidades c ON f.CidadeId = c.Id WHERE f.Id = @Id";
            using var connection = _factory.CreateConnection();
            var result = await connection.QueryAsync<Fornecedores, Cidades, Fornecedores>(sql, (fornecedor, cidade) => {
                fornecedor.oCidade = cidade;
                return fornecedor;
            }, new { Id = id }, splitOn: "PId");

            return result.SingleOrDefault();
        }

        public async Task<int> CreateAsync(Fornecedores fornecedor)
        {
            const string sql = @"
                INSERT INTO Fornecedores
                    (Nome, CpfCnpj, InscEstadualRg, DataFundacaoNascimento, CidadeId, Endereco, Numero, Bairro, CEP, Complemento, Telefone, Ativo, DataCadastro, DataAlteracao)
                VALUES
                    (@Nome, @CpfCnpj, @InscEstadualRg, @DataFundacaoNascimento, @CidadeId, @Endereco, @Numero, @Bairro, @CEP, @Complemento, @Telefone, @Ativo, GETDATE(), NULL);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = _factory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                fornecedor.Nome,
                fornecedor.CpfCnpj,
                fornecedor.InscEstadualRg,
                fornecedor.DataFundacaoNascimento,
                CidadeId = fornecedor.oCidade?.Id,
                fornecedor.Endereco,
                fornecedor.Numero,
                fornecedor.Bairro,
                fornecedor.CEP,
                fornecedor.Complemento,
                fornecedor.Telefone,
                fornecedor.Ativo
            });
        }

        public async Task<bool> UpdateAsync(Fornecedores fornecedor)
        {
            const string sql = @"
                UPDATE Fornecedores SET
                    Nome = @Nome, CpfCnpj = @CpfCnpj, InscEstadualRg = @InscEstadualRg,
                    DataFundacaoNascimento = @DataFundacaoNascimento, CidadeId = @CidadeId,
                    Endereco = @Endereco, Numero = @Numero, Bairro = @Bairro, CEP = @CEP,
                    Complemento = @Complemento, Telefone = @Telefone, Ativo = @Ativo,
                    DataAlteracao = GETDATE()
                WHERE Id = @Id";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new
            {
                fornecedor.Nome,
                fornecedor.CpfCnpj,
                fornecedor.InscEstadualRg,
                fornecedor.DataFundacaoNascimento,
                CidadeId = fornecedor.oCidade?.Id,
                fornecedor.Endereco,
                fornecedor.Numero,
                fornecedor.Bairro,
                fornecedor.CEP,
                fornecedor.Complemento,
                fornecedor.Telefone,
                fornecedor.Ativo,
                fornecedor.Id
            });
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Fornecedores WHERE Id = @Id";
            using var connection = _factory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}