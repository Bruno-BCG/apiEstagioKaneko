namespace restauranteApi.Models
{
    public class Produtos
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Imagem { get; set; }
        public decimal Preco { get; set; }
        public string Descricao { get; set; }
        public int Estoque { get; set; }
        public int TempoPreparo { get; set; }
        public string Ingredientes { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public Grupo oGrupo { get; set; }
    }
}
