namespace MauiApp.Models
{
    public class Teste
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Objetivo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int CoordenadorId { get; set; }
        public int ClienteId { get; set; }
        public int ProdutoId { get; set; }
        public bool Ativo { get; set; }
        
        // Navegação
        public Usuario Coordenador { get; set; }
        public Cliente Cliente { get; set; }
        public Produto Produto { get; set; }
        public List<Roteiro> Roteiros { get; set; }
    }
}