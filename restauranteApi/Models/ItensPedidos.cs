namespace restauranteApi.Models
{
    public class ItensPedidos
    {
        public int NumeroItem { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public string Observacao { get; set; }
        public decimal TotalItem { get; set; }

        public Pedidos oPedido { get; set; }
        public Produtos oProduto { get; set; }
        public Mesas oMesa { get; set; }
    }
}
