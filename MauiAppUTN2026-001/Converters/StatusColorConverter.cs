using System.Globalization;

namespace MauiAppUTN2026_001.Converters
{
    /// <summary>
    /// Convierte el estado de la reserva a un color visual.
    /// </summary>
    public class StatusColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string estado)
            {
                return estado switch
                {
                    "Pendiente"  => Color.FromArgb("#F59E0B"),  // Amber
                    "Confirmada" => Color.FromArgb("#3B82F6"),  // Blue
                    "EnCurso"    => Color.FromArgb("#8B5CF6"),  // Purple
                    "Completada" => Color.FromArgb("#10B981"),  // Green
                    "Cancelada"  => Color.FromArgb("#EF4444"),  // Red
                    _            => Color.FromArgb("#6B7280"),  // Gray
                };
            }
            return Color.FromArgb("#6B7280");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convierte el estado a un texto amigable con ícono.
    /// </summary>
    public class StatusTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string estado)
            {
                return estado switch
                {
                    "Pendiente"  => "⏳ Pendiente",
                    "Confirmada" => "✅ Confirmada",
                    "EnCurso"    => "🏨 En Curso",
                    "Completada" => "✔️ Completada",
                    "Cancelada"  => "❌ Cancelada",
                    _            => estado,
                };
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convierte el estado a un color de fondo suave para badges.
    /// </summary>
    public class StatusBackgroundConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string estado)
            {
                return estado switch
                {
                    "Pendiente"  => Color.FromArgb("#FEF3C7"),  // Amber light
                    "Confirmada" => Color.FromArgb("#DBEAFE"),  // Blue light
                    "EnCurso"    => Color.FromArgb("#EDE9FE"),  // Purple light
                    "Completada" => Color.FromArgb("#D1FAE5"),  // Green light
                    "Cancelada"  => Color.FromArgb("#FEE2E2"),  // Red light
                    _            => Color.FromArgb("#F3F4F6"),  // Gray light
                };
            }
            return Color.FromArgb("#F3F4F6");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
