namespace restauranteApi.Models
{
    public class Parcelamentos
    {
        public int NumeroParcela { get; set; }
        public int PrazoDias { get; set; }
        public decimal PorcentagemValor { get; set; }

        public CondicoesPagamento oCondicaoPagamento { get; set; }
        public FormasPagamento oFormaPagamento { get; set; }
    }
}
