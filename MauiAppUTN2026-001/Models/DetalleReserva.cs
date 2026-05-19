using SQLite;

namespace MauiAppUTN2026_001.Models
{
    [Table("DetallesReserva")]
    public class DetalleReserva
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int ReservaId { get; set; }

        public string NumeroHabitacion { get; set; } = string.Empty;

        /// <summary>
        /// Individual, Doble, Suite, Familiar, Deluxe
        /// </summary>
        public string TipoHabitacion { get; set; } = "Individual";

        public double PrecioPorNoche { get; set; }

        public int NumeroNoches { get; set; } = 1;

        public double Subtotal { get; set; }

        [Ignore]
        public string DisplayHabitacion => $"Hab. {NumeroHabitacion} - {TipoHabitacion}";

        [Ignore]
        public string DisplayPrecio => $"${PrecioPorNoche:N2}/noche x {NumeroNoches} noches";
    }
}
