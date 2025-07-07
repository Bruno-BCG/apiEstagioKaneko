namespace restauranteApi.Models
{
    public class Fornecedores
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string InscEstadualRg { get; set; }
        public DateTime DataFundacaoNascimento { get; set; }
        public string Endereco { get; set; }
        public int Numero { get; set; }
        public string Bairro { get; set; }
        public string CEP { get; set; }
        public string Complemento { get; set; }
        public string Telefone { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public Cidades oCidade { get; set; }
    }
}
