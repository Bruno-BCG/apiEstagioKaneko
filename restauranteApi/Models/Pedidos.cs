namespace restauranteApi.Models
{
    public class Pedidos
    {
        public int Id { get; set; }
        public string Observacao { get; set; }
        public DateTime DataPedido { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public Mesas oMesa { get; set; }
        public Funcionarios oFuncionario { get; set; }
        public Vendas oVenda { get; set; }
    }
}
