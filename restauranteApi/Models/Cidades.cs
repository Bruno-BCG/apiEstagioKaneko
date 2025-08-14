namespace restauranteApi.Models
{
    public class Cidades
    {
        public int id { get; set; }
        public string? cidade { get; set; }

        // FK -> Estados
        public int? estadoId { get; set; }
        public Estados? estado { get; set; }

        public bool? ativo { get; set; }
        public DateTime? dataCadastro { get; set; }
        public DateTime? dataAlteracao { get; set; }
    }
}
