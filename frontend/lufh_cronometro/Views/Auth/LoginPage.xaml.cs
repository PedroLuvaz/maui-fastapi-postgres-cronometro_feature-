using MauiApp.Services;
using MauiApp.Models;
using MauiApp.Views.Menu;

namespace MauiApp.Views.Auth
{
    public partial class LoginPage : ContentPage
    {
        private readonly ApiService _apiService;

        public LoginPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(EmailEntry.Text) || 
                string.IsNullOrWhiteSpace(SenhaEntry.Text))
            {
                await DisplayAlert("Erro", "Por favor, preencha todos os campos.", "OK");
                return;
            }

            try
            {
                // Mostrar loading
                var loadingPage = new LoadingPage();
                await Navigation.PushModalAsync(loadingPage);

                // Fazer login
                var usuario = await _apiService.LoginAsync(
                    EmailEntry.Text.Trim(), 
                    SenhaEntry.Text
                );

                // Salvar dados do usuário no armazenamento seguro
                await SecureStorage.SetAsync("user_id", usuario.Id.ToString());
                await SecureStorage.SetAsync("user_nome", usuario.Nome);
                await SecureStorage.SetAsync("user_email", usuario.Email);
                await SecureStorage.SetAsync("user_tipo", usuario.TipoUsuario.ToString());

                // Fechar loading
                await Navigation.PopModalAsync();

                // Navegar para tela principal
                Application.Current.MainPage = new NavigationPage(new MainMenuPage())
                {
                    BarBackgroundColor = Color.FromArgb("#1976D2"),
                    BarTextColor = Colors.White
                };
            }
            catch (HttpRequestException)
            {
                await Navigation.PopModalAsync();
                await DisplayAlert("Erro de Conexão", 
                    "Não foi possível conectar ao servidor. Verifique sua conexão com a internet.", 
                    "OK");
            }
            catch (Exception ex)
            {
                await Navigation.PopModalAsync();
                await DisplayAlert("Erro de Autenticação", 
                    "E-mail ou senha incorretos. Tente novamente.", 
                    "OK");
            }
        }

        private async void OnEsqueciSenhaClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Recuperar Senha", 
                "Entre em contato com o administrador do sistema para recuperar sua senha.\n\nEmail: admin@lufh.com", 
                "OK");
        }

        // Método opcional: verificar se usuário já está logado
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Verificar se tem sessão ativa
            var userId = await SecureStorage.GetAsync("user_id");
            if (!string.IsNullOrEmpty(userId))
            {
                // Já está logado, ir direto para o menu
                Application.Current.MainPage = new NavigationPage(new MainMenuPage())
                {
                    BarBackgroundColor = Color.FromArgb("#1976D2"),
                    BarTextColor = Colors.White
                };
            }
        }
    }
}