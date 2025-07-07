// File: PedidosRepository.cs
using Dapper;
using Microsoft.Data.SqlClient;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace restauranteApi.Repositories
{
    public class PedidosRepository : IPedidosRepository
    {
        private readonly SqlConnectionFactory _factory;

        public PedidosRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Pedidos>> GetAllAsync()
        {
            const string sql = "SELECT p.*, m.NumeroMesa AS MNumeroMesa, f.Id AS FId, v.Id AS VId FROM Pedidos p LEFT JOIN Mesas m ON p.MesaNumero = m.NumeroMesa LEFT JOIN Funcionarios f ON p.FuncionarioId = f.Id LEFT JOIN Vendas v ON p.VendaId = v.Id";
            using var connection = _factory.CreateConnection();
            return await connection.QueryAsync<Pedidos, Mesas, Funcionarios, Vendas, Pedidos>(
                sql,
                (pedido, mesa, funcionario, venda) => {
                    pedido.oMesa = mesa;
                    pedido.oFuncionario = funcionario;
                    pedido.oVenda = venda;
                    return pedido;
                },
                splitOn: "MNumeroMesa,FId,VId"
            );
        }

        public async Task<Pedidos> GetByIdAsync(int id)
        {
            const string sql = "SELECT p.*, m.NumeroMesa AS MNumeroMesa, f.Id AS FId, v.Id AS VId FROM Pedidos p LEFT JOIN Mesas m ON p.MesaNumero = m.NumeroMesa LEFT JOIN Funcionarios f ON p.FuncionarioId = f.Id LEFT JOIN Vendas v ON p.VendaId = v.Id WHERE p.Id = @Id";
            using var connection = _factory.CreateConnection();
            var result = await connection.QueryAsync<Pedidos, Mesas, Funcionarios, Vendas, Pedidos>(
                sql,
                (pedido, mesa, funcionario, venda) => {
                    pedido.oMesa = mesa;
                    pedido.oFuncionario = funcionario;
                    pedido.oVenda = venda;
                    return pedido;
            }, new { Id = id }, splitOn: "MNumeroMesa,FId,VId");

            return result.SingleOrDefault();
        }

        public async Task<int> CreateAsync(Pedidos pedido)
        {
            const string sql = @"
                INSERT INTO Pedidos
                    (MesaNumero, FuncionarioId, VendaId, Observacao, DataPedido, Ativo, DataCadastro, DataAlteracao)
                VALUES
                    (@MesaNumero, @FuncionarioId, @VendaId, @Observacao, GETDATE(), @Ativo, GETDATE(), NULL);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = _factory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                MesaNumero = pedido.oMesa?.NumeroMesa,
                FuncionarioId = pedido.oFuncionario?.Id,
                VendaId = pedido.oVenda?.Id,
                pedido.Observacao,
                pedido.Ativo
            });
        }

        public async Task<bool> UpdateAsync(Pedidos pedido)
        {
            const string sql = @"
                UPDATE Pedidos SET
                    MesaNumero = @MesaNumero,
                    FuncionarioId = @FuncionarioId,
                    VendaId = @VendaId,
                    Observacao = @Observacao,
                    Ativo = @Ativo,
                    DataAlteracao = GETDATE()
                WHERE Id = @Id";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new
            {
                MesaNumero = pedido.oMesa?.NumeroMesa,
                FuncionarioId = pedido.oFuncionario?.Id,
                VendaId = pedido.oVenda?.Id,
                pedido.Observacao,
                pedido.Ativo,
                pedido.Id
            });
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Pedidos WHERE Id = @Id";
            using var connection = _factory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}