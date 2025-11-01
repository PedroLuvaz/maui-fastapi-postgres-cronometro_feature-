using MauiApp.Services;
using MauiApp.Models;

namespace MauiApp.Views.Visualizacao
{
    public partial class DetalhesMensuracaoPage : ContentPage
    {
        private readonly ApiService _apiService;
        private MensuracaoModel _mensuracao;  // ALTERADO

        public DetalhesMensuracaoPage(int mensuracaoId)
        {
            InitializeComponent();
            _apiService = new ApiService();
            CarregarMensuracao(mensuracaoId);
        }

        private async void CarregarMensuracao(int mensuracaoId)
        {
            try
            {
                _mensuracao = await _apiService.ObterMensuracaoAsync(mensuracaoId);

                // Header
                IdLabel.Text = $"ID: {_mensuracao.Id}";
                
                var statusInfo = _mensuracao.Status switch
                {
                    StatusMensuracao.Concluida => ("CONCLUÍDA", "#4CAF50", "#E8F5E9"),
                    StatusMensuracao.EmAndamento => ("EM ANDAMENTO", "#FF9800", "#FFF3E0"),
                    StatusMensuracao.Aguardando => ("AGUARDANDO", "#2196F3", "#E3F2FD"),
                    _ => ("DESCONHECIDO", "#999", "#F5F5F5")
                };

                StatusLabel.Text = statusInfo.Item1;
                StatusLabel.TextColor = Color.FromArgb(statusInfo.Item2);
                StatusFrame.BackgroundColor = Color.FromArgb(statusInfo.Item3);

                // Informações Gerais
                TesteLabel.Text = _mensuracao.Teste?.Nome ?? "N/A";
                VoluntarioLabel.Text = _mensuracao.Voluntario?.Nome ?? "N/A";
                DataInicioLabel.Text = _mensuracao.DataInicio?.ToString("dd/MM/yyyy HH:mm:ss") ?? "N/A";
                DataFimLabel.Text = _mensuracao.DataFim?.ToString("dd/MM/yyyy HH:mm:ss") ?? "N/A";

                var duracao = TimeSpan.FromSeconds(_mensuracao.TempoTotal);
                DuracaoLabel.Text = $"{duracao:hh\\:mm\\:ss}";

                // Estatísticas
                InterrupcoesLabel.Text = _mensuracao.Interrupcoes?.Count.ToString() ?? "0";
                FrustracoesLabel.Text = _mensuracao.Frustacoes?.Count.ToString() ?? "0";
                TarefasLabel.Text = "0/0"; // TODO

                // Interrupções Detalhadas
                if (_mensuracao.Interrupcoes?.Any() == true)
                {
                    InterrupcoesFrame.IsVisible = true;
                    foreach (var interrupcao in _mensuracao.Interrupcoes.OrderBy(i => i.Momento))
                    {
                        var frame = new Frame
                        {
                            Padding = 10,
                            CornerRadius = 5,
                            BackgroundColor = Color.FromArgb("#FFF3E0"),
                            HasShadow = false,
                            Margin = new Thickness(0, 5, 0, 0)
                        };

                        var stack = new VerticalStackLayout { Spacing = 3 };
                        stack.Add(new Label
                        {
                            Text = $"⏱️ {interrupcao.Momento:HH:mm:ss}",
                            FontSize = 12,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Color.FromArgb("#FF9800")
                        });
                        stack.Add(new Label
                        {
                            Text = interrupcao.Motivo,
                            FontSize = 13,
                            TextColor = Color.FromArgb("#333")
                        });

                        frame.Content = stack;
                        InterrupcoesContainer.Add(frame);
                    }
                }

                // Frustrações Detalhadas
                if (_mensuracao.Frustacoes?.Any() == true)
                {
                    FrustracoesFrame.IsVisible = true;
                    foreach (var frustracao in _mensuracao.Frustacoes.OrderBy(f => f.Momento))
                    {
                        var frame = new Frame
                        {
                            Padding = 10,
                            CornerRadius = 5,
                            BackgroundColor = Color.FromArgb("#FFEBEE"),
                            HasShadow = false,
                            Margin = new Thickness(0, 5, 0, 0)
                        };

                        var stack = new VerticalStackLayout { Spacing = 3 };
                        stack.Add(new Label
                        {
                            Text = $"😟 {frustracao.Momento:HH:mm:ss} - Nível {frustracao.Nivel}/5",
                            FontSize = 12,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Color.FromArgb("#F44336")
                        });
                        stack.Add(new Label
                        {
                            Text = frustracao.Descricao,
                            FontSize = 13,
                            TextColor = Color.FromArgb("#333")
                        });

                        frame.Content = stack;
                        FrustracoesContainer.Add(frame);
                    }
                }

                // Observações
                if (!string.IsNullOrWhiteSpace(_mensuracao.Observacoes))
                {
                    ObservacoesLabel.Text = _mensuracao.Observacoes;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível carregar a mensuração: {ex.Message}", 
                    "OK");
                await Navigation.PopAsync();
            }
        }
    }
}