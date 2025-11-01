using MauiApp.Services;
using MauiApp.Models;
using MauiApp.Views.Edicao;
using System.Collections.ObjectModel;

namespace MauiApp.Views.Visualizacao
{
    public partial class ListaProdutosPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly bool _modoEdicao;
        private ObservableCollection<Produto> _produtos;
        private List<Produto> _todosProdutos;

        public ListaProdutosPage(bool modoEdicao = false)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _modoEdicao = modoEdicao;
            BindingContext = this;
            
            CarregarProdutos();
        }

        public bool ModoEdicao => _modoEdicao;

        private async void CarregarProdutos()
        {
            try
            {
                _todosProdutos = await _apiService.ListarProdutosAsync();
                _produtos = new ObservableCollection<Produto>(_todosProdutos);
                
                ProdutosCollectionView.ItemsSource = _produtos;
                AtualizarTotal();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível carregar os produtos: {ex.Message}", 
                    "OK");
            }
        }

        private void AtualizarTotal()
        {
            int total = _produtos?.Count ?? 0;
            int ativos = _produtos?.Count(p => p.Ativo) ?? 0;
            TotalLabel.Text = $"{total} produto(s) • {ativos} ativo(s)";
        }

        private void OnBuscaTextChanged(object sender, TextChangedEventArgs e)
        {
            string busca = e.NewTextValue?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(busca))
            {
                ProdutosCollectionView.ItemsSource = new ObservableCollection<Produto>(_todosProdutos);
            }
            else
            {
                var filtrados = _todosProdutos.Where(p =>
                    p.Nome.ToLower().Contains(busca) ||
                    (p.Descricao?.ToLower().Contains(busca) ?? false) ||
                    (p.Cliente?.Nome?.ToLower().Contains(busca) ?? false)
                ).ToList();

                ProdutosCollectionView.ItemsSource = new ObservableCollection<Produto>(filtrados);
            }
        }

        private async void OnProdutoSelecionado(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Produto produtoSelecionado)
            {
                if (_modoEdicao)
                {
                    await Navigation.PushAsync(new EditarProdutoPage(produtoSelecionado.Id));
                }
                else
                {
                    await Navigation.PushAsync(new DetalhesProdutoPage(produtoSelecionado.Id));
                }

                ((CollectionView)sender).SelectedItem = null;
            }
        }

        private void OnAtualizarClicked(object sender, EventArgs e)
        {
            CarregarProdutos();
        }

        private async void OnRefreshing(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            CarregarProdutos();
            RefreshView.IsRefreshing = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarregarProdutos();
        }
    }
}