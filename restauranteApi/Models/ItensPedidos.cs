namespace restauranteApi.Models
{
    public class ItensPedidos
    {
        // PK composta e FKs
        public int pedidosId { get; set; }    // alias de pedidos_id
        public int produtoId { get; set; }    // alias de produto_id
        public int numeroMesa { get; set; }   // alias de numero_mesa

        // Objetos aninhados
        public Pedidos? pedido { get; set; }
        public Produtos? produto { get; set; }
        public Mesas? mesa { get; set; }

        public int quantidade { get; set; }
        public decimal? precoUnitario { get; set; }
        public decimal? totalItem { get; set; }
    }
}
