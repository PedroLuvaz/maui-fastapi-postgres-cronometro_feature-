using MauiApp.Services;
using MauiApp.Models;

namespace MauiApp.Views.Visualizacao
{
    public partial class ListaMensuracoesPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly int _testeId;
        private List<MensuracaoModel> _todasMensuracoes;  // ALTERADO

        public ListaMensuracoesPage(int testeId)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _testeId = testeId;
            CarregarMensuracoes();
        }

        private async void CarregarMensuracoes()
        {
            try
            {
                var teste = await _apiService.ObterTesteAsync(_testeId);
                TesteNomeLabel.Text = $"Teste: {teste.Nome}";

                // TODO: Criar endpoint para listar mensurações por teste
                _todasMensuracoes = new List<MensuracaoModel>(); // await _apiService.ListarMensuracoesDoTesteAsync(_testeId);

                AtualizarLista();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", 
                    $"Não foi possível carregar as mensurações: {ex.Message}", 
                    "OK");
            }
        }

        private void OnFiltroChanged(object sender, EventArgs e)
        {
            AtualizarLista();
        }

        private void AtualizarLista()
        {
            ListaContainer.Children.Clear();

            if (_todasMensuracoes == null || !_todasMensuracoes.Any())
            {
                EmptyFrame.IsVisible = true;
                return;
            }

            EmptyFrame.IsVisible = false;

            // Filtrar por status
            var mensuracoesFiltradas = _todasMensuracoes;
            var filtroSelecionado = FiltroStatusPicker.SelectedItem?.ToString();

            if (filtroSelecionado != "Todas")
            {
                mensuracoesFiltradas = filtroSelecionado switch
                {
                    "Concluídas" => _todasMensuracoes.Where(m => m.Status == StatusMensuracao.Concluida).ToList(),
                    "Em Andamento" => _todasMensuracoes.Where(m => m.Status == StatusMensuracao.EmAndamento).ToList(),
                    "Aguardando" => _todasMensuracoes.Where(m => m.Status == StatusMensuracao.Aguardando).ToList(),
                    _ => _todasMensuracoes
                };
            }

            // Ordenar por data (mais recentes primeiro)
            mensuracoesFiltradas = mensuracoesFiltradas
                .OrderByDescending(m => m.DataInicio)
                .ToList();

            // Criar cards
            foreach (var mensuracao in mensuracoesFiltradas)
            {
                var card = CriarCardMensuracao(mensuracao);
                ListaContainer.Children.Add(card);
            }

            if (!mensuracoesFiltradas.Any())
            {
                EmptyFrame.IsVisible = true;
            }
        }

        private Frame CriarCardMensuracao(MensuracaoModel mensuracao)  // ALTERADO
        {
            var statusColor = mensuracao.Status switch
            {
                StatusMensuracao.Concluida => "#4CAF50",
                StatusMensuracao.EmAndamento => "#FF9800",
                StatusMensuracao.Aguardando => "#2196F3",
                _ => "#999"
            };

            var statusTexto = mensuracao.Status switch
            {
                StatusMensuracao.Concluida => "CONCLUÍDA",
                StatusMensuracao.EmAndamento => "EM ANDAMENTO",
                StatusMensuracao.Aguardando => "AGUARDANDO",
                _ => "DESCONHECIDO"
            };

            var frame = new Frame
            {
                Padding = 15,
                CornerRadius = 10,
                BackgroundColor = Colors.White,
                HasShadow = true
            };

            var grid = new Grid
            {
                ColumnDefinitions = 
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = GridLength.Auto }
                },
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                },
                RowSpacing = 8
            };

            // Voluntário
            var voluntarioLabel = new Label
            {
                Text = $"👤 {mensuracao.Voluntario?.Nome ?? "N/A"}",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#333")
            };
            grid.Add(voluntarioLabel, 0, 0);

            // Status Badge
            var statusFrame = new Frame
            {
                Padding = new Thickness(8, 4),
                CornerRadius = 4,
                BackgroundColor = Color.FromArgb(statusColor),
                HasShadow = false,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start
            };
            statusFrame.Content = new Label
            {
                Text = statusTexto,
                FontSize = 10,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White
            };
            grid.Add(statusFrame, 1, 0);

            // Data/Hora
            var dataLabel = new Label
            {
                Text = $"📅 {mensuracao.DataInicio?.ToString("dd/MM/yyyy HH:mm") ?? "N/A"}",
                FontSize = 13,
                TextColor = Color.FromArgb("#666")
            };
            grid.Add(dataLabel, 0, 1);
            Grid.SetColumnSpan(dataLabel, 2);

            // Tempo Total
            if (mensuracao.Status == StatusMensuracao.Concluida)
            {
                var duracao = TimeSpan.FromSeconds(mensuracao.TempoTotal);
                var tempoLabel = new Label
                {
                    Text = $"⏱️ Duração: {duracao:hh\\:mm\\:ss}",
                    FontSize = 13,
                    TextColor = Color.FromArgb("#666")
                };
                grid.Add(tempoLabel, 0, 2);
                Grid.SetColumnSpan(tempoLabel, 2);
            }

            frame.Content = grid;

            // Gesture para abrir detalhes
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) =>
            {
                await Navigation.PushAsync(new DetalhesMensuracaoPage(mensuracao.Id));
            };
            frame.GestureRecognizers.Add(tapGesture);

            return frame;
        }
    }
}