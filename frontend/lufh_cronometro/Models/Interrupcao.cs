using System;

namespace MauiApp.Models
{
    public class Interrupcao
    {
        public int Id { get; set; }
        public int MensuracaoId { get; set; }
        public DateTime Momento { get; set; }
        public string? Motivo { get; set; }
        public int Duracao { get; set; } // em segundos

        // Navegação
        public MensuracaoModel? Mensuracao { get; set; }
    }
}