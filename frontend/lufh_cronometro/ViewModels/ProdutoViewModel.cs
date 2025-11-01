using LUFH_Cronometro.Models;
using LUFH_Cronometro.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LUFH_Cronometro.ViewModels
{
    public class ProdutoViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private ObservableCollection<Produto> _produtos;
        private Produto _produtoSelecionado;
        private bool _isRefreshing;

        public ObservableCollection<Produto> Produtos
        {
            get => _produtos;
            set => SetProperty(ref _produtos, value);
        }

        public Produto ProdutoSelecionado
        {
            get => _produtoSelecionado;
            set => SetProperty(ref _produtoSelecionado, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public ICommand CarregarProdutosCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand SelecionarProdutoCommand { get; }

        public ProdutoViewModel()
        {
            _apiService = new ApiService();
            Produtos = new ObservableCollection<Produto>();

            CarregarProdutosCommand = new Command(async () => await CarregarProdutos());
            RefreshCommand = new Command(async () => await RefreshProdutos());
            SelecionarProdutoCommand = new Command<Produto>(OnProdutoSelecionado);

            Title = "Produtos";
        }

        public async Task CarregarProdutos()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var produtos = await _apiService.GetAsync<Produto>("/produtos");
                
                Produtos.Clear();
                foreach (var produto in produtos)
                {
                    Produtos.Add(produto);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", 
                    $"Não foi possível carregar produtos: {ex.Message}", 
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RefreshProdutos()
        {
            IsRefreshing = true;
            await CarregarProdutos();
            IsRefreshing = false;
        }

        private void OnProdutoSelecionado(Produto produto)
        {
            if (produto == null)
                return;

            ProdutoSelecionado = produto;
        }

        public async Task<List<Produto>> ObterProdutosPorCliente(int clienteId)
        {
            try
            {
                var todosProdutos = await _apiService.ListarProdutosAsync();
                return todosProdutos.Where(p => p.ClienteId == clienteId).ToList();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", 
                    $"Não foi possível obter produtos: {ex.Message}", 
                    "OK");
                return new List<Produto>();
            }
        }

        public async Task<bool> CriarProduto(Produto produto)
        {
            try
            {
                IsBusy = true;
                await _apiService.CriarProdutoAsync(produto);
                await CarregarProdutos();
                return true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", 
                    $"Não foi possível criar produto: {ex.Message}", 
                    "OK");
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task<bool> AtualizarProduto(Produto produto)
        {
            try
            {
                IsBusy = true;
                // TODO: Implementar endpoint PUT no backend
                // await _apiService.AtualizarProdutoAsync(produto);
                await CarregarProdutos();
                return true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", 
                    $"Não foi possível atualizar produto: {ex.Message}", 
                    "OK");
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task<bool> ExcluirProduto(int id)
        {
            try
            {
                IsBusy = true;
                // TODO: Implementar endpoint DELETE no backend
                // await _apiService.ExcluirProdutoAsync(id);
                await CarregarProdutos();
                return true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", 
                    $"Não foi possível excluir produto: {ex.Message}", 
                    "OK");
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}