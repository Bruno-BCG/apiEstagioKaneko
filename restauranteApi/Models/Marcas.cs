namespace restauranteApi.Models
{
    public class Marcas
    {
        public int id { get; set; }
        public string? marca { get; set; }
        public bool ativo { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime? dataAlteracao { get; set; }
    }
}
