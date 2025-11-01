using MauiApp.Services;

namespace MauiApp.Views.Visualizacao
{
    public partial class DetalhesProdutoPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly int _produtoId;

        public DetalhesProdutoPage(int produtoId)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _produtoId = produtoId;
            CarregarDetalhes();
        }

        private async void CarregarDetalhes()
        {
            try
            {
                var produto = await _apiService.ObterProdutoAsync(_produtoId);

                NomeLabel.Text = produto.Nome;
                StatusLabel.Text = produto.Ativo ? "✅ Ativo" : "❌ Inativo";
                IdLabel.Text = produto.Id.ToString();
                VersaoLabel.Text = produto.Versao ?? "Não informado";
                ClienteLabel.Text = produto.Cliente?.Nome ?? "Não informado";
                DescricaoLabel.Text = produto.Descricao ?? "Sem descrição";
                DataCriacaoLabel.Text = produto.DataCriacao.ToString("dd/MM/yyyy HH:mm");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Não foi possível carregar os detalhes: {ex.Message}", "OK");
            }
        }
    }
}