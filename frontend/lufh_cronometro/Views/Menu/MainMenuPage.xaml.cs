using MauiApp.Views.Cadastro;
using MauiApp.Views.Edicao;
using MauiApp.Views.Visualizacao;
using MauiApp.Views.Mensuracao;
using MauiApp.Views.Auth;

namespace MauiApp.Views.Menu
{
    public partial class MainMenuPage : ContentPage
    {
        public MainMenuPage()
        {
            InitializeComponent();
            CarregarDadosUsuario();
        }

        private async void CarregarDadosUsuario()
        {
            try
            {
                var nome = await SecureStorage.GetAsync("user_nome");
                var tipo = await SecureStorage.GetAsync("user_tipo");

                if (!string.IsNullOrEmpty(nome))
                {
                    UsuarioNomeLabel.Text = $"👤 {nome} ({tipo})";
                }
            }
            catch (Exception)
            {
                UsuarioNomeLabel.Text = "Usuário";
            }
        }

        // ==================== GESTÃO ====================
        
        private async void OnCadastrarClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuCadastroPage());
        }

        private async void OnEditarClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuEdicaoPage());
        }

        private async void OnVisualizarClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuVisualizacaoPage());
        }

        private async void OnRelatoriosClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Em Desenvolvimento", 
                "Módulo de relatórios em breve!", 
                "OK");
        }

        // ==================== TESTES ====================

        private async void OnIniciarMensuracaoClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SelecionarTestePage());
        }

        private async void OnConfigurarRoteirosClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Em Desenvolvimento", 
                "Funcionalidade de configuração de roteiros será implementada em breve.", 
                "OK");
        }

        // ==================== SISTEMA ====================

        private async void OnConfiguracoesClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Configurações", 
                "Módulo de configurações em desenvolvimento.", 
                "OK");
        }

        private async void OnSairClicked(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert("Sair", 
                "Deseja realmente sair do sistema?", 
                "Sim", "Não");

            if (confirmar)
            {
                // Limpar dados de sessão
                SecureStorage.RemoveAll();

                // Voltar para login
                //Application.Current.MainPage = new NavigationPage(new LoginPage())
                {
                    //BarBackgroundColor = Color.FromArgb("#1976D2"),
                    //BarTextColor = Colors.White
                };
            }
        }
    }
}