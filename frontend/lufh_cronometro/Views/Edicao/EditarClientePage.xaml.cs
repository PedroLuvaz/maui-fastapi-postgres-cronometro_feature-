using MauiApp.Services;
using MauiApp.Models;

namespace MauiApp.Views.Edicao
{
    public partial class EditarClientePage : ContentPage
    {
        private readonly ApiService _apiService;
        private Cliente _cliente;

        public EditarClientePage(int clienteId)
        {
            InitializeComponent();
            _apiService = new ApiService();
            CarregarCliente(clienteId);
        }

        private async void CarregarCliente(int clienteId)
        {
            try
            {
                _cliente = await _apiService.ObterClienteAsync(clienteId);

                IdLabel.Text = _cliente.Id.ToString();
                NomeEntry.Text = _cliente.Nome;
                EmailEntry.Text = _cliente.Email;
                TelefoneEntry.Text = _cliente.Telefone;
                EmpresaEntry.Text = _cliente.Empresa;
                AtivoSwitch.IsToggled = _cliente.Ativo;
                DataCriacaoLabel.Text = _cliente.DataCriacao.ToString("dd/MM/yyyy HH:mm");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível carregar o cliente: {ex.Message}", 
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

            try
            {
                _cliente.Nome = NomeEntry.Text.Trim();
                _cliente.Email = EmailEntry.Text?.Trim();
                _cliente.Telefone = TelefoneEntry.Text?.Trim();
                _cliente.Empresa = EmpresaEntry.Text?.Trim();
                _cliente.Ativo = AtivoSwitch.IsToggled;

                // TODO: Criar endpoint PUT no backend
                // await _apiService.AtualizarClienteAsync(_cliente);

                await DisplayAlert("Sucesso", 
                    "Cliente atualizado com sucesso!", 
                    "OK");

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível atualizar o cliente: {ex.Message}", 
                    "OK");
            }
        }

        private async void OnExcluirClicked(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert("Confirmar Exclusão", 
                $"Deseja realmente excluir o cliente '{_cliente.Nome}'?\n\nEsta ação não pode ser desfeita.", 
                "Sim, Excluir", "Cancelar");

            if (confirmar)
            {
                try
                {
                    // TODO: Criar endpoint DELETE no backend
                    // await _apiService.ExcluirClienteAsync(_cliente.Id);

                    await DisplayAlert("Sucesso", 
                        "Cliente excluído com sucesso!", 
                        "OK");

                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", 
                        $"Não foi possível excluir o cliente: {ex.Message}", 
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