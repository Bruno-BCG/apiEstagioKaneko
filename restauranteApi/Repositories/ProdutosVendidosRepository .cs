
using Dapper;
using Microsoft.Data.SqlClient;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace restauranteApi.Repositories
{
    public class ProdutosVendidosRepository : IProdutosVendidosRepository
    {
        private readonly SqlConnectionFactory _factory;

        public ProdutosVendidosRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<ProdutosVendidos>> GetAllAsync()
        {
            const string sql = "SELECT pv.*, v.Id AS VId, v.ValorTotal AS VValorTotal, p.Id AS PId, p.Nome AS PNome FROM ProdutosVendidos pv JOIN Vendas v ON pv.venda_id = v.Id JOIN Produtos p ON pv.produto_id = p.Id";
            using var connection = _factory.CreateConnection();
            return await connection.QueryAsync<ProdutosVendidos, Vendas, Produtos, ProdutosVendidos>(
                sql,
                (produtoVendido, venda, produto) => {
                    produtoVendido.oVenda = venda;
                    produtoVendido.oProduto = produto;
                    return produtoVendido;
                },
                splitOn: "VId,PId"
            );
        }

        public async Task<ProdutosVendidos> GetByIdAsync(int numeroItem, int vendaId)
        {
            const string sql = "SELECT pv.*, v.Id AS VId, v.ValorTotal AS VValorTotal, p.Id AS PId, p.Nome AS PNome FROM ProdutosVendidos pv JOIN Vendas v ON pv.venda_id = v.Id JOIN Produtos p ON pv.produto_id = p.Id WHERE pv.numeroItem = @NumeroItem AND pv.venda_id = @VendaId";
            using var connection = _factory.CreateConnection();
            var result = await connection.QueryAsync<ProdutosVendidos, Vendas, Produtos, ProdutosVendidos>(
                sql,
                (produtoVendido, venda, produto) => {
                    produtoVendido.oVenda = venda;
                    produtoVendido.oProduto = produto;
                    return produtoVendido;
                },
                new { NumeroItem = numeroItem, VendaId = vendaId },
                splitOn: "VId,PId"
            );

            return result.SingleOrDefault();
        }

        public async Task<bool> CreateAsync(ProdutosVendidos produtoVendido)
        {
            const string sql = @"
                INSERT INTO ProdutosVendidos
                    (numeroItem, venda_id, produto_id, quantidade, precoUnitario, observacoes, totalItem)
                VALUES
                    (@numeroItem, @venda_id, @produto_id, @quantidade, @precoUnitario, @observacoes, @totalItem);";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new
            {
                produtoVendido.NumeroItem,
                venda_id = produtoVendido.oVenda?.Id,
                produto_id = produtoVendido.oProduto?.Id,
                produtoVendido.Quantidade,
                produtoVendido.PrecoUnitario,
                produtoVendido.Observacoes,
                produtoVendido.TotalItem
            });
            return affected > 0;
        }

        public async Task<bool> UpdateAsync(ProdutosVendidos produtoVendido)
        {
            const string sql = @"
                UPDATE ProdutosVendidos SET
                    produto_id = @produto_id,
                    quantidade = @quantidade,
                    precoUnitario = @precoUnitario,
                    observacoes = @observacoes,
                    totalItem = @totalItem
                WHERE numeroItem = @numeroItem AND venda_id = @venda_id";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new
            {
                produto_id = produtoVendido.oProduto?.Id,
                produtoVendido.Quantidade,
                produtoVendido.PrecoUnitario,
                produtoVendido.Observacoes,
                produtoVendido.TotalItem,
                produtoVendido.NumeroItem,
                venda_id = produtoVendido.oVenda?.Id
            });
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int numeroItem, int vendaId)
        {
            const string sql = "DELETE FROM ProdutosVendidos WHERE numeroItem = @NumeroItem AND venda_id = @VendaId";
            using var connection = _factory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { NumeroItem = numeroItem, VendaId = vendaId });
            return rowsAffected > 0;
        }
    }
}