namespace restauranteApi.Models
{
    // -------------------- Financeiro --------------------
    public class FormasPagamento
    {
        public int id { get; set; }
        public string? descricao { get; set; }
        public bool? ativo { get; set; }
        public DateTime? dataCadastro { get; set; }
        public DateTime? dataAlteracao { get; set; }
    }
}
