using MauiApp.Services;
using MauiApp.Models;
using MauiApp.Views.Edicao;
using System.Collections.ObjectModel;

namespace MauiApp.Views.Visualizacao
{
    public partial class ListaClientesPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly bool _modoEdicao;
        private ObservableCollection<Cliente> _clientes;
        private List<Cliente> _todosClientes;

        public ListaClientesPage(bool modoEdicao = false)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _modoEdicao = modoEdicao;
            BindingContext = this;
            
            CarregarClientes();
        }

        public bool ModoEdicao => _modoEdicao;

        private async void CarregarClientes()
        {
            try
            {
                _todosClientes = await _apiService.ListarClientesAsync();
                _clientes = new ObservableCollection<Cliente>(_todosClientes);
                
                ClientesCollectionView.ItemsSource = _clientes;
                AtualizarTotal();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível carregar os clientes: {ex.Message}", 
                    "OK");
            }
        }

        private void AtualizarTotal()
        {
            int total = _clientes?.Count ?? 0;
            int ativos = _clientes?.Count(c => c.Ativo) ?? 0;
            TotalLabel.Text = $"{total} cliente(s) • {ativos} ativo(s)";
        }

        private void OnBuscaTextChanged(object sender, TextChangedEventArgs e)
        {
            string busca = e.NewTextValue?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(busca))
            {
                ClientesCollectionView.ItemsSource = new ObservableCollection<Cliente>(_todosClientes);
            }
            else
            {
                var filtrados = _todosClientes.Where(c =>
                    c.Nome.ToLower().Contains(busca) ||
                    (c.Email?.ToLower().Contains(busca) ?? false) ||
                    (c.Empresa?.ToLower().Contains(busca) ?? false)
                ).ToList();

                ClientesCollectionView.ItemsSource = new ObservableCollection<Cliente>(filtrados);
            }
        }

        private async void OnClienteSelecionado(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Cliente clienteSelecionado)
            {
                if (_modoEdicao)
                {
                    await Navigation.PushAsync(new EditarClientePage(clienteSelecionado.Id));
                }
                else
                {
                    await Navigation.PushAsync(new DetalhesClientePage(clienteSelecionado.Id));
                }

                // Limpar seleção
                ((CollectionView)sender).SelectedItem = null;
            }
        }

        private void OnAtualizarClicked(object sender, EventArgs e)
        {
            CarregarClientes();
        }

        private async void OnRefreshing(object sender, EventArgs e)
        {
            await Task.Delay(1000); // Simular delay
            CarregarClientes();
            RefreshView.IsRefreshing = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarregarClientes();
        }
    }
}