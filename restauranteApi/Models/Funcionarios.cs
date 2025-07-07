namespace restauranteApi.Models
{
    public class Funcionarios
    {
        public int Id { get; set; }
        public string Foto { get; set; }
        public string Nome { get; set; }
        public string Apelido { get; set; }
        public char Genero { get; set; }
        public string Endereco { get; set; }
        public int Numero { get; set; }
        public string Bairro { get; set; }
        public string CEP { get; set; }
        public string Complemento { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Matricula { get; set; }
        public string Cargo { get; set; }
        public decimal Salario { get; set; }
        public string Turno { get; set; }
        public int CargaHoraria { get; set; }
        public DateTime DataAdmissao { get; set; }
        public DateTime DataDemissao { get; set; }
        public decimal PorcentagemComissao { get; set; }
        public bool EhAdministrador { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public Cidades oCidade { get; set; }
    }
}
