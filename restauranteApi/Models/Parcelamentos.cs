namespace restauranteApi.Models
{
    public class Parcelamentos
    {
        public int numeroParcela { get; set; }

        // FKs
        public int condicoesPagamentoId { get; set; }
        public CondicoesPagamento? condicaoPagamento { get; set; }

        public int formasPagamentoId { get; set; }
        public FormasPagamento? formaPagamento { get; set; }

        public int prazoDias { get; set; }
        public decimal porcentagemValor { get; set; }
    }
}
