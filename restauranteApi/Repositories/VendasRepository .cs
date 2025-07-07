// File: VendasRepository.cs
using Dapper;
using Microsoft.Data.SqlClient;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace restauranteApi.Repositories
{
    public class VendasRepository : IVendasRepository
    {
        private readonly SqlConnectionFactory _factory;

        public VendasRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Vendas>> GetAllAsync()
        {
            const string sql = "SELECT v.*, c.Id AS CId, c.Nome AS CNome, cp.Id AS CPId, cp.Descricao AS CPDescricao FROM Vendas v LEFT JOIN Clientes c ON v.ClienteId = c.Id LEFT JOIN CondicoesPagamento cp ON v.CondicaoPagamentoId = cp.Id";
            using var connection = _factory.CreateConnection();
            return await connection.QueryAsync<Vendas, Clientes, CondicoesPagamento, Vendas>(
                sql,
                (venda, cliente, condicaoPagamento) => {
                    venda.oCliente = cliente;
                    venda.oCondicaoPagamento = condicaoPagamento;
                    return venda;
                },
                splitOn: "CId,CPId"
            );
        }

        public async Task<Vendas> GetByIdAsync(int id)
        {
            const string sql = "SELECT v.*, c.Id AS CId, c.Nome AS CNome, cp.Id AS CPId, cp.Descricao AS CPDescricao FROM Vendas v LEFT JOIN Clientes c ON v.ClienteId = c.Id LEFT JOIN CondicoesPagamento cp ON v.CondicaoPagamentoId = cp.Id WHERE v.Id = @Id";
            using var connection = _factory.CreateConnection();
            var result = await connection.QueryAsync<Vendas, Clientes, CondicoesPagamento, Vendas>(
                sql,
                (venda, cliente, condicaoPagamento) => {
                    venda.oCliente = cliente;
                    venda.oCondicaoPagamento = condicaoPagamento;
                    return venda;
                }, new { Id = id }, splitOn: "CId");

            return result.SingleOrDefault();
        }

        public async Task<int> CreateAsync(Vendas venda)
        {
            const string sql = @"
                INSERT INTO Vendas
                    (ClienteId, CondicaoPagamentoId, DataVenda, ValorTotal, Ativo, DataCadastro, DataAlteracao)
                VALUES
                    (@ClienteId, @CondicaoPagamentoId, GETDATE(), @ValorTotal, @Ativo, GETDATE(), NULL);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = _factory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                ClienteId = venda.oCliente?.Id,
                CondicaoPagamentoId = venda.oCondicaoPagamento?.Id,
                venda.ValorTotal,
                venda.Ativo
            });
        }

        public async Task<bool> UpdateAsync(Vendas venda)
        {
            const string sql = @"
                UPDATE Vendas SET
                    ClienteId = @ClienteId,
                    CondicaoPagamentoId = @CondicaoPagamentoId,
                    ValorTotal = @ValorTotal,
                    Ativo = @Ativo,
                    DataAlteracao = GETDATE()
                WHERE Id = @Id";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new
            {
                ClienteId = venda.oCliente?.Id,
                CondicaoPagamentoId = venda.oCondicaoPagamento?.Id,
                venda.ValorTotal,
                venda.Ativo,
                venda.Id
            });
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Vendas WHERE Id = @Id";
            using var connection = _factory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}