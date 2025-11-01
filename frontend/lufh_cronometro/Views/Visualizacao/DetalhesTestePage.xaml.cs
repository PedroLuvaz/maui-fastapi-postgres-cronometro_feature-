using MauiApp.Services;
using MauiApp.Models;
using MauiApp.Views.Mensuracao;

namespace MauiApp.Views.Visualizacao
{
    public partial class DetalhesTestePage : ContentPage
    {
        private readonly ApiService _apiService;
        private Teste _teste;

        public DetalhesTestePage(int testeId)
        {
            InitializeComponent();
            _apiService = new ApiService();
            CarregarTeste(testeId);
        }

        private async void CarregarTeste(int testeId)
        {
            try
            {
                _teste = await _apiService.ObterTesteAsync(testeId);

                // Informações Básicas
                IdLabel.Text = $"ID: {_teste.Id}";
                NomeLabel.Text = _teste.Nome;
                ObjetivoLabel.Text = _teste.Objetivo ?? "Não informado";
                DataCriacaoLabel.Text = _teste.DataCriacao.ToString("dd/MM/yyyy HH:mm");

                // Status
                StatusLabel.Text = _teste.Ativo ? "ATIVO" : "INATIVO";
                StatusLabel.TextColor = _teste.Ativo ? 
                    Color.FromArgb("#4CAF50") : 
                    Color.FromArgb("#F44336");

                // Relacionamentos
                ClienteLabel.Text = _teste.Cliente?.Nome ?? "N/A";
                // REMOVIDO: ProdutoLabel (não existe mais)
                CoordenadorLabel.Text = _teste.Coordenador?.Nome ?? "N/A";

                // Estatísticas (TODO: implementar no backend)
                TotalMensuracoesLabel.Text = "0";
                ConcluidasLabel.Text = "0";
                EmAndamentoLabel.Text = "0";

                // Roteiros (TODO: implementar)
                RoteirosLabel.Text = "Funcionalidade em desenvolvimento";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível carregar o teste: {ex.Message}", 
                    "OK");
                await Navigation.PopAsync();
            }
        }

        private async void OnClienteClicked(object sender, EventArgs e)
        {
            if (_teste?.Cliente != null)
            {
                await Navigation.PushAsync(new DetalhesClientePage(_teste.Cliente.Id));
            }
        }

        // REMOVIDO: OnProdutoClicked (não existe mais)

        private async void OnCoordenadorClicked(object sender, EventArgs e)
        {
            if (_teste?.Coordenador != null)
            {
                await Navigation.PushAsync(new DetalhesUsuarioPage(_teste.Coordenador.Id));
            }
        }

        private async void OnVerMensuracoesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaMensuracoesPage(_teste.Id));
        }

        private async void OnConfigurarRoteirosClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Em Desenvolvimento", 
                "Funcionalidade de configuração de roteiros será implementada em breve.", 
                "OK");
        }
    }
}