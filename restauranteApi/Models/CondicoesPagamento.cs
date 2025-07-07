namespace restauranteApi.Models
{
    public class CondicoesPagamento
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int QuantidadeParcelas { get; set; }
        public decimal Juros { get; set; }
        public decimal Multa { get; set; }
        public decimal Desconto { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }
    } 
}
