using MauiApp.Services;
using MauiApp.Models;

namespace MauiApp.Views.Cadastro
{
    public partial class CadastroClientePage : ContentPage
    {
        private readonly ApiService _apiService;

        public CadastroClientePage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            // Validação
            if (string.IsNullOrWhiteSpace(NomeEntry.Text))
            {
                await DisplayAlert("Erro", "O nome é obrigatório.", "OK");
                return;
            }

            try
            {
                var novoCliente = new Cliente
                {
                    Nome = NomeEntry.Text.Trim(),
                    Email = EmailEntry.Text?.Trim(),
                    Telefone = TelefoneEntry.Text?.Trim(),
                    Empresa = EmpresaEntry.Text?.Trim()
                };

                var clienteCriado = await _apiService.CriarClienteAsync(novoCliente);

                await DisplayAlert("Sucesso", 
                    $"Cliente '{clienteCriado.Nome}' cadastrado com sucesso!", 
                    "OK");

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível cadastrar o cliente: {ex.Message}", 
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