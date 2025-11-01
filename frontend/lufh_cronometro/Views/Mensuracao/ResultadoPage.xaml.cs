using MauiApp.Services;
using MauiApp.Models;
using MauiApp.Views.Menu;

namespace MauiApp.Views.Mensuracao
{
    public partial class ResultadoPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly int _mensuracaoId;
        private MensuracaoModel _mensuracao;  // ALTERADO

        public ResultadoPage(int mensuracaoId)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _mensuracaoId = mensuracaoId;
            CarregarResultado();
        }

        private async void CarregarResultado()
        {
            try
            {
                _mensuracao = await _apiService.ObterMensuracaoAsync(_mensuracaoId);

                // Informações Gerais
                TesteLabel.Text = _mensuracao.Teste?.Nome ?? "N/A";
                VoluntarioLabel.Text = _mensuracao.Voluntario?.Nome ?? "N/A";
                DataHoraLabel.Text = _mensuracao.DataInicio?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
                
                TimeSpan duracao = TimeSpan.FromSeconds(_mensuracao.TempoTotal);
                DuracaoLabel.Text = $"{duracao:hh\\:mm\\:ss}";

                // Estatísticas
                InterrupcoesLabel.Text = _mensuracao.Interrupcoes?.Count.ToString() ?? "0";
                FrustracoesLabel.Text = _mensuracao.Frustacoes?.Count.ToString() ?? "0";
                TarefasLabel.Text = "0/0"; // TODO: Implementar contagem de tarefas

                // Observações
                if (!string.IsNullOrWhiteSpace(_mensuracao.Observacoes))
                {
                    ObservacoesLabel.Text = _mensuracao.Observacoes;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível carregar os resultados: {ex.Message}", 
                    "OK");
            }
        }

        private async void OnVerDetalhesClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Em Desenvolvimento", 
                "Visualização detalhada em breve!", 
                "OK");
        }

        private async void OnVoltarMenuClicked(object sender, EventArgs e)
        {
            // Voltar para o menu principal (limpar pilha de navegação)
            Application.Current.MainPage = new NavigationPage(new MainMenuPage())
            {
                BarBackgroundColor = Color.FromArgb("#1976D2"),
                BarTextColor = Colors.White
            };
        }

        protected override bool OnBackButtonPressed()
        {
            // Bloquear voltar no hardware
            return true;
        }
    }
}