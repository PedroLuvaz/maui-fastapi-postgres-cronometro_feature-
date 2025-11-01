using MauiApp.Services;
using MauiApp.Models;

namespace MauiApp.Views.Mensuracao
{
    public partial class SelecionarTestePage : ContentPage
    {
        private readonly ApiService _apiService;
        private List<Teste> _testes;
        private List<Usuario> _voluntarios;

        public SelecionarTestePage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            CarregarDados();
        }

        private async void CarregarDados()
        {
            try
            {
                // Carregar testes ativos
                var todosTestes = await _apiService.ListarTestesAsync();
                _testes = todosTestes.Where(t => t.Ativo).ToList();
                TestePicker.ItemsSource = _testes.Select(t => t.Nome).ToList();

                // Carregar voluntários
                var usuarios = await _apiService.ListarUsuariosAsync();
                _voluntarios = usuarios.Where(u => 
                    u.TipoUsuario == TipoUsuario.Voluntario && u.Ativo
                ).ToList();
                VoluntarioPicker.ItemsSource = _voluntarios.Select(v => v.Nome).ToList();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível carregar os dados: {ex.Message}", 
                    "OK");
            }
        }

        private void OnTesteChanged(object sender, EventArgs e)
        {
            if (TestePicker.SelectedIndex >= 0)
            {
                var testeSelecionado = _testes[TestePicker.SelectedIndex];
                
                InfoClienteLabel.Text = $"Cliente: {testeSelecionado.Cliente?.Nome ?? "N/A"}";
                InfoProdutoLabel.Text = $"Produto: {testeSelecionado.Produto?.Nome ?? "N/A"}";
                InfoCoordenadorLabel.Text = $"Coordenador: {testeSelecionado.Coordenador?.Nome ?? "N/A"}";
                
                InfoTesteFrame.IsVisible = true;
            }
            else
            {
                InfoTesteFrame.IsVisible = false;
            }
        }

        private async void OnIniciarClicked(object sender, EventArgs e)
        {
            // Validações
            if (TestePicker.SelectedIndex < 0)
            {
                await DisplayAlert("Erro", "Selecione um teste.", "OK");
                return;
            }

            if (VoluntarioPicker.SelectedIndex < 0)
            {
                await DisplayAlert("Erro", "Selecione um voluntário.", "OK");
                return;
            }

            try
            {
                var testeSelecionado = _testes[TestePicker.SelectedIndex];
                var voluntarioSelecionado = _voluntarios[VoluntarioPicker.SelectedIndex];

                // Criar mensuração
                var novaMensuracao = new MensuracaoModel  // ALTERADO
                {
                    TesteId = testeSelecionado.Id,
                    VoluntarioId = voluntarioSelecionado.Id,
                    Status = StatusMensuracao.Aguardando,
                    Observacoes = ObservacoesEditor.Text?.Trim()
                };

                var mensuracaoCriada = await _apiService.CriarMensuracaoAsync(novaMensuracao);

                // Navegar para cronômetro
                await Navigation.PushAsync(new CronometroPage(mensuracaoCriada.Id, testeSelecionado, voluntarioSelecionado));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível iniciar a mensuração: {ex.Message}", 
                    "OK");
            }
        }

        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}