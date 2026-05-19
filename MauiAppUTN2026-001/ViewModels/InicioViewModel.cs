using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppUTN2026_001.Data;

namespace MauiAppUTN2026_001.ViewModels
{
    public partial class InicioViewModel : BaseViewModel
    {
        private readonly DatabaseService _db;

        [ObservableProperty]
        private int _totalReservas;

        [ObservableProperty]
        private int _pendientes;

        [ObservableProperty]
        private int _confirmadas;

        [ObservableProperty]
        private int _completadas;

        [ObservableProperty]
        private int _canceladas;

        [ObservableProperty]
        private int _enCurso;

        public InicioViewModel(DatabaseService db)
        {
            _db = db;
            Title = "Inicio";
        }

        [RelayCommand]
        public async Task LoadStatsAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                TotalReservas = await _db.GetTotalReservasAsync();
                Pendientes = await _db.GetReservasByEstadoAsync("Pendiente");
                Confirmadas = await _db.GetReservasByEstadoAsync("Confirmada");
                EnCurso = await _db.GetReservasByEstadoAsync("EnCurso");
                Completadas = await _db.GetReservasByEstadoAsync("Completada");
                Canceladas = await _db.GetReservasByEstadoAsync("Cancelada");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task GoToReservasAsync()
        {
            await Shell.Current.GoToAsync("//ReservasPage");
        }

        [RelayCommand]
        public async Task GoToNuevaReservaAsync()
        {
            await Shell.Current.GoToAsync(nameof(Views.ReservaFormPage));
        }
    }
}
