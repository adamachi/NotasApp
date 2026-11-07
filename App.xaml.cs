using Microsoft.Extensions.DependencyInjection;

namespace NotasApp
{
    public partial class App : Application
    {
        private readonly AppShell _appShell;

        public App(AppShell appShell, IServiceProvider services)
        {
            InitializeComponent();
            _appShell = appShell;

            // Los recursos de App.xaml deben estar cargados antes de crear las
            // páginas, porque sus XAML usan StaticResource definidos en Colors.xaml.
            _appShell.ConfigurePages(
                services.GetRequiredService<NotasApp.Views.NotasPage>(),
                services.GetRequiredService<NotasApp.Views.HistorialPage>());
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(_appShell);
        }
    }
}