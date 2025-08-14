namespace restauranteApi.Models
{
    public class Transportadoras
    {
        public int id { get; set; }
        public string? transportadora { get; set; }
        public string? cpfCnpj { get; set; }
        public string? inscricaoEstadual { get; set; }
        public string? inscricaoEstadualSubtrib { get; set; }

        // FKs
        public int? cidadeId { get; set; }
        public Cidades? cidade { get; set; }

        public int? condicoesPagamentoId { get; set; }
        public CondicoesPagamento? condicaoPagamento { get; set; }

        public string? endereco { get; set; }
        public int? numeroEndereco { get; set; }
        public string? bairro { get; set; }
        public string? complemento { get; set; }
        public string? cep { get; set; }
        public string? telefone { get; set; }
        public string? email { get; set; }
        public string? tipo { get; set; }
        public bool? ativo { get; set; }
        public DateTime? dataCadastro { get; set; }
        public DateTime? dataAlteracao { get; set; }
    }
}
