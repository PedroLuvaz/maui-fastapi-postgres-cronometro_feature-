using LUFH_Cronometro.Models;
using LUFH_Cronometro.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LUFH_Cronometro.ViewModels
{
    public class ClienteViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private ObservableCollection<Cliente> _clientes;

        public ObservableCollection<Cliente> Clientes
        {
            get => _clientes;
            set => SetProperty(ref _clientes, value);
        }

        public ICommand CarregarClientesCommand { get; }

        public ClienteViewModel()
        {
            _apiService = new ApiService();
            Clientes = new ObservableCollection<Cliente>();
            CarregarClientesCommand = new Command(async () => await CarregarClientes());
            Title = "Clientes";
        }

        public async Task CarregarClientes()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                var clientes = await _apiService.GetAsync<Cliente>("/clientes");
                
                Clientes.Clear();
                foreach (var cliente in clientes)
                {
                    Clientes.Add(cliente);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", 
                    $"Não foi possível carregar clientes: {ex.Message}", 
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}