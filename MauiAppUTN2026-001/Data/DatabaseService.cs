using SQLite;
using MauiAppUTN2026_001.Models;

namespace MauiAppUTN2026_001.Data
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;
        private readonly string _dbPath;

        public DatabaseService()
        {
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, "reservas.db3");
        }

        private async Task InitAsync()
        {
            if (_database != null)
                return;

            _database = new SQLiteAsyncConnection(_dbPath);
            await _database.CreateTableAsync<Reserva>();
            await _database.CreateTableAsync<DetalleReserva>();
        }

        // ───── Reservas CRUD ─────

        public async Task<List<Reserva>> GetReservasAsync()
        {
            await InitAsync();
            return await _database!.Table<Reserva>()
                .OrderByDescending(r => r.FechaCheckIn)
                .ToListAsync();
        }

        public async Task<Reserva?> GetReservaAsync(int id)
        {
            await InitAsync();
            return await _database!.Table<Reserva>()
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> SaveReservaAsync(Reserva reserva)
        {
            await InitAsync();
            if (reserva.Id != 0)
            {
                return await _database!.UpdateAsync(reserva);
            }
            else
            {
                var count = await _database!.Table<Reserva>().CountAsync();
                reserva.CodigoReserva = $"RES-{(count + 1):D4}";
                return await _database!.InsertAsync(reserva);
            }
        }

        public async Task<int> DeleteReservaAsync(Reserva reserva)
        {
            await InitAsync();
            // Eliminar detalles primero (cascada manual)
            await _database!.ExecuteAsync(
                "DELETE FROM DetallesReserva WHERE ReservaId = ?", reserva.Id);
            return await _database!.DeleteAsync(reserva);
        }

        // ───── Detalles CRUD ─────

        public async Task<List<DetalleReserva>> GetDetallesAsync(int reservaId)
        {
            await InitAsync();
            return await _database!.Table<DetalleReserva>()
                .Where(d => d.ReservaId == reservaId)
                .ToListAsync();
        }

        public async Task<DetalleReserva?> GetDetalleAsync(int id)
        {
            await InitAsync();
            return await _database!.Table<DetalleReserva>()
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> SaveDetalleAsync(DetalleReserva detalle)
        {
            await InitAsync();
            detalle.Subtotal = detalle.PrecioPorNoche * detalle.NumeroNoches;

            if (detalle.Id != 0)
                return await _database!.UpdateAsync(detalle);
            else
                return await _database!.InsertAsync(detalle);
        }

        public async Task<int> DeleteDetalleAsync(DetalleReserva detalle)
        {
            await InitAsync();
            return await _database!.DeleteAsync(detalle);
        }

        public async Task RecalcularTotalAsync(int reservaId)
        {
            await InitAsync();
            var detalles = await GetDetallesAsync(reservaId);
            var total = detalles.Sum(d => d.Subtotal);
            var reserva = await GetReservaAsync(reservaId);
            if (reserva != null)
            {
                reserva.Total = total;
                reserva.NumeroHabitaciones = detalles.Count;
                await _database!.UpdateAsync(reserva);
            }
        }

        // ───── Estadísticas ─────

        public async Task<int> GetTotalReservasAsync()
        {
            await InitAsync();
            return await _database!.Table<Reserva>().CountAsync();
        }

        public async Task<int> GetReservasByEstadoAsync(string estado)
        {
            await InitAsync();
            return await _database!.Table<Reserva>()
                .Where(r => r.Estado == estado)
                .CountAsync();
        }
    }
}
