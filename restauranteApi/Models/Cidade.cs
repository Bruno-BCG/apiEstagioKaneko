namespace restauranteApi.Models
{
    public class Cidades
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public Estados oEstado { get; set; }
    }
}
