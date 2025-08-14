namespace restauranteApi.Models
{
    public class Produtos
    {
        public int id { get; set; }
        public string? produto { get; set; }
        public string? imagem { get; set; }
        public decimal preco { get; set; }
        public string? descricao { get; set; }
        public int? estoque { get; set; }
        public int? tempoPreparo { get; set; }
        public string? ingredientes { get; set; }

        // FKs
        public int? marcasId { get; set; }
        public Marcas? marca { get; set; }

        public int grupoId { get; set; }
        public Grupos? grupo { get; set; }

        public bool ativo { get; set; }
        public DateTime? dataCadastro { get; set; }
        public DateTime? dataAlteracao { get; set; }
    }
}
