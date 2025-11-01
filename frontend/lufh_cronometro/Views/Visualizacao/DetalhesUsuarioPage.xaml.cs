using MauiApp.Services;
using MauiApp.Models;

namespace MauiApp.Views.Visualizacao
{
    public partial class DetalhesUsuarioPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly int _usuarioId;

        public DetalhesUsuarioPage(int usuarioId)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _usuarioId = usuarioId;
            CarregarDetalhes();
        }

        private async void CarregarDetalhes()
        {
            try
            {
                var usuario = await _apiService.ObterUsuarioAsync(_usuarioId);

                NomeLabel.Text = usuario.Nome;
                StatusLabel.Text = usuario.Ativo ? "✅ Ativo" : "❌ Inativo";
                IdLabel.Text = usuario.Id.ToString();
                EmailLabel.Text = usuario.Email;
                DataCriacaoLabel.Text = usuario.DataCriacao.ToString("dd/MM/yyyy HH:mm");
                
                TipoLabel.Text = usuario.TipoUsuario.ToString();
                TipoBadge.BackgroundColor = usuario.TipoUsuario switch
                {
                    TipoUsuario.Admin => Color.FromArgb("#F44336"),
                    TipoUsuario.Coordenador => Color.FromArgb("#2196F3"),
                    TipoUsuario.Tecnico => Color.FromArgb("#FF9800"),
                    TipoUsuario.Voluntario => Color.FromArgb("#4CAF50"),
                    _ => Color.FromArgb("#757575")
                };
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Não foi possível carregar os detalhes: {ex.Message}", "OK");
            }
        }
    }
}