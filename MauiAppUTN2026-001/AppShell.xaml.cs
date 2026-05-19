using MauiAppUTN2026_001.Views;

namespace MauiAppUTN2026_001
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registrar rutas de navegación
            Routing.RegisterRoute(nameof(ReservaDetallePage), typeof(ReservaDetallePage));
            Routing.RegisterRoute(nameof(ReservaFormPage), typeof(ReservaFormPage));
            Routing.RegisterRoute(nameof(DetalleFormPage), typeof(DetalleFormPage));
        }
    }
}
