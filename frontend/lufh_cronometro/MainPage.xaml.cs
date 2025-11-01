namespace LUFH_Cronometro
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnClientesClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("clientes");
        }

        private async void OnProdutosClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("produtos");
        }

        private async void OnTestesClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("testes");
        }

        private async void OnMensuracoesClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Em breve", "Mensurações em desenvolvimento", "OK");
        }

        private async void OnUsuariosClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Em breve", "Usuários em desenvolvimento", "OK");
        }
    }
}
