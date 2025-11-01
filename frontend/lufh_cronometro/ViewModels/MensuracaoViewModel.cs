using System;
using System.Windows.Input;
using MauiApp.Services;
using MauiApp.Models;

namespace MauiApp.ViewModels
{
    public class MensuracaoViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private MensuracaoModel _mensuracaoAtual;  // ALTERADO
        private TimeSpan _tempoDecorrido;
        private bool _estaCronometrando;
        private DateTime? _inicioMensuracao;

        public MensuracaoViewModel()
        {
            _apiService = new ApiService();
            IniciarCommand = new Command(async () => await IniciarMensuracao());
            PausarCommand = new Command(async () => await PausarMensuracao());
            FinalizarCommand = new Command(async () => await FinalizarMensuracao());
        }

        public MensuracaoModel MensuracaoAtual  // ALTERADO
        {
            get => _mensuracaoAtual;
            set => SetProperty(ref _mensuracaoAtual, value);
        }

        public TimeSpan TempoDecorrido
        {
            get => _tempoDecorrido;
            set => SetProperty(ref _tempoDecorrido, value);
        }

        public bool EstaCronometrando
        {
            get => _estaCronometrando;
            set => SetProperty(ref _estaCronometrando, value);
        }

        public ICommand IniciarCommand { get; }
        public ICommand PausarCommand { get; }
        public ICommand FinalizarCommand { get; }

        private async Task IniciarMensuracao()
        {
            if (MensuracaoAtual != null)
            {
                await _apiService.IniciarMensuracaoAsync(MensuracaoAtual.Id);
                _inicioMensuracao = DateTime.Now;
                EstaCronometrando = true;
                IniciarCronometro();
            }
        }

        private async Task PausarMensuracao()
        {
            if (MensuracaoAtual != null)
            {
                await _apiService.PausarMensuracaoAsync(MensuracaoAtual.Id);
                EstaCronometrando = false;
            }
        }

        private async Task FinalizarMensuracao()
        {
            if (MensuracaoAtual != null)
            {
                int tempoTotal = (int)TempoDecorrido.TotalSeconds;
                await _apiService.FinalizarMensuracaoAsync(MensuracaoAtual.Id, tempoTotal);
                EstaCronometrando = false;
            }
        }

        private void IniciarCronometro()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (EstaCronometrando && _inicioMensuracao.HasValue)
                {
                    TempoDecorrido = DateTime.Now - _inicioMensuracao.Value;
                    return true;
                }
                return false;
            });
        }

        public async Task CarregarMensuracao(int mensuracaoId)  // ALTERADO
        {
            MensuracaoAtual = await _apiService.ObterMensuracaoAsync(mensuracaoId);
        }
    }
}