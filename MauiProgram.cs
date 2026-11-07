using Microsoft.Extensions.Logging;
using NotasApp.Services;
using NotasApp.ViewModels;
using NotasApp.Views;

namespace NotasApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            // Inicializa el proveedor SQLite nativo antes de crear NotasDatabase.
            // En Android, omitirlo puede provocar una JavaProxyThrowable al arrancar.
            SQLitePCL.Batteries_V2.Init();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<NotasDatabase>();
            builder.Services.AddSingleton<NotasViewModel>();
            builder.Services.AddSingleton<HistorialViewModel>();
            builder.Services.AddTransient<DetalleNotaViewModel>();
            builder.Services.AddSingleton<NotasPage>();
            builder.Services.AddSingleton<HistorialPage>();
            builder.Services.AddTransient<DetalleNotaPage>();
            builder.Services.AddSingleton<AppShell>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
