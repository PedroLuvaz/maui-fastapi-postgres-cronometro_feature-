using MauiApp.Models;
using MauiApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiApp.ViewModels
{
    public class UsuarioViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private ObservableCollection<Usuario> _usuarios;
        private Usuario _usuarioSelecionado;
        private bool _isRefreshing;

        public ObservableCollection<Usuario> Usuarios
        {
            get => _usuarios;
            set => SetProperty(ref _usuarios, value);
        }

        public Usuario UsuarioSelecionado
        {
            get => _usuarioSelecionado;
            set => SetProperty(ref _usuarioSelecionado, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public ICommand CarregarUsuariosCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand SelecionarUsuarioCommand { get; }

        public UsuarioViewModel()
        {
            _apiService = new ApiService();
            Usuarios = new ObservableCollection<Usuario>();

            CarregarUsuariosCommand = new Command(async () => await CarregarUsuarios());
            RefreshCommand = new Command(async () => await RefreshUsuarios());
            SelecionarUsuarioCommand = new Command<Usuario>(OnUsuarioSelecionado);

            Title = "Usuários";
        }

        public async Task CarregarUsuarios()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var usuarios = await _apiService.ListarUsuariosAsync();
                
                Usuarios.Clear();
                foreach (var usuario in usuarios)
                {
                    Usuarios.Add(usuario);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", 
                    $"Não foi possível carregar usuários: {ex.Message}", 
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RefreshUsuarios()
        {
            IsRefreshing = true;
            await CarregarUsuarios();
            IsRefreshing = false;
        }

        private void OnUsuarioSelecionado(Usuario usuario)
        {
            if (usuario == null)
                return;

            UsuarioSelecionado = usuario;
        }

        public async Task<List<Usuario>> ObterVoluntarios()
        {
            try
            {
                var todosUsuarios = await _apiService.ListarUsuariosAsync();
                return todosUsuarios
                    .Where(u => u.TipoUsuario == TipoUsuario.Voluntario && u.Ativo)
                    .ToList();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", 
                    $"Não foi possível obter voluntários: {ex.Message}", 
                    "OK");
                return new List<Usuario>();
            }
        }

        public async Task<List<Usuario>> ObterCoordenadores()
        {
            try
            {
                var todosUsuarios = await _apiService.ListarUsuariosAsync();
                return todosUsuarios
                    .Where(u => (u.TipoUsuario == TipoUsuario.Coordenador || 
                                 u.TipoUsuario == TipoUsuario.Admin) && u.Ativo)
                    .ToList();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", 
                    $"Não foi possível obter coordenadores: {ex.Message}", 
                    "OK");
                return new List<Usuario>();
            }
        }

        public async Task<bool> CriarUsuario(Usuario usuario)
        {
            try
            {
                IsBusy = true;
                await _apiService.CriarUsuarioAsync(usuario);
                await CarregarUsuarios();
                return true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", 
                    $"Não foi possível criar usuário: {ex.Message}", 
                    "OK");
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task<bool> AtualizarUsuario(Usuario usuario)
        {
            try
            {
                IsBusy = true;
                // TODO: Implementar endpoint PUT no backend
                // await _apiService.AtualizarUsuarioAsync(usuario);
                await CarregarUsuarios();
                return true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", 
                    $"Não foi possível atualizar usuário: {ex.Message}", 
                    "OK");
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task<bool> ExcluirUsuario(int id)
        {
            try
            {
                IsBusy = true;
                // TODO: Implementar endpoint DELETE no backend
                // await _apiService.ExcluirUsuarioAsync(id);
                await CarregarUsuarios();
                return true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", 
                    $"Não foi possível excluir usuário: {ex.Message}", 
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