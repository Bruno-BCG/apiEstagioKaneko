using Dapper;
using Microsoft.Data.SqlClient;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace restauranteApi.Repositories
{
    public class ItensPedidosRepository : IItensPedidosRepository
    {
        private readonly SqlConnectionFactory _factory;

        public ItensPedidosRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<ItensPedidos>> GetAllAsync()
        {
            const string sql = "SELECT ip.*, p.Id AS PId, p.Observacao AS PObservacao, prod.Id AS ProdId, prod.Nome AS ProdNome, m.NumeroMesa AS MNumeroMesa FROM ItensPedidos ip JOIN Pedidos p ON ip.pedidos_id = p.Id JOIN Produtos prod ON ip.produto_id = prod.Id JOIN Mesas m ON ip.numero_mesa = m.NumeroMesa";
            using var connection = _factory.CreateConnection();
            return await connection.QueryAsync<ItensPedidos, Pedidos, Produtos, Mesas, ItensPedidos>(
                sql,
                (itemPedido, pedido, produto, mesa) => {
                    itemPedido.oPedido = pedido;
                    itemPedido.oProduto = produto;
                    itemPedido.oMesa = mesa;
                    return itemPedido;
                },
                splitOn: "PId,ProdId,MNumeroMesa"
            );
        }

        public async Task<ItensPedidos> GetByIdAsync(int pedidosId, int numeroItem)
        {
            const string sql = "SELECT ip.*, p.Id AS PId, p.Observacao AS PObservacao, prod.Id AS ProdId, prod.Nome AS ProdNome, m.NumeroMesa AS MNumeroMesa FROM ItensPedidos ip JOIN Pedidos p ON ip.pedidos_id = p.Id JOIN Produtos prod ON ip.produto_id = prod.Id JOIN Mesas m ON ip.numero_mesa = m.NumeroMesa WHERE ip.pedidos_id = @PedidosId AND ip.numeroItem = @NumeroItem";
            using var connection = _factory.CreateConnection();
            var result = await connection.QueryAsync<ItensPedidos, Pedidos, Produtos, Mesas, ItensPedidos>(
                sql,
                (itemPedido, pedido, produto, mesa) =>
                {
                    itemPedido.oPedido = pedido;
                    itemPedido.oProduto = produto;
                    itemPedido.oMesa = mesa;
                    return itemPedido;
                },
                new { PedidosId = pedidosId, NumeroItem = numeroItem },
                splitOn: "PId,ProdId,MNumeroMesa"
            );

            return result.SingleOrDefault();
        }

        public async Task<bool> CreateAsync(ItensPedidos itemPedido)
        {
            const string sql = @"
                INSERT INTO ItensPedidos
                    (pedidos_id, numeroItem, produto_id, numero_mesa, quantidade, precoUnitario, observacao, totalItem)
                VALUES
                    (@pedidos_id, @numeroItem, @produto_id, @numero_mesa, @quantidade, @precoUnitario, @observacao, @totalItem);";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new
            {
                pedidos_id = itemPedido.oPedido?.Id,
                itemPedido.NumeroItem,
                produto_id = itemPedido.oProduto?.Id,
                numero_mesa = itemPedido.oMesa?.NumeroMesa,
                itemPedido.Quantidade,
                itemPedido.PrecoUnitario,
                itemPedido.Observacao,
                itemPedido.TotalItem
            });
            return affected > 0;
        }

        public async Task<bool> UpdateAsync(ItensPedidos itemPedido)
        {
            const string sql = @"
                UPDATE ItensPedidos SET
                    produto_id = @produto_id,
                    numero_mesa = @numero_mesa,
                    quantidade = @quantidade,
                    precoUnitario = @precoUnitario,
                    observacao = @observacao,
                    totalItem = @totalItem
                WHERE pedidos_id = @pedidos_id AND numeroItem = @numeroItem";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new
            {
                produto_id = itemPedido.oProduto?.Id,
                numero_mesa = itemPedido.oMesa?.NumeroMesa,
                itemPedido.Quantidade,
                itemPedido.PrecoUnitario,
                itemPedido.Observacao,
                itemPedido.TotalItem,
                pedidos_id = itemPedido.oPedido?.Id,
                itemPedido.NumeroItem
            });
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int pedidosId, int numeroItem)
        {
            const string sql = "DELETE FROM ItensPedidos WHERE pedidos_id = @PedidosId AND numeroItem = @NumeroItem";
            using var connection = _factory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { PedidosId = pedidosId, NumeroItem = numeroItem });
            return rowsAffected > 0;
        }
    }
}