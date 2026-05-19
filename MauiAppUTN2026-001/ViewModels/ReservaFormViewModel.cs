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

        public List<string> Estados { get; } = new()
        {
            "Pendiente", "Confirmada", "EnCurso", "Completada", "Cancelada"
        };

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
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
