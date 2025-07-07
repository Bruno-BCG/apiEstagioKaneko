namespace restauranteApi.Models
{
    public class Estados
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public Paises oPais { get; set; }
    }
}
