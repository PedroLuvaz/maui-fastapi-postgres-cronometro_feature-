using MauiApp.Services;
using MauiApp.Models;
using MauiApp.Views.Edicao;
using System.Collections.ObjectModel;

namespace MauiApp.Views.Visualizacao
{
    public partial class ListaTestesPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly bool _modoEdicao;
        private ObservableCollection<Teste> _testes;
        private List<Teste> _todosTestes;

        public ListaTestesPage(bool modoEdicao = false)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _modoEdicao = modoEdicao;
            BindingContext = this;
            
            CarregarTestes();
        }

        public bool ModoEdicao => _modoEdicao;

        private async void CarregarTestes()
        {
            try
            {
                _todosTestes = await _apiService.ListarTestesAsync();
                _testes = new ObservableCollection<Teste>(_todosTestes);
                
                TestesCollectionView.ItemsSource = _testes;
                AtualizarTotal();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível carregar os testes: {ex.Message}", 
                    "OK");
            }
        }

        private void AtualizarTotal()
        {
            int total = _testes?.Count ?? 0;
            int ativos = _testes?.Count(t => t.Ativo) ?? 0;
            TotalLabel.Text = $"{total} teste(s) • {ativos} ativo(s)";
        }

        private void OnBuscaTextChanged(object sender, TextChangedEventArgs e)
        {
            string busca = e.NewTextValue?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(busca))
            {
                TestesCollectionView.ItemsSource = new ObservableCollection<Teste>(_todosTestes);
            }
            else
            {
                var filtrados = _todosTestes.Where(t =>
                    t.Nome.ToLower().Contains(busca) ||
                    (t.Objetivo?.ToLower().Contains(busca) ?? false) ||
                    (t.Cliente?.Nome?.ToLower().Contains(busca) ?? false) ||
                    (t.Produto?.Nome?.ToLower().Contains(busca) ?? false)
                ).ToList();

                TestesCollectionView.ItemsSource = new ObservableCollection<Teste>(filtrados);
            }
        }

        private async void OnTesteSelecionado(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Teste testeSelecionado)
            {
                if (_modoEdicao)
                {
                    //await Navigation.PushAsync(new EditarTestePage(testeSelecionado.Id));
                }
                else
                {
                    await Navigation.PushAsync(new DetalhesTestePage(testeSelecionado.Id));
                }

                ((CollectionView)sender).SelectedItem = null;
            }
        }

        private void OnAtualizarClicked(object sender, EventArgs e)
        {
            CarregarTestes();
        }

        private async void OnRefreshing(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            CarregarTestes();
            RefreshView.IsRefreshing = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarregarTestes();
        }
    }
}