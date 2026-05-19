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
        private string _numeroHabitacion = string.Empty;

        [ObservableProperty]
        private int _selectedTipoIndex;

        [ObservableProperty]
        private string _precioPorNocheText = string.Empty;

        [ObservableProperty]
        private string _numeroNochesText = "1";

        [ObservableProperty]
        private double _subtotal;

        [ObservableProperty]
        private bool _isEditing;

        public List<string> TiposHabitacion { get; } = new()
        {
            "Individual", "Doble", "Suite", "Familiar", "Deluxe"
        };

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

        partial void OnPrecioPorNocheTextChanged(string value) => CalculateSubtotal();
        partial void OnNumeroNochesTextChanged(string value) => CalculateSubtotal();

        private void CalculateSubtotal()
        {
            if (double.TryParse(PrecioPorNocheText, out double precio) &&
                int.TryParse(NumeroNochesText, out int noches))
            {
                Subtotal = precio * noches;
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
                NumeroHabitacion = detalle.NumeroHabitacion;
                SelectedTipoIndex = TiposHabitacion.IndexOf(detalle.TipoHabitacion);
                if (SelectedTipoIndex < 0) SelectedTipoIndex = 0;
                PrecioPorNocheText = detalle.PrecioPorNoche.ToString("F2");
                NumeroNochesText = detalle.NumeroNoches.ToString();
                Subtotal = detalle.Subtotal;
            }
        }

        [RelayCommand]
        public async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(NumeroHabitacion))
            {
                await Shell.Current.DisplayAlert(
                    "Validación", "El número de habitación es requerido.", "OK");
                return;
            }

            if (!double.TryParse(PrecioPorNocheText, out double precio) || precio <= 0)
            {
                await Shell.Current.DisplayAlert(
                    "Validación", "Ingrese un precio por noche válido.", "OK");
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
            detalle.NumeroHabitacion = NumeroHabitacion.Trim();
            detalle.TipoHabitacion = TiposHabitacion[SelectedTipoIndex];
            detalle.PrecioPorNoche = precio;
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
