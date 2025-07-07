namespace restauranteApi.Models
{
    public class ItensCompras
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal TotalItem { get; set; }

        public Compras oCompra { get; set; }
        public Produtos oProduto { get; set; }
    }
}
