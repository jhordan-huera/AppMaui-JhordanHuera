using SQLite;

namespace MauiAppUTN2026_001.Models
{
    [Table("Reservas")]
    public class Reserva
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string CodigoReserva { get; set; } = string.Empty;

        public string NombreHuesped { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public DateTime FechaCheckIn { get; set; } = DateTime.Now;

        public DateTime FechaCheckOut { get; set; } = DateTime.Now.AddDays(1);

        /// <summary>
        /// Pendiente, Confirmada, EnCurso, Completada, Cancelada
        /// </summary>
        public string Estado { get; set; } = "Pendiente";

        public int NumeroHabitaciones { get; set; }

        public double Total { get; set; }

        [Ignore]
        public int DiasEstancia => (FechaCheckOut - FechaCheckIn).Days;
    }
}
