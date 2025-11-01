using MauiApp.Views.Cadastro;

namespace MauiApp.Views.Menu
{
    public partial class MenuCadastroPage : ContentPage
    {
        public MenuCadastroPage()
        {
            InitializeComponent();
        }

        private async void OnCadastrarClienteClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CadastroClientePage());
        }

        private async void OnCadastrarProdutoClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CadastroProdutoPage());
        }

        private async void OnCadastrarUsuarioClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CadastroUsuarioPage());
        }

        private async void OnCadastrarTesteClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CadastroTestePage());
        }
    }
}