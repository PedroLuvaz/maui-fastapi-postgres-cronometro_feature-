using System;

namespace MauiApp.Models
{
    public class Frustacao
    {
        public int Id { get; set; }
        public int MensuracaoId { get; set; }
        public DateTime Momento { get; set; }
        public int Nivel { get; set; } // 1 a 5
        public string? Descricao { get; set; }

        // Navegação
        public MensuracaoModel? Mensuracao { get; set; }
    }
}