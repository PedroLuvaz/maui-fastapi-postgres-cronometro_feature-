using MauiApp.Views.Auth;

namespace LUFH_Cronometro
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Iniciar direto no Menu Principal
            MainPage = new AppShell();
        }
    }
}