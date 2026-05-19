using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppUTN2026_001.Data;
using MauiAppUTN2026_001.Models;
using System.Collections.ObjectModel;

namespace MauiAppUTN2026_001.ViewModels
{
    [QueryProperty(nameof(ReservaId), "id")]
    public partial class ReservaDetalleViewModel : BaseViewModel
    {
        private readonly DatabaseService _db;

        [ObservableProperty]
        private int _reservaId;

        [ObservableProperty]
        private Reserva? _reserva;

        [ObservableProperty]
        private bool _hasDetalles;

        public ObservableCollection<DetalleReserva> Detalles { get; } = new();

        public ReservaDetalleViewModel(DatabaseService db)
        {
            _db = db;
            Title = "Detalle de Reserva";
        }

        partial void OnReservaIdChanged(int value)
        {
            if (value > 0)
                LoadDataCommand.ExecuteAsync(null);
        }

        [RelayCommand]
        public async Task LoadDataAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                Reserva = await _db.GetReservaAsync(ReservaId);

                var detalles = await _db.GetDetallesAsync(ReservaId);
                Detalles.Clear();
                foreach (var d in detalles)
                    Detalles.Add(d);

                HasDetalles = Detalles.Count > 0;

                if (Reserva != null)
                    Title = Reserva.CodigoReserva;
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task AddDetalleAsync()
        {
            await Shell.Current.GoToAsync(
                $"{nameof(Views.DetalleFormPage)}?reservaId={ReservaId}");
        }

        [RelayCommand]
        public async Task EditDetalleAsync(DetalleReserva detalle)
        {
            if (detalle == null) return;
            await Shell.Current.GoToAsync(
                $"{nameof(Views.DetalleFormPage)}?reservaId={ReservaId}&detalleId={detalle.Id}");
        }

        [RelayCommand]
        public async Task DeleteDetalleAsync(DetalleReserva detalle)
        {
            if (detalle == null) return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Eliminar Habitación",
                $"¿Eliminar la habitación {detalle.NumeroHabitacion}?",
                "Eliminar", "Cancelar");

            if (confirm)
            {
                await _db.DeleteDetalleAsync(detalle);
                await _db.RecalcularTotalAsync(ReservaId);
                await LoadDataAsync();
            }
        }

        [RelayCommand]
        public async Task EditReservaAsync()
        {
            await Shell.Current.GoToAsync(
                $"{nameof(Views.ReservaFormPage)}?id={ReservaId}");
        }

        [RelayCommand]
        public async Task ChangeEstadoAsync()
        {
            if (Reserva == null) return;

            var result = await Shell.Current.DisplayActionSheet(
                "Cambiar Estado", "Cancelar", null,
                "Pendiente", "Confirmada", "EnCurso", "Completada", "Cancelada");

            if (!string.IsNullOrEmpty(result) && result != "Cancelar")
            {
                Reserva.Estado = result;
                await _db.SaveReservaAsync(Reserva);
                await LoadDataAsync();
            }
        }
    }
}
