using LUFH_Cronometro.Models;
using LUFH_Cronometro.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LUFH_Cronometro.ViewModels
{
    public class TesteViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private ObservableCollection<Teste> _testes;

        public ObservableCollection<Teste> Testes
        {
            get => _testes;
            set => SetProperty(ref _testes, value);
        }

        public ICommand CarregarTestesCommand { get; }

        public TesteViewModel()
        {
            _apiService = new ApiService();
            Testes = new ObservableCollection<Teste>();
            CarregarTestesCommand = new Command(async () => await CarregarTestes());
            Title = "Testes";
        }

        public async Task CarregarTestes()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                var testes = await _apiService.GetAsync<Teste>("/testes");
                
                Testes.Clear();
                foreach (var teste in testes)
                {
                    Testes.Add(teste);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", 
                    $"Não foi possível carregar testes: {ex.Message}", 
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}