using MauiApp.Services;
using MauiApp.Models;
using System.Diagnostics;

namespace MauiApp.Views.Mensuracao
{
    public partial class CronometroPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly int _mensuracaoId;
        private readonly Teste _teste;
        private readonly Usuario _voluntario;

        private Stopwatch _stopwatch;
        private bool _rodando = false;
        private int _interrupcoes = 0;
        private int _frustracoes = 0;
        private int _tarefasConcluidas = 0;
        private int _totalTarefas = 0;

        public CronometroPage(int mensuracaoId, Teste teste, Usuario voluntario)
        {
            InitializeComponent();
            
            _apiService = new ApiService();
            _mensuracaoId = mensuracaoId;
            _teste = teste;
            _voluntario = voluntario;
            _stopwatch = new Stopwatch();

            ConfigurarTela();
            IniciarAtualizacaoTempo();
        }

        private void ConfigurarTela()
        {
            TesteNomeLabel.Text = _teste.Nome;
            VoluntarioNomeLabel.Text = $"👤 {_voluntario.Nome}";
            
            // TODO: Carregar total de tarefas do teste
            _totalTarefas = 5; // Exemplo
            AtualizarContadores();
        }

        private void IniciarAtualizacaoTempo()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                if (_rodando)
                {
                    var tempo = _stopwatch.Elapsed;
                    TempoLabel.Text = $"{tempo:hh\\:mm\\:ss}";
                }
                return true; // Continuar timer
            });
        }

        private async void OnIniciarPausarClicked(object sender, EventArgs e)
        {
            if (!_rodando)
            {
                // Iniciar
                _stopwatch.Start();
                _rodando = true;
                
                IniciarPausarButton.Text = "⏸️ PAUSAR";
                IniciarPausarButton.BackgroundColor = Color.FromArgb("#FF9800");
                StatusLabel.Text = "RODANDO";
                StatusLabel.TextColor = Color.FromArgb("#4CAF50");

                // Atualizar no backend
                await _apiService.IniciarMensuracaoAsync(_mensuracaoId);
            }
            else
            {
                // Pausar
                _stopwatch.Stop();
                _rodando = false;
                
                IniciarPausarButton.Text = "▶️ RETOMAR";
                IniciarPausarButton.BackgroundColor = Color.FromArgb("#4CAF50");
                StatusLabel.Text = "PAUSADO";
                StatusLabel.TextColor = Color.FromArgb("#FFC107");

                await _apiService.PausarMensuracaoAsync(_mensuracaoId);
            }
        }

        private async void OnInterrupcaoClicked(object sender, EventArgs e)
        {
            string motivo = await DisplayPromptAsync("Interrupção", 
                "Digite o motivo da interrupção:", 
                "OK", "Cancelar", 
                placeholder: "Ex: Dúvida do usuário");

            if (!string.IsNullOrWhiteSpace(motivo))
            {
                try
                {
                    var interrupcao = new Interrupcao
                    {
                        MensuracaoId = _mensuracaoId,
                        Momento = DateTime.Now,
                        Motivo = motivo,
                        Duracao = 0 // Será calculado depois
                    };

                    await _apiService.CriarInterrupcaoAsync(interrupcao);
                    
                    _interrupcoes++;
                    AtualizarContadores();

                    await DisplayAlert("Registrado", "Interrupção registrada com sucesso.", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", $"Não foi possível registrar: {ex.Message}", "OK");
                }
            }
        }

        private async void OnFrustracaoClicked(object sender, EventArgs e)
        {
            string descricao = await DisplayPromptAsync("Frustração", 
                "Descreva a frustração observada:", 
                "OK", "Cancelar", 
                placeholder: "Ex: Não encontrou o botão");

            if (!string.IsNullOrWhiteSpace(descricao))
            {
                string nivelStr = await DisplayActionSheet("Nível de Frustração", 
                    "Cancelar", null, 
                    "1 - Muito Leve", "2 - Leve", "3 - Moderado", "4 - Alto", "5 - Muito Alto");

                if (!string.IsNullOrEmpty(nivelStr) && nivelStr != "Cancelar")
                {
                    int nivel = int.Parse(nivelStr.Substring(0, 1));

                    try
                    {
                        var frustacao = new Frustacao
                        {
                            MensuracaoId = _mensuracaoId,
                            Momento = DateTime.Now,
                            Descricao = descricao,
                            Nivel = nivel
                        };

                        await _apiService.CriarFrustacaoAsync(frustacao);
                        
                        _frustracoes++;
                        AtualizarContadores();

                        await DisplayAlert("Registrado", "Frustração registrada com sucesso.", "OK");
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Erro", $"Não foi possível registrar: {ex.Message}", "OK");
                    }
                }
            }
        }

        private async void OnFinalizarClicked(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert("Finalizar Teste", 
                $"Deseja finalizar o teste?\n\nTempo total: {_stopwatch.Elapsed:hh\\:mm\\:ss}", 
                "Sim, Finalizar", "Cancelar");

            if (confirmar)
            {
                try
                {
                    _stopwatch.Stop();
                    _rodando = false;

                    int tempoTotalSegundos = (int)_stopwatch.Elapsed.TotalSeconds;

                    await _apiService.FinalizarMensuracaoAsync(_mensuracaoId, tempoTotalSegundos);

                    await DisplayAlert("Sucesso", 
                        "Teste finalizado com sucesso!", 
                        "OK");

                    // Navegar para resultados
                    await Navigation.PushAsync(new ResultadoPage(_mensuracaoId));
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", 
                        $"Não foi possível finalizar: {ex.Message}", 
                        "OK");
                }
            }
        }

        private void AtualizarContadores()
        {
            InterrupcoesLabel.Text = _interrupcoes.ToString();
            FrustracoesLabel.Text = _frustracoes.ToString();
            TarefasLabel.Text = $"{_tarefasConcluidas}/{_totalTarefas}";
        }

        protected override bool OnBackButtonPressed()
        {
            // Impedir voltar acidentalmente
            Device.BeginInvokeOnMainThread(async () =>
            {
                bool sair = await DisplayAlert("Atenção", 
                    "O teste está em andamento. Deseja realmente sair?", 
                    "Sim", "Não");

                if (sair)
                {
                    await Navigation.PopAsync();
                }
            });

            return true; // Bloquear comportamento padrão
        }
    }
}