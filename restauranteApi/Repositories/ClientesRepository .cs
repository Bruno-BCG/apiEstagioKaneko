// File: ClientesRepository.cs
using Dapper;
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

        public ClientesRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Clientes>> GetAllAsync()
        {
            const string sql = "SELECT c.*, ci.Id AS CId, ci.Nome AS CNome, ci.Ativo AS CAtivo, ci.DataCadastro AS CDataCadastro, ci.DataAlteracao AS CDataAlteracao FROM Clientes c LEFT JOIN Cidades ci ON c.CidadeId = ci.Id";
            using var connection = _factory.CreateConnection();
            return await connection.QueryAsync<Clientes, Cidades, Clientes>(sql, (cliente, cidade) => {
                cliente.oCidade = cidade;
                return cliente;
            }, splitOn: "CId");
        }

        public async Task<Clientes> GetByIdAsync(int id)
        {
            const string sql = "SELECT c.*, ci.Id AS CId, ci.Nome AS CNome, ci.Ativo AS CAtivo, ci.DataCadastro AS CDataCadastro, ci.DataAlteracao AS CDataAlteracao FROM Clientes c LEFT JOIN Cidades ci ON c.CidadeId = ci.Id WHERE c.Id = @Id";
            using var connection = _factory.CreateConnection();
            var result = await connection.QueryAsync<Clientes, Cidades, Clientes>(sql, (cliente, cidade) => {
                cliente.oCidade = cidade;
                return cliente;
            }, new { Id = id }, splitOn: "CId");

            return result.SingleOrDefault();
        }

        public async Task<int> CreateAsync(Clientes cliente)
        {
            const string sql = @"
                INSERT INTO Clientes
                    (Nome, Apelido, Genero, CPF, RG, DataNascimento, CidadeId, Telefone, Anotacao, PratoPreferencial, Ativo, DataCadastro, DataAlteracao)
                VALUES
                    (@Nome, @Apelido, @Genero, @CPF, @RG, @DataNascimento, @CidadeId, @Telefone, @Anotacao, @PratoPreferencial, @Ativo, GETDATE(), NULL);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = _factory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                cliente.Nome,
                cliente.Apelido,
                cliente.Genero,
                cliente.CPF,
                cliente.RG,
                cliente.DataNascimento,
                CidadeId = cliente.oCidade?.Id,
                cliente.Telefone,
                cliente.Anotacao,
                cliente.PratoPreferencial,
                cliente.Ativo
            });
        }

        public async Task<bool> UpdateAsync(Clientes cliente)
        {
            const string sql = @"
                UPDATE Clientes SET
                    Nome = @Nome,
                    Apelido = @Apelido,
                    Genero = @Genero,
                    CPF = @CPF,
                    RG = @RG,
                    DataNascimento = @DataNascimento,
                    CidadeId = @CidadeId,
                    Telefone = @Telefone,
                    Anotacao = @Anotacao,
                    PratoPreferencial = @PratoPreferencial,
                    Ativo = @Ativo,
                    DataAlteracao = GETDATE()
                WHERE Id = @Id";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new
            {
                cliente.Nome,
                cliente.Apelido,
                cliente.Genero,
                cliente.CPF,
                cliente.RG,
                cliente.DataNascimento,
                CidadeId = cliente.oCidade?.Id,
                cliente.Telefone,
                cliente.Anotacao,
                cliente.PratoPreferencial,
                cliente.Ativo,
                cliente.Id
            });
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Clientes WHERE Id = @Id";
            using var connection = _factory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}