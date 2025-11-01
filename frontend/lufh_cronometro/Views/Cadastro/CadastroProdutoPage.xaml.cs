using MauiApp.Services;
using MauiApp.Models;

namespace MauiApp.Views.Cadastro
{
    public partial class CadastroProdutoPage : ContentPage
    {
        private readonly ApiService _apiService;
        private List<Cliente> _clientes;

        public CadastroProdutoPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            CarregarClientes();
        }

        private async void CarregarClientes()
        {
            try
            {
                _clientes = await _apiService.ListarClientesAsync();
                
                ClientePicker.ItemsSource = _clientes.Select(c => c.Nome).ToList();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível carregar clientes: {ex.Message}", 
                    "OK");
            }
        }

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            // Validação
            if (string.IsNullOrWhiteSpace(NomeEntry.Text))
            {
                await DisplayAlert("Erro", "O nome do produto é obrigatório.", "OK");
                return;
            }

            if (ClientePicker.SelectedIndex < 0)
            {
                await DisplayAlert("Erro", "Selecione um cliente.", "OK");
                return;
            }

            try
            {
                var clienteSelecionado = _clientes[ClientePicker.SelectedIndex];

                var novoProduto = new Produto
                {
                    Nome = NomeEntry.Text.Trim(),
                    Descricao = DescricaoEditor.Text?.Trim(),
                    Versao = VersaoEntry.Text?.Trim(),
                    ClienteId = clienteSelecionado.Id
                };

                var produtoCriado = await _apiService.CriarProdutoAsync(novoProduto);

                await DisplayAlert("Sucesso", 
                    $"Produto '{produtoCriado.Nome}' cadastrado com sucesso!", 
                    "OK");

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível cadastrar o produto: {ex.Message}", 
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