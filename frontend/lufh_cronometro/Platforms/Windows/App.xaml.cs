using Microsoft.UI.Xaml;

namespace MauiApp.WinUI
{
    public partial class App : MauiWinUIApplication
    {
        public App()
        {
            // Não precisa de InitializeComponent() aqui
        }

        protected override Microsoft.Maui.Hosting.MauiApp CreateMauiApp() 
            => MauiProgram.CreateMauiApp();
    }
}

