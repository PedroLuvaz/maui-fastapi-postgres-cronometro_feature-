using MauiApp.Services;
using MauiApp.Models;
using MauiApp.Views.Edicao;
using System.Collections.ObjectModel;

namespace MauiApp.Views.Visualizacao
{
    public partial class ListaUsuariosPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly bool _modoEdicao;
        private ObservableCollection<Usuario> _usuarios;
        private List<Usuario> _todosUsuarios;

        public ListaUsuariosPage(bool modoEdicao = false)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _modoEdicao = modoEdicao;
            BindingContext = this;
            
            CarregarUsuarios();
        }

        public bool ModoEdicao => _modoEdicao;

        private async void CarregarUsuarios()
        {
            try
            {
                _todosUsuarios = await _apiService.ListarUsuariosAsync();
                _usuarios = new ObservableCollection<Usuario>(_todosUsuarios);
                
                UsuariosCollectionView.ItemsSource = _usuarios;
                AtualizarTotal();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível carregar os usuários: {ex.Message}", 
                    "OK");
            }
        }

        private void AtualizarTotal()
        {
            int total = _usuarios?.Count ?? 0;
            int ativos = _usuarios?.Count(u => u.Ativo) ?? 0;
            TotalLabel.Text = $"{total} usuário(s) • {ativos} ativo(s)";
        }

        private void OnBuscaTextChanged(object sender, TextChangedEventArgs e)
        {
            string busca = e.NewTextValue?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(busca))
            {
                UsuariosCollectionView.ItemsSource = new ObservableCollection<Usuario>(_todosUsuarios);
            }
            else
            {
                var filtrados = _todosUsuarios.Where(u =>
                    u.Nome.ToLower().Contains(busca) ||
                    (u.Email?.ToLower().Contains(busca) ?? false) ||
                    u.TipoUsuario.ToString().ToLower().Contains(busca)
                ).ToList();

                UsuariosCollectionView.ItemsSource = new ObservableCollection<Usuario>(filtrados);
            }
        }

        private async void OnUsuarioSelecionado(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Usuario usuarioSelecionado)
            {
                if (_modoEdicao)
                {
                    await Navigation.PushAsync(new EditarUsuarioPage(usuarioSelecionado.Id));
                }
                else
                {
                    await Navigation.PushAsync(new DetalhesUsuarioPage(usuarioSelecionado.Id));
                }

                ((CollectionView)sender).SelectedItem = null;
            }
        }

        private void OnAtualizarClicked(object sender, EventArgs e)
        {
            CarregarUsuarios();
        }

        private async void OnRefreshing(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            CarregarUsuarios();
            RefreshView.IsRefreshing = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarregarUsuarios();
        }
    }
}