using Microsoft.Extensions.Logging;
using MauiApp.Services;
using MauiApp.Views.Auth;
using MauiApp.Views.Menu;
using MauiApp.Views.Cadastro;
using MauiApp.Views.Edicao;
using MauiApp.Views.Visualizacao;
using MauiApp.Views.Mensuracao;

namespace MauiApp
{
    public static class MauiProgram
    {
        public static Microsoft.Maui.Hosting.MauiApp CreateMauiApp()
        {
            var builder = Microsoft.Maui.Hosting.MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
