using MauiApp.Services;
using MauiApp.Models;

namespace MauiApp.Views.Cadastro
{
    public partial class CadastroTestePage : ContentPage
    {
        private readonly ApiService _apiService;
        private List<Cliente> _clientes;
        private List<Produto> _todosProdutos;
        private List<Usuario> _coordenadores;

        public CadastroTestePage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            CarregarDados();
        }

        private async void CarregarDados()
        {
            try
            {
                // Carregar clientes
                _clientes = await _apiService.ListarClientesAsync();
                ClientePicker.ItemsSource = _clientes.Select(c => c.Nome).ToList();

                // Carregar todos os produtos
                _todosProdutos = await _apiService.ListarProdutosAsync();

                // Carregar coordenadores
                var usuarios = await _apiService.ListarUsuariosAsync();
                _coordenadores = usuarios.Where(u => 
                    u.TipoUsuario == TipoUsuario.Coordenador || 
                    u.TipoUsuario == TipoUsuario.Admin
                ).ToList();
                CoordenadorPicker.ItemsSource = _coordenadores.Select(c => c.Nome).ToList();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível carregar os dados: {ex.Message}", 
                    "OK");
            }
        }

        private void OnClienteChanged(object sender, EventArgs e)
        {
            if (ClientePicker.SelectedIndex >= 0)
            {
                var clienteSelecionado = _clientes[ClientePicker.SelectedIndex];
                
                // Filtrar produtos do cliente
                var produtosDoCliente = _todosProdutos
                    .Where(p => p.ClienteId == clienteSelecionado.Id)
                    .ToList();

                ProdutoPicker.ItemsSource = produtosDoCliente.Select(p => p.Nome).ToList();
                ProdutoPicker.IsEnabled = produtosDoCliente.Any();

                if (!produtosDoCliente.Any())
                {
                    DisplayAlert("Aviso", 
                        "Este cliente não possui produtos cadastrados.", 
                        "OK");
                }
            }
        }

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            // Validações
            if (string.IsNullOrWhiteSpace(NomeEntry.Text))
            {
                await DisplayAlert("Erro", "O nome do teste é obrigatório.", "OK");
                return;
            }

            if (ClientePicker.SelectedIndex < 0)
            {
                await DisplayAlert("Erro", "Selecione um cliente.", "OK");
                return;
            }

            if (ProdutoPicker.SelectedIndex < 0)
            {
                await DisplayAlert("Erro", "Selecione um produto.", "OK");
                return;
            }

            if (CoordenadorPicker.SelectedIndex < 0)
            {
                await DisplayAlert("Erro", "Selecione um coordenador.", "OK");
                return;
            }

            try
            {
                var clienteSelecionado = _clientes[ClientePicker.SelectedIndex];
                var produtosDoCliente = _todosProdutos
                    .Where(p => p.ClienteId == clienteSelecionado.Id)
                    .ToList();
                var produtoSelecionado = produtosDoCliente[ProdutoPicker.SelectedIndex];
                var coordenadorSelecionado = _coordenadores[CoordenadorPicker.SelectedIndex];

                var novoTeste = new Teste
                {
                    Nome = NomeEntry.Text.Trim(),
                    Objetivo = ObjetivoEditor.Text?.Trim(),
                    ClienteId = clienteSelecionado.Id,
                    ProdutoId = produtoSelecionado.Id,
                    CoordenadorId = coordenadorSelecionado.Id
                };

                var testeCriado = await _apiService.CriarTesteAsync(novoTeste);

                await DisplayAlert("Sucesso", 
                    $"Teste '{testeCriado.Nome}' cadastrado com sucesso!", 
                    "OK");

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível cadastrar o teste: {ex.Message}", 
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