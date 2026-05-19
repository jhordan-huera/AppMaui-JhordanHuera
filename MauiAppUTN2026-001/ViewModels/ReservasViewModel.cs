using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppUTN2026_001.Data;
using MauiAppUTN2026_001.Models;
using System.Collections.ObjectModel;

namespace MauiAppUTN2026_001.ViewModels
{
    public partial class ReservasViewModel : BaseViewModel
    {
        private readonly DatabaseService _db;

        public ObservableCollection<Reserva> Reservas { get; } = new();

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private bool _isEmpty;

        private List<Reserva> _allReservas = new();

        public ReservasViewModel(DatabaseService db)
        {
            _db = db;
            Title = "Mis Reservas";
        }

        [RelayCommand]
        public async Task LoadReservasAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                _allReservas = await _db.GetReservasAsync();
                FilterReservas();
            }
            finally
            {
                IsBusy = false;
            }
        }

        partial void OnSearchTextChanged(string value)
        {
            FilterReservas();
        }

        private void FilterReservas()
        {
            Reservas.Clear();

            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allReservas
                : _allReservas.Where(r =>
                    r.NombreHuesped.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    r.CodigoReserva.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            foreach (var r in filtered)
                Reservas.Add(r);

            IsEmpty = Reservas.Count == 0;
        }

        [RelayCommand]
        public async Task GoToNewReservaAsync()
        {
            await Shell.Current.GoToAsync(nameof(Views.ReservaFormPage));
        }

        [RelayCommand]
        public async Task GoToDetalleAsync(Reserva reserva)
        {
            if (reserva == null) return;
            await Shell.Current.GoToAsync(
                $"{nameof(Views.ReservaDetallePage)}?id={reserva.Id}");
        }

        [RelayCommand]
        public async Task DeleteReservaAsync(Reserva reserva)
        {
            if (reserva == null) return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Eliminar Reserva",
                $"¿Eliminar la reserva {reserva.CodigoReserva} de {reserva.NombreHuesped}?\n\nSe eliminarán también todos los detalles de habitación.",
                "Eliminar", "Cancelar");

            if (confirm)
            {
                await _db.DeleteReservaAsync(reserva);
                await LoadReservasAsync();
            }
        }
    }
}
