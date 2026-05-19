using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppUTN2026_001.Data;
using MauiAppUTN2026_001.Models;

namespace MauiAppUTN2026_001.ViewModels
{
    [QueryProperty(nameof(ReservaId), "id")]
    public partial class ReservaFormViewModel : BaseViewModel
    {
        private readonly DatabaseService _db;

        [ObservableProperty]
        private int _reservaId;

        [ObservableProperty]
        private string _nombreHuesped = string.Empty;

        [ObservableProperty]
        private string _telefono = string.Empty;

        [ObservableProperty]
        private DateTime _fechaCheckIn = DateTime.Now;

        [ObservableProperty]
        private DateTime _fechaCheckOut = DateTime.Now.AddDays(1);

        [ObservableProperty]
        private int _selectedEstadoIndex;

        [ObservableProperty]
        private bool _isEditing;

        // ── Habitación (solo para creación) ──

        [ObservableProperty]
        private Habitacion? _selectedHabitacion;

        [ObservableProperty]
        private string _numeroNochesText = "1";

        [ObservableProperty]
        private double _subtotal;

        public List<string> Estados { get; } = new()
        {
            "Pendiente", "Confirmada", "EnCurso", "Completada", "Cancelada"
        };

        /// <summary>
        /// Catálogo de habitaciones predefinidas.
        /// </summary>
        public List<Habitacion> Habitaciones { get; } = Habitacion.ObtenerHabitaciones();

        public ReservaFormViewModel(DatabaseService db)
        {
            _db = db;
            Title = "Nueva Reserva";
        }

        partial void OnReservaIdChanged(int value)
        {
            if (value > 0)
            {
                IsEditing = true;
                Title = "Editar Reserva";
                LoadReservaCommand.ExecuteAsync(null);
            }
        }

        partial void OnFechaCheckInChanged(DateTime value) => AutoCalculateNoches();
        partial void OnFechaCheckOutChanged(DateTime value) => AutoCalculateNoches();

        /// <summary>
        /// Calcula automáticamente el número de noches según las fechas.
        /// </summary>
        private void AutoCalculateNoches()
        {
            var dias = (FechaCheckOut - FechaCheckIn).Days;
            if (dias > 0)
            {
                NumeroNochesText = dias.ToString();
            }
        }

        partial void OnSelectedHabitacionChanged(Habitacion? value) => CalculateSubtotal();
        partial void OnNumeroNochesTextChanged(string value) => CalculateSubtotal();

        private void CalculateSubtotal()
        {
            if (SelectedHabitacion != null &&
                int.TryParse(NumeroNochesText, out int noches) && noches > 0)
            {
                Subtotal = SelectedHabitacion.PrecioPorNoche * noches;
            }
            else
            {
                Subtotal = 0;
            }
        }

        [RelayCommand]
        public async Task LoadReservaAsync()
        {
            var reserva = await _db.GetReservaAsync(ReservaId);
            if (reserva != null)
            {
                NombreHuesped = reserva.NombreHuesped;
                Telefono = reserva.Telefono;
                FechaCheckIn = reserva.FechaCheckIn;
                FechaCheckOut = reserva.FechaCheckOut;
                SelectedEstadoIndex = Estados.IndexOf(reserva.Estado);
                if (SelectedEstadoIndex < 0) SelectedEstadoIndex = 0;

                // Cargar habitación existente si hay
                var detalles = await _db.GetDetallesAsync(ReservaId);
                if (detalles.Count > 0)
                {
                    var first = detalles[0];
                    SelectedHabitacion = Habitaciones.FirstOrDefault(
                        h => h.Numero == first.NumeroHabitacion) ?? Habitaciones.FirstOrDefault();
                    NumeroNochesText = first.NumeroNoches.ToString();
                }
            }
        }

        [RelayCommand]
        public async Task SaveAsync()
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(NombreHuesped))
            {
                await Shell.Current.DisplayAlert(
                    "Validación", "El nombre del huésped es requerido.", "OK");
                return;
            }

            if (FechaCheckOut <= FechaCheckIn)
            {
                await Shell.Current.DisplayAlert(
                    "Validación", "La fecha de check-out debe ser posterior al check-in.", "OK");
                return;
            }

            if (SelectedHabitacion == null)
            {
                await Shell.Current.DisplayAlert(
                    "Validación", "Seleccione una habitación.", "OK");
                return;
            }

            if (!int.TryParse(NumeroNochesText, out int noches) || noches <= 0)
            {
                await Shell.Current.DisplayAlert(
                    "Validación", "El número de noches debe ser mayor a 0.", "OK");
                return;
            }

            // Guardar reserva
            Reserva reserva;
            if (IsEditing)
            {
                reserva = await _db.GetReservaAsync(ReservaId) ?? new Reserva();
            }
            else
            {
                reserva = new Reserva();
            }

            reserva.NombreHuesped = NombreHuesped.Trim();
            reserva.Telefono = Telefono.Trim();
            reserva.FechaCheckIn = FechaCheckIn;
            reserva.FechaCheckOut = FechaCheckOut;
            reserva.Estado = Estados[SelectedEstadoIndex];

            await _db.SaveReservaAsync(reserva);

            // Guardar/actualizar la habitación como detalle
            var detallesExistentes = await _db.GetDetallesAsync(reserva.Id);

            DetalleReserva detalle;
            if (detallesExistentes.Count > 0)
            {
                // Actualizar el primer detalle existente
                detalle = detallesExistentes[0];
            }
            else
            {
                detalle = new DetalleReserva();
            }

            detalle.ReservaId = reserva.Id;
            detalle.NumeroHabitacion = SelectedHabitacion.Numero;
            detalle.TipoHabitacion = SelectedHabitacion.Tipo;
            detalle.PrecioPorNoche = SelectedHabitacion.PrecioPorNoche;
            detalle.NumeroNoches = noches;

            await _db.SaveDetalleAsync(detalle);
            await _db.RecalcularTotalAsync(reserva.Id);

            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
