namespace MauiApp.Models
{
    public class Roteiro
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int TesteId { get; set; }
        public int Ordem { get; set; }
        public bool Ativo { get; set; }
        public List<Tarefa> Tarefas { get; set; }
    }

    public class Tarefa
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Objetivo { get; set; }
        public int RoteiroId { get; set; }
        public int Ordem { get; set; }
        public int? TempoEstimado { get; set; }
        public bool Ativo { get; set; }
    }
}