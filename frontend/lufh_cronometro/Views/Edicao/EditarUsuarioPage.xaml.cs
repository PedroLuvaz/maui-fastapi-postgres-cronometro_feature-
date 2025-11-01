using MauiApp.Services;
using MauiApp.Models;

namespace MauiApp.Views.Edicao
{
    public partial class EditarUsuarioPage : ContentPage
    {
        private readonly ApiService _apiService;
        private Usuario _usuario;

        public EditarUsuarioPage(int usuarioId)
        {
            InitializeComponent();
            _apiService = new ApiService();
            CarregarUsuario(usuarioId);
        }

        private async void CarregarUsuario(int usuarioId)
        {
            try
            {
                _usuario = await _apiService.ObterUsuarioAsync(usuarioId);

                IdLabel.Text = _usuario.Id.ToString();
                NomeEntry.Text = _usuario.Nome;
                EmailEntry.Text = _usuario.Email;
                TipoUsuarioPicker.SelectedItem = _usuario.TipoUsuario.ToString();
                AtivoSwitch.IsToggled = _usuario.Ativo;
                DataCriacaoLabel.Text = _usuario.DataCriacao.ToString("dd/MM/yyyy HH:mm");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível carregar o usuário: {ex.Message}", 
                    "OK");
                await Navigation.PopAsync();
            }
        }

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NomeEntry.Text))
            {
                await DisplayAlert("Erro", "O nome é obrigatório.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                await DisplayAlert("Erro", "O e-mail é obrigatório.", "OK");
                return;
            }

            if (TipoUsuarioPicker.SelectedIndex < 0)
            {
                await DisplayAlert("Erro", "Selecione o tipo de usuário.", "OK");
                return;
            }

            try
            {
                var tipoUsuario = TipoUsuarioPicker.SelectedItem.ToString() switch
                {
                    "Admin" => TipoUsuario.Admin,
                    "Tecnico" => TipoUsuario.Tecnico,
                    "Coordenador" => TipoUsuario.Coordenador,
                    "Voluntario" => TipoUsuario.Voluntario,
                    _ => TipoUsuario.Voluntario
                };

                _usuario.Nome = NomeEntry.Text.Trim();
                _usuario.Email = EmailEntry.Text.Trim();
                _usuario.TipoUsuario = tipoUsuario;
                _usuario.Ativo = AtivoSwitch.IsToggled;

                // Se nova senha foi fornecida
                if (!string.IsNullOrWhiteSpace(NovaSenhaEntry.Text))
                {
                    _usuario.Senha = NovaSenhaEntry.Text;
                }

                // TODO: Criar endpoint PUT no backend
                // await _apiService.AtualizarUsuarioAsync(_usuario);

                await DisplayAlert("Sucesso", 
                    "Usuário atualizado com sucesso!", 
                    "OK");

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível atualizar o usuário: {ex.Message}", 
                    "OK");
            }
        }

        private async void OnExcluirClicked(object sender, EventArgs e)
        {
            // Verificar se não é o próprio usuário logado
            var userIdLogado = await SecureStorage.GetAsync("user_id");
            if (_usuario.Id.ToString() == userIdLogado)
            {
                await DisplayAlert("Atenção", 
                    "Você não pode excluir seu próprio usuário.", 
                    "OK");
                return;
            }

            bool confirmar = await DisplayAlert("Confirmar Exclusão", 
                $"Deseja realmente excluir o usuário '{_usuario.Nome}'?\n\nEsta ação não pode ser desfeita.", 
                "Sim, Excluir", "Cancelar");

            if (confirmar)
            {
                try
                {
                    // TODO: Criar endpoint DELETE no backend
                    // await _apiService.ExcluirUsuarioAsync(_usuario.Id);

                    await DisplayAlert("Sucesso", 
                        "Usuário excluído com sucesso!", 
                        "OK");

                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", 
                        $"Não foi possível excluir o usuário: {ex.Message}", 
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