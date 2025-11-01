using MauiApp.Views.Visualizacao;

namespace MauiApp.Views.Menu
{
    public partial class MenuEdicaoPage : ContentPage
    {
        public MenuEdicaoPage()
        {
            InitializeComponent();
        }

        private async void OnEditarClientesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaClientesPage(modoEdicao: true));
        }

        private async void OnEditarProdutosClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaProdutosPage(modoEdicao: true));
        }

        private async void OnEditarUsuariosClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaUsuariosPage(modoEdicao: true));
        }

        private async void OnEditarTestesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaTestesPage(modoEdicao: true));
        }
    }
}