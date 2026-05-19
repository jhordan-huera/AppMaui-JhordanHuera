namespace MauiAppUTN2026_001.Models
{
    /// <summary>
    /// Catálogo de habitaciones predefinidas del hotel.
    /// </summary>
    public class Habitacion
    {
        public string Numero { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public double PrecioPorNoche { get; set; }

        public string Display => $"Hab. {Numero} — {Tipo} (${PrecioPorNoche:N2}/noche)";

        /// <summary>
        /// Habitaciones disponibles en el hotel.
        /// </summary>
        public static List<Habitacion> ObtenerHabitaciones() => new()
        {
            // Individuales
            new() { Numero = "101", Tipo = "Individual", PrecioPorNoche = 35.00 },
            new() { Numero = "102", Tipo = "Individual", PrecioPorNoche = 35.00 },
            new() { Numero = "103", Tipo = "Individual", PrecioPorNoche = 40.00 },

            // Dobles
            new() { Numero = "201", Tipo = "Doble", PrecioPorNoche = 60.00 },
            new() { Numero = "202", Tipo = "Doble", PrecioPorNoche = 60.00 },
            new() { Numero = "203", Tipo = "Doble", PrecioPorNoche = 65.00 },

            // Suites
            new() { Numero = "301", Tipo = "Suite", PrecioPorNoche = 120.00 },
            new() { Numero = "302", Tipo = "Suite", PrecioPorNoche = 130.00 },

            // Familiares
            new() { Numero = "401", Tipo = "Familiar", PrecioPorNoche = 90.00 },
            new() { Numero = "402", Tipo = "Familiar", PrecioPorNoche = 95.00 },

            // Deluxe
            new() { Numero = "501", Tipo = "Deluxe", PrecioPorNoche = 180.00 },
            new() { Numero = "502", Tipo = "Deluxe", PrecioPorNoche = 200.00 },
        };
    }
}
