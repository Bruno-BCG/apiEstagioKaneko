namespace restauranteApi.Models
{
    public class Veiculos
    {
        public int id { get; set; }

        // FKs
        public int transportadorasId { get; set; }
        public Transportadoras? transportadora { get; set; }

        public int? marcasId { get; set; }
        public Marcas? marca { get; set; }

        public string? placa { get; set; }
        public string? modelo { get; set; }
        public int? anoFabricacao { get; set; }
        public decimal? capacidadeCargaKg { get; set; }
        public bool? ativo { get; set; }
        public DateTime? dataCadastro { get; set; }
        public DateTime? dataAlteracao { get; set; }
    }
}
