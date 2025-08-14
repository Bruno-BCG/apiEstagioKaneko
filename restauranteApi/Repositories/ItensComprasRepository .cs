// File: ItensComprasRepository.cs
using Dapper;
using Microsoft.Data.SqlClient;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace restauranteApi.Repositories
{
    public class ItensComprasRepository : IItensComprasRepository
    {
        private readonly SqlConnectionFactory _factory;

        public ItensComprasRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<itensPedidos>> GetAllAsync()
        {
            const string sql = "SELECT ic.*, c.Modelo AS CModelo, c.Serie AS CSerie, c.Numero AS CNumero, c.FornecedorId AS CFornecedorId, p.Id AS PId, p.Nome AS PNome FROM ItensCompras ic JOIN Compras c ON ic.Modelo = c.Modelo AND ic.Serie = c.Serie AND ic.Numero = c.Numero AND ic.FornecedorId = c.FornecedorId JOIN Produtos p ON ic.ProdutoId = p.Id";
            using var connection = _factory.CreateConnection();
            return await connection.QueryAsync<itensPedidos, Compras, Produtos, itensPedidos>(
                sql,
                (itemCompra, compra, produto) => {
                    itemCompra.oCompra = compra;
                    itemCompra.oProduto = produto;
                    return itemCompra;
                },
                splitOn: "CModelo,PId"
            );
        }

        public async Task<itensPedidos> GetByIdAsync(int id)
        {
            const string sql = "SELECT ic.*, c.Modelo AS CModelo, c.Serie AS CSerie, c.Numero AS CNumero, c.FornecedorId AS CFornecedorId, p.Id AS PId, p.Nome AS PNome FROM ItensCompras ic JOIN Compras c ON ic.Modelo = c.Modelo AND ic.Serie = c.Serie AND ic.Numero = c.Numero AND ic.FornecedorId = c.FornecedorId JOIN Produtos p ON ic.ProdutoId = p.Id WHERE ic.Id = @Id";
            using var connection = _factory.CreateConnection();
            var result = await connection.QueryAsync<itensPedidos, Compras, Produtos, itensPedidos>(
                sql,
                (itemCompra, compra, produto) => {
                    itemCompra.oCompra = compra;
                    itemCompra.oProduto = produto;
                    return itemCompra;
                },
                new { Id = id },
                splitOn: "CModelo,PId"
            );

            return result.SingleOrDefault();
        }

        public async Task<int> CreateAsync(itensPedidos itemCompra)
        {
            const string sql = @"
                INSERT INTO ItensCompras
                    (Modelo, Serie, Numero, FornecedorId, ProdutoId, Quantidade, PrecoUnitario, TotalItem)
                VALUES
                    (@Modelo, @Serie, @Numero, @FornecedorId, @ProdutoId, @Quantidade, @PrecoUnitario, @TotalItem);
                SELECT CAST(SCOPE_IDENTITY() AS INT);"; // Assuming Id is IDENTITY here based on SQL schema

            using var connection = _factory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                modelo = itemCompra.oCompra?.Modelo,
                serie = itemCompra.oCompra?.Serie,
                numero = itemCompra.oCompra?.Numero,
                FornecedorId = itemCompra.oCompra?.oFornecedor?.Id, // Accessing nested navigation property
                ProdutoId = itemCompra.oProduto?.Id,
                itemCompra.Quantidade,
                itemCompra.PrecoUnitario,
                itemCompra.TotalItem
            });
        }

        public async Task<bool> UpdateAsync(itensPedidos itemCompra)
        {
            const string sql = @"
                UPDATE ItensCompras SET
                    Modelo = @Modelo,
                    Serie = @Serie,
                    Numero = @Numero,
                    FornecedorId = @FornecedorId,
                    ProdutoId = @ProdutoId,
                    Quantidade = @Quantidade,
                    PrecoUnitario = @PrecoUnitario,
                    TotalItem = @TotalItem
                WHERE Id = @Id";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new
            {
                Modelo = itemCompra.oCompra?.Modelo,
                Serie = itemCompra.oCompra?.Serie,
                Numero = itemCompra.oCompra?.Numero,
                FornecedorId = itemCompra.oCompra?.oFornecedor?.Id,
                ProdutoId = itemCompra.oProduto?.Id,
                itemCompra.Quantidade,
                itemCompra.PrecoUnitario,
                itemCompra.TotalItem,
                itemCompra.Id
            });
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM ItensCompras WHERE Id = @Id";
            using var connection = _factory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}