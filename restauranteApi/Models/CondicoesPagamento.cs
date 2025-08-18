namespace restauranteApi.Models
{
    public class CondicoesPagamento
    {
        public int id { get; set; }
        public string? descricao { get; set; }
        public int quantidadeParcelas { get; set; }
        public decimal? juros { get; set; }
        public decimal? multa { get; set; }
        public decimal? desconto { get; set; }
        public bool? ativo { get; set; }
        public DateTime? dataCadastro { get; set; }
        public DateTime? dataAlteracao { get; set; }
        public List<Parcelamentos>? parcelamentos { get; set; }
    }
}
