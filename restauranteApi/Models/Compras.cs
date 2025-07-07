namespace restauranteApi.Models
{
    public class Compras
    {
        public char Modelo { get; set; }
        public char Serie { get; set; }
        public int Numero { get; set; }
        public DateTime DataCompra { get; set; }
        public decimal ValorTotal { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public Fornecedores oFornecedor { get; set; }
        public CondicoesPagamento oCondicaoPagamento { get; set; }
    }
}
