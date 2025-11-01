using MauiApp.Services;
using MauiApp.Models;

namespace MauiApp.Views.Cadastro
{
    public partial class CadastroUsuarioPage : ContentPage
    {
        private readonly ApiService _apiService;

        public CadastroUsuarioPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            // Validações
            if (string.IsNullOrWhiteSpace(NomeEntry.Text))
            {
                await DisplayAlert("Erro", "O nome é obrigatório.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                await DisplayAlert("Erro", "O e-mail é obrigatório.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(SenhaEntry.Text) || SenhaEntry.Text.Length < 6)
            {
                await DisplayAlert("Erro", "A senha deve ter no mínimo 6 caracteres.", "OK");
                return;
            }

            if (SenhaEntry.Text != ConfirmarSenhaEntry.Text)
            {
                await DisplayAlert("Erro", "As senhas não coincidem.", "OK");
                return;
            }

            if (TipoUsuarioPicker.SelectedIndex < 0)
            {
                await DisplayAlert("Erro", "Selecione o tipo de usuário.", "OK");
                return;
            }

            try
            {
                var tipoUsuario = TipoUsuarioPicker.SelectedItem.ToString() switch
                {
                    "Admin" => TipoUsuario.Admin,
                    "Tecnico" => TipoUsuario.Tecnico,
                    "Coordenador" => TipoUsuario.Coordenador,
                    "Voluntario" => TipoUsuario.Voluntario,
                    _ => TipoUsuario.Voluntario
                };

                var novoUsuario = new Usuario
                {
                    Nome = NomeEntry.Text.Trim(),
                    Email = EmailEntry.Text.Trim(),
                    Senha = SenhaEntry.Text,
                    TipoUsuario = tipoUsuario
                };

                var usuarioCriado = await _apiService.CriarUsuarioAsync(novoUsuario);

                await DisplayAlert("Sucesso", 
                    $"Usuário '{usuarioCriado.Nome}' cadastrado com sucesso!", 
                    "OK");

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível cadastrar o usuário: {ex.Message}", 
                    "OK");
            }
        }

        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert("Cancelar", 
                "Deseja realmente cancelar o cadastro?", 
                "Sim", "Não");

            if (confirmar)
            {
                await Navigation.PopAsync();
            }
        }
    }
}