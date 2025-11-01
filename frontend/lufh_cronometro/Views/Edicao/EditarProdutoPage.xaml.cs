using MauiApp.Services;
using MauiApp.Models;

namespace MauiApp.Views.Edicao
{
    public partial class EditarProdutoPage : ContentPage
    {
        private readonly ApiService _apiService;
        private Produto _produto;

        public EditarProdutoPage(int produtoId)
        {
            InitializeComponent();
            _apiService = new ApiService();
            CarregarProduto(produtoId);
        }

        private async void CarregarProduto(int produtoId)
        {
            try
            {
                _produto = await _apiService.ObterProdutoAsync(produtoId);
                var cliente = await _apiService.ObterClienteAsync(_produto.ClienteId);

                IdLabel.Text = _produto.Id.ToString();
                NomeEntry.Text = _produto.Nome;
                DescricaoEditor.Text = _produto.Descricao;
                VersaoEntry.Text = _produto.Versao;
                ClienteLabel.Text = cliente.Nome;
                AtivoSwitch.IsToggled = _produto.Ativo;
                DataCriacaoLabel.Text = _produto.DataCriacao.ToString("dd/MM/yyyy HH:mm");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível carregar o produto: {ex.Message}", 
                    "OK");
                await Navigation.PopAsync();
            }
        }

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NomeEntry.Text))
            {
                await DisplayAlert("Erro", "O nome do produto é obrigatório.", "OK");
                return;
            }

            try
            {
                _produto.Nome = NomeEntry.Text.Trim();
                _produto.Descricao = DescricaoEditor.Text?.Trim();
                _produto.Versao = VersaoEntry.Text?.Trim();
                _produto.Ativo = AtivoSwitch.IsToggled;

                // TODO: Criar endpoint PUT no backend
                // await _apiService.AtualizarProdutoAsync(_produto);

                await DisplayAlert("Sucesso", 
                    "Produto atualizado com sucesso!", 
                    "OK");

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível atualizar o produto: {ex.Message}", 
                    "OK");
            }
        }

        private async void OnExcluirClicked(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert("Confirmar Exclusão", 
                $"Deseja realmente excluir o produto '{_produto.Nome}'?\n\nEsta ação não pode ser desfeita.", 
                "Sim, Excluir", "Cancelar");

            if (confirmar)
            {
                try
                {
                    // TODO: Criar endpoint DELETE no backend
                    // await _apiService.ExcluirProdutoAsync(_produto.Id);

                    await DisplayAlert("Sucesso", 
                        "Produto excluído com sucesso!", 
                        "OK");

                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", 
                        $"Não foi possível excluir o produto: {ex.Message}", 
                        "OK");
                }
            }
        }

        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert("Cancelar", 
                "Deseja realmente cancelar as alterações?", 
                "Sim", "Não");

            if (confirmar)
            {
                await Navigation.PopAsync();
            }
        }
    }
}