using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppUTN2026_001.Data;
using MauiAppUTN2026_001.Models;

namespace MauiAppUTN2026_001.ViewModels
{
    [QueryProperty(nameof(ReservaId), "reservaId")]
    [QueryProperty(nameof(DetalleId), "detalleId")]
    public partial class DetalleFormViewModel : BaseViewModel
    {
        private readonly DatabaseService _db;

        [ObservableProperty]
        private int _reservaId;

        [ObservableProperty]
        private int _detalleId;

        [ObservableProperty]
        private Habitacion? _selectedHabitacion;

        [ObservableProperty]
        private string _numeroNochesText = "1";

        [ObservableProperty]
        private double _subtotal;

        [ObservableProperty]
        private bool _isEditing;

        /// <summary>
        /// Catálogo de habitaciones predefinidas.
        /// </summary>
        public List<Habitacion> Habitaciones { get; } = Habitacion.ObtenerHabitaciones();

        public DetalleFormViewModel(DatabaseService db)
        {
            _db = db;
            Title = "Agregar Habitación";
        }

        partial void OnDetalleIdChanged(int value)
        {
            if (value > 0)
            {
                IsEditing = true;
                Title = "Editar Habitación";
                LoadDetalleCommand.ExecuteAsync(null);
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
        public async Task LoadDetalleAsync()
        {
            var detalle = await _db.GetDetalleAsync(DetalleId);
            if (detalle != null)
            {
                // Buscar la habitación predefinida que coincida
                SelectedHabitacion = Habitaciones.FirstOrDefault(
                    h => h.Numero == detalle.NumeroHabitacion) ?? Habitaciones.FirstOrDefault();
                NumeroNochesText = detalle.NumeroNoches.ToString();
                Subtotal = detalle.Subtotal;
            }
        }

        [RelayCommand]
        public async Task SaveAsync()
        {
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

            DetalleReserva detalle;
            if (IsEditing)
            {
                detalle = await _db.GetDetalleAsync(DetalleId) ?? new DetalleReserva();
            }
            else
            {
                detalle = new DetalleReserva();
            }

            detalle.ReservaId = ReservaId;
            detalle.NumeroHabitacion = SelectedHabitacion.Numero;
            detalle.TipoHabitacion = SelectedHabitacion.Tipo;
            detalle.PrecioPorNoche = SelectedHabitacion.PrecioPorNoche;
            detalle.NumeroNoches = noches;

            await _db.SaveDetalleAsync(detalle);
            await _db.RecalcularTotalAsync(ReservaId);
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
