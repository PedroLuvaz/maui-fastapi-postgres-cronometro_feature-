namespace LUFH_Cronometro
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            // Registrar rotas
            Routing.RegisterRoute("login", typeof(Views.Auth.LoginPage));
            Routing.RegisterRoute("clientes", typeof(Views.Visualizacao.ListaClientesPage));
            Routing.RegisterRoute("produtos", typeof(Views.Visualizacao.ListaProdutosPage));
            Routing.RegisterRoute("testes", typeof(Views.Visualizacao.ListaTestesPage));
        }
    }
}
