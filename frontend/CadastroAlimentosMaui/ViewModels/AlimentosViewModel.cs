using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Input;
using CadastroAlimentosMaui.Models;

namespace CadastroAlimentosMaui.ViewModels;

public class AlimentosViewModel : INotifyPropertyChanged
{
    private readonly HttpClient _http;

    public ObservableCollection<Alimento> Alimentos { get; set; } = new();

    private string _nome = "";
    private double _calorias;

    public string Nome { get => _nome; set { _nome = value; OnPropertyChanged(); } }
    public double Calorias { get => _calorias; set { _calorias = value; OnPropertyChanged(); } }

    public ICommand SalvarCommand { get; }
    public ICommand ExcluirCommand { get; }

    public AlimentosViewModel()
    {
        // Ajuste a BaseAddress conforme o ambiente:
        // - Rodando no PC -> "http://localhost:8000"
        // - Android Emulator -> "http://10.0.2.2:8000"
        _http = new HttpClient { BaseAddress = new Uri("http://127.0.0.1:8000/") };
        // _http = new HttpClient { BaseAddress = new Uri("http://10.0.2.2:8000") };

        SalvarCommand = new Command(async () => await SalvarAlimento());
        ExcluirCommand = new Command<Alimento>(async (a) => await ExcluirAlimento(a));

        Task.Run(async () => await CarregarAlimentos());
    }

    public async Task CarregarAlimentos()
    {
        try
        {
            var lista = await _http.GetFromJsonAsync<List<Alimento>>("/alimentos");
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Alimentos.Clear();
                if (lista != null)
                    foreach (var item in lista)
                        Alimentos.Add(item);
            });
        }
        catch (Exception ex)
        {
            // tratar/loggar erro
        }
    }

    public async Task SalvarAlimento()
    {
        if (string.IsNullOrWhiteSpace(Nome) || Calorias <= 0) return;
        var novo = new Alimento { Nome = Nome, Calorias = Calorias };
        await _http.PostAsJsonAsync("/alimentos", novo);
        await CarregarAlimentos();
        Nome = ""; Calorias = 0;
    }

    public async Task ExcluirAlimento(Alimento alimento)
    {
        if (alimento == null) return;
        await _http.DeleteAsync($"/alimentos/{alimento.Id}");
        await CarregarAlimentos();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
