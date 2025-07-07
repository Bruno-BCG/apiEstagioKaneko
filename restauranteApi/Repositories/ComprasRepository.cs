using Dapper;
using Microsoft.Data.SqlClient;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace restauranteApi.Repositories
{
    public class ComprasRepository : IComprasRepository
    {
        private readonly SqlConnectionFactory _factory;

        public ComprasRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Compras>> GetAllAsync()
        {
            const string sql = "SELECT c.*, f.Id AS FId, f.Nome AS FNome, cp.Id AS CPId, cp.Descricao AS CPDescricao FROM Compras c LEFT JOIN Fornecedores f ON c.FornecedorId = f.Id LEFT JOIN CondicoesPagamento cp ON c.CondicaoPagamentoId = cp.Id";
            using var connection = _factory.CreateConnection();
            return await connection.QueryAsync<Compras, Fornecedores, CondicoesPagamento, Compras>(
                sql,
                (compra, fornecedor, condicaoPagamento) => {
                    compra.oFornecedor = fornecedor;
                    compra.oCondicaoPagamento = condicaoPagamento;
                    return compra;
                },
                splitOn: "FId,CPId"
            );
        }

        public async Task<Compras> GetByIdAsync(char modelo, char serie, int numero, int fornecedorId)
        {
            const string sql = "SELECT c.*, f.Id AS FId, f.Nome AS FNome, cp.Id AS CPId, cp.Descricao AS CPDescricao FROM Compras c LEFT JOIN Fornecedores f ON c.FornecedorId = f.Id LEFT JOIN CondicoesPagamento cp ON c.CondicaoPagamentoId = cp.Id WHERE c.Modelo = @Modelo AND c.Serie = @Serie AND c.Numero = @Numero AND c.FornecedorId = @FornecedorId";
            using var connection = _factory.CreateConnection();
            var result = await connection.QueryAsync<Compras, Fornecedores, CondicoesPagamento, Compras>(
                sql,
                (compra, fornecedor, condicaoPagamento) => {
                    compra.oFornecedor = fornecedor;
                    compra.oCondicaoPagamento = condicaoPagamento;
                    return compra;
                },
                new { Modelo = modelo, Serie = serie, Numero = numero, FornecedorId = fornecedorId },
                splitOn: "FId,CPId"
            );

            return result.SingleOrDefault();
        }

        public async Task<bool> CreateAsync(Compras compra)
        {
            const string sql = @"
                INSERT INTO Compras
                    (Modelo, Serie, Numero, FornecedorId, CondicaoPagamentoId, DataCompra, ValorTotal, Ativo, DataCadastro, DataAlteracao)
                VALUES
                    (@Modelo, @Serie, @Numero, @FornecedorId, @CondicaoPagamentoId, GETDATE(), @ValorTotal, @Ativo, GETDATE(), NULL);";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new
            {
                compra.Modelo,
                compra.Serie,
                compra.Numero,
                FornecedorId = compra.oFornecedor?.Id,
                CondicaoPagamentoId = compra.oCondicaoPagamento?.Id,
                compra.ValorTotal,
                compra.Ativo
            });
            return affected > 0;
        }

        public async Task<bool> UpdateAsync(Compras compra)
        {
            const string sql = @"
                UPDATE Compras SET
                    CondicaoPagamentoId = @CondicaoPagamentoId,
                    DataCompra = @DataCompra,
                    ValorTotal = @ValorTotal,
                    Ativo = @Ativo,
                    DataAlteracao = GETDATE()
                WHERE Modelo = @Modelo AND Serie = @Serie AND Numero = @Numero AND FornecedorId = @FornecedorId";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new
            {
                CondicaoPagamentoId = compra.oCondicaoPagamento?.Id,
                compra.DataCompra,
                compra.ValorTotal,
                compra.Ativo,
                compra.Modelo,
                compra.Serie,
                compra.Numero,
                FornecedorId = compra.oFornecedor?.Id
            });
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(char modelo, char serie, int numero, int fornecedorId)
        {
            const string sql = "DELETE FROM Compras WHERE Modelo = @Modelo AND Serie = @Serie AND Numero = @Numero AND FornecedorId = @FornecedorId";
            using var connection = _factory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { Modelo = modelo, Serie = serie, Numero = numero, FornecedorId = fornecedorId });
            return rowsAffected > 0;
        }
    }
}