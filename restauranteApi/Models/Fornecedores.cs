namespace restauranteApi.Models
{
    public class Fornecedores
    {
        public int id { get; set; }
        public string? fornecedor { get; set; }
        public string? cpfCnpj { get; set; }
        public string? inscEstadualRg { get; set; }
        public DateTime? dataFundacaoNascimento { get; set; }

        // FKs
        public int? cidadeId { get; set; }
        public Cidades? cidade { get; set; }

        public int? condicoesPagamentoId { get; set; }
        public CondicoesPagamento? condicaoPagamento { get; set; }

        public string? endereco { get; set; }
        public int? numero { get; set; }
        public string? bairro { get; set; }
        public string? cep { get; set; }
        public string? complemento { get; set; }
        public string? telefone { get; set; }
        public bool? ativo { get; set; }
        public DateTime? dataCadastro { get; set; }
        public DateTime? dataAlteracao { get; set; }
    }
}
