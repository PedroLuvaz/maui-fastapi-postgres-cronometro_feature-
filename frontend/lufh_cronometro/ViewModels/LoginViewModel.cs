using LUFH_Cronometro.Services;
using System.Windows.Input;

namespace LUFH_Cronometro.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private string _email = string.Empty;
        private string _senha = string.Empty;
        private string _errorMessage = string.Empty;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Senha
        {
            get => _senha;
            set => SetProperty(ref _senha, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _apiService = new ApiService();
            LoginCommand = new Command(async () => await LoginAsync(), () => !IsBusy);
        }

        private async Task LoginAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Senha))
                {
                    ErrorMessage = "Por favor, preencha todos os campos";
                    return;
                }

                var success = await _apiService.LoginAsync(Email, Senha);

                if (success)
                {
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    ErrorMessage = "Email ou senha inválidos";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro ao fazer login: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
                ((Command)LoginCommand).ChangeCanExecute();
            }
        }
    }
}