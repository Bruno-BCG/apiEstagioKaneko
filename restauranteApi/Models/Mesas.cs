namespace restauranteApi.Models
{
    public class Mesas
    {
        public int numeroMesa { get; set; }
        public int? quantidadeCadeiras { get; set; }
        public string? localizacao { get; set; }
        public string? statusMesa { get; set; }
        public bool? ativo { get; set; }
        public DateTime? dataCadastro { get; set; }
        public DateTime? dataAlteracao { get; set; }
    }
}
