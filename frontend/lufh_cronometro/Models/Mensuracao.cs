using System;
using System.Collections.Generic;

namespace MauiApp.Models
{
    public class MensuracaoModel  // RENOMEADO de Mensuracao para MensuracaoModel
    {
        public int Id { get; set; }
        public int TesteId { get; set; }
        public int VoluntarioId { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int TempoTotal { get; set; }
        public StatusMensuracao Status { get; set; }
        public string? Observacoes { get; set; }

        // Navegação
        public Teste? Teste { get; set; }
        public Usuario? Voluntario { get; set; }
        public List<Interrupcao>? Interrupcoes { get; set; }
        public List<Frustacao>? Frustacoes { get; set; }
    }

    public enum StatusMensuracao
    {
        Aguardando = 0,
        EmAndamento = 1,
        Concluida = 2,
        Cancelada = 3
    }
}