namespace restauranteApi.Models
{
    public class Vendas
    {
        public int Id { get; set; }
        public DateTime DataVenda { get; set; }
        public decimal ValorTotal { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public Clientes oCliente { get; set; }
        public CondicoesPagamento oCondicaoPagamento { get; set; }
    }
}
