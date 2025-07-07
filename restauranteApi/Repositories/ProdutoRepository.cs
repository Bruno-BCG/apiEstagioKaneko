// File: ProdutoRepository.cs
using Dapper;
using Microsoft.Data.SqlClient;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace restauranteApi.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly SqlConnectionFactory _factory;

        public ProdutoRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Produtos>> GetAllAsync()
        {
            const string sql = "SELECT p.*, g.Id AS GId, g.Nome AS GNome, g.Descricao AS GDescricao, g.IpImpressora AS GIpImpressora, g.Ativo AS GAtivo, g.DataCadastro AS GDataCadastro, g.DataAlteracao AS GDataAlteracao FROM Produtos p JOIN Grupo g ON p.GrupoId = g.Id";
            using var connection = _factory.CreateConnection();
            return await connection.QueryAsync<Produtos, Grupo, Produtos>(sql, (produto, grupo) =>
            {
                produto.oGrupo = grupo;
                return produto;
            }, splitOn: "GId");
        }

        public async Task<Produtos> GetByIdAsync(int id)
        {
            const string sql = "SELECT p.*, g.Id AS GId, g.Nome AS GNome, g.Descricao AS GDescricao, g.IpImpressora AS GIpImpressora, g.Ativo AS GAtivo, g.DataCadastro AS GDataCadastro, g.DataAlteracao AS GDataAlteracao FROM Produtos p JOIN Grupo g ON p.GrupoId = g.Id WHERE p.Id = @Id";
            using var connection = _factory.CreateConnection();
            var result = await connection.QueryAsync<Produtos, Grupo, Produtos>(sql, (produto, grupo) =>
            {
                produto.oGrupo = grupo;
                return produto;
            }, new { Id = id }, splitOn: "GId");

            return result.SingleOrDefault();
        }

        public async Task<int> CreateAsync(Produtos produto)
        {
            const string sql = @"
                INSERT INTO Produtos
                    (Nome, Imagem, Preco, Descricao, Estoque, TempoPreparo, Ingredientes, Ativo, DataCadastro, DataAlteracao, GrupoId)
                VALUES
                    (@Nome, @Imagem, @Preco, @Descricao, @Estoque, @TempoPreparo, @Ingredientes, @Ativo, GETDATE(), NULL, @GrupoId);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = _factory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                produto.Nome,
                produto.Imagem,
                produto.Preco,
                produto.Descricao,
                produto.Estoque,
                produto.TempoPreparo,
                produto.Ingredientes,
                produto.Ativo,
                GrupoId = produto.oGrupo?.Id // Corrected to use navigation property
            });
        }

        public async Task<bool> UpdateAsync(Produtos produto)
        {
            const string sql = @"
                UPDATE Produtos SET
                    Nome = @Nome,
                    Imagem = @Imagem,
                    Preco = @Preco,
                    Descricao = @Descricao,
                    Estoque = @Estoque,
                    TempoPreparo = @TempoPreparo,
                    Ingredientes = @Ingredientes,
                    Ativo = @Ativo,
                    DataAlteracao = GETDATE(),
                    GrupoId = @GrupoId
                WHERE Id = @Id";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new
            {
                produto.Nome,
                produto.Imagem,
                produto.Preco,
                produto.Descricao,
                produto.Estoque,
                produto.TempoPreparo,
                produto.Ingredientes,
                produto.Ativo,
                GrupoId = produto.oGrupo?.Id, // Corrected to use navigation property
                produto.Id
            });
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Produtos WHERE Id = @Id";
            using var connection = _factory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}