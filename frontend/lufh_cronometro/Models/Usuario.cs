namespace MauiApp.Models
{
    public enum TipoUsuario
    {
        Admin,
        Tecnico,
        Coordenador,
        Voluntario
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}