using Microsoft.Extensions.Logging;
using MauiAppUTN2026_001.Data;
using MauiAppUTN2026_001.ViewModels;
using MauiAppUTN2026_001.Views;

namespace MauiAppUTN2026_001
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Servicios
            builder.Services.AddSingleton<DatabaseService>();

            // ViewModels
            builder.Services.AddTransient<InicioViewModel>();
            builder.Services.AddTransient<ReservasViewModel>();
            builder.Services.AddTransient<ReservaDetalleViewModel>();
            builder.Services.AddTransient<ReservaFormViewModel>();
            builder.Services.AddTransient<DetalleFormViewModel>();

            // Views
            builder.Services.AddTransient<InicioPage>();
            builder.Services.AddTransient<ReservasPage>();
            builder.Services.AddTransient<ReservaDetallePage>();
            builder.Services.AddTransient<ReservaFormPage>();
            builder.Services.AddTransient<DetalleFormPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
