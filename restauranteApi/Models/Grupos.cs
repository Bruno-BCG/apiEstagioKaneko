namespace restauranteApi.Models
{
    public class Grupos
    {
        public int id { get; set; }
        public string? grupo { get; set; }
        public string? descricao { get; set; }
        public string? ipImpressora { get; set; }
        public string? imagem { get; set; }
        public bool? ativo { get; set; }
        public DateTime? dataCadastro { get; set; }
        public DateTime? dataAlteracao { get; set; }
    }
}
