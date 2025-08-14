namespace restauranteApi.Models
{
    // -------------------- Pessoas/Entidades --------------------
    public class Funcionarios
    {
        public int id { get; set; }
        public string? foto { get; set; }
        public string? funcionario { get; set; }
        public string? apelido { get; set; }
        public string? genero { get; set; } // CHAR(1)

        // FK -> Cidades
        public int? cidadeId { get; set; }
        public Cidades? cidade { get; set; }

        public string? endereco { get; set; }
        public int? numero { get; set; }
        public string? bairro { get; set; }
        public string? cep { get; set; }
        public string? complemento { get; set; }
        public string? cpfCnpj { get; set; }
        public string? rg { get; set; }
        public DateTime? dataNascimento { get; set; }
        public string? telefone { get; set; }
        public string? email { get; set; }
        public string? matricula { get; set; }
        public string? cargo { get; set; }
        public decimal? salario { get; set; }
        public string? turno { get; set; }
        public int? cargaHoraria { get; set; }
        public DateTime? dataAdmissao { get; set; }
        public DateTime? dataDemissao { get; set; }
        public decimal? porcentagemComissao { get; set; }
        public bool? ehAdministrador { get; set; }
        public bool? ativo { get; set; }
        public DateTime? dataCadastro { get; set; }
        public DateTime? dataAlteracao { get; set; }
    }
}
