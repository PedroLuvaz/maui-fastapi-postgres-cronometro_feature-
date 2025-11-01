using System;

namespace MauiApp.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public string? Versao { get; set; }  // ← ADICIONAR ESTA LINHA
        public int ClienteId { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool Ativo { get; set; }

        // Navegação
        public Cliente? Cliente { get; set; }
    }
}