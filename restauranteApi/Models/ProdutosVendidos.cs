namespace restauranteApi.Models
{
    public class ProdutosVendidos
    {
        public int NumeroItem { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public string Observacoes { get; set; }
        public decimal TotalItem { get; set; }

        public Vendas oVenda { get; set; }
        public Produtos oProduto { get; set; }
    }
}
