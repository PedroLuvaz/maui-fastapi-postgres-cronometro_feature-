using MauiApp.Services;
using MauiApp.Models;

namespace MauiApp.Views.Visualizacao
{
    public partial class DetalhesClientePage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly int _clienteId;

        public DetalhesClientePage(int clienteId)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _clienteId = clienteId;
            CarregarDetalhes();
        }

        private async void CarregarDetalhes()
        {
            try
            {
                var cliente = await _apiService.ObterClienteAsync(_clienteId);

                NomeLabel.Text = cliente.Nome;
                StatusLabel.Text = cliente.Ativo ? "✅ Ativo" : "❌ Inativo";
                IdLabel.Text = cliente.Id.ToString();
                EmailLabel.Text = cliente.Email ?? "Não informado";
                TelefoneLabel.Text = cliente.Telefone ?? "Não informado";
                EmpresaLabel.Text = cliente.Empresa ?? "Não informado";
                DataCriacaoLabel.Text = cliente.DataCriacao.ToString("dd/MM/yyyy HH:mm");

                // Carregar produtos
                var produtos = await _apiService.ListarProdutosAsync();
                var produtosDoCliente = produtos.Where(p => p.ClienteId == _clienteId).ToList();
                
                if (produtosDoCliente.Any())
                {
                    ProdutosLabel.Text = string.Join("\n", produtosDoCliente.Select(p => $"• {p.Nome}"));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Não foi possível carregar os detalhes: {ex.Message}", "OK");
            }
        }
    }
}