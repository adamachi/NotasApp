using NotasApp.Views;

namespace NotasApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DetalleNotaPage), typeof(DetalleNotaPage));
        }

        public void ConfigurePages(NotasPage notasPage, HistorialPage historialPage)
        {
            NotasShellContent.Content = notasPage;
            HistorialShellContent.Content = historialPage;
        }
    }
}
