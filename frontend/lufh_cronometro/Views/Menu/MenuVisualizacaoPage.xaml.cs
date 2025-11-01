using MauiApp.Views.Visualizacao;

namespace MauiApp.Views.Menu
{
    public partial class MenuVisualizacaoPage : ContentPage
    {
        public MenuVisualizacaoPage()
        {
            InitializeComponent();
        }

        private async void OnVisualizarClientesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaClientesPage(modoEdicao: false));
        }

        private async void OnVisualizarProdutosClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaProdutosPage(modoEdicao: false));
        }

        private async void OnVisualizarUsuariosClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaUsuariosPage(modoEdicao: false));
        }

        private async void OnVisualizarTestesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaTestesPage(modoEdicao: false));
        }
    }
}