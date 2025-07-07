namespace restauranteApi.Models
{
    public class Clientes
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Apelido { get; set; }
        public char Genero { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string Anotacao { get; set; }
        public string PratoPreferencial { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public Cidades oCidade { get; set; }
    }
}
