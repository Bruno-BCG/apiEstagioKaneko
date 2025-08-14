namespace restauranteApi.Models
{
    public class Estados
    {
        public int id { get; set; }
        public string? estado { get; set; }
        public string? uf { get; set; }

        // FK -> Paises
        public int? paisId { get; set; }
        public Paises? pais { get; set; }

        public bool? ativo { get; set; }
        public DateTime? dataCadastro { get; set; }
        public DateTime? dataAlteracao { get; set; }
    }
}
