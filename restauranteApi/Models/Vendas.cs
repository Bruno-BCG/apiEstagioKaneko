namespace restauranteApi.Models
{
    public class Vendas
    {
        public int id { get; set; }

        // FKs
        public int? clientesId { get; set; }
        public Clientes? cliente { get; set; }

        public int? condicoesPagamentoId { get; set; }
        public CondicoesPagamento? condicaoPagamento { get; set; }

        public DateTime? dataVenda { get; set; }
        public decimal? valorTotal { get; set; }
        public bool? ativo { get; set; }
        public DateTime? dataCadastro { get; set; }
        public DateTime? dataAlteracao { get; set; }
    }
}
