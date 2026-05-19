namespace MauiAppUTN2026_001.Views
{
    public partial class ReservaDetallePage : ContentPage
    {
        private readonly ViewModels.ReservaDetalleViewModel _viewModel;

        public ReservaDetallePage(ViewModels.ReservaDetalleViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_viewModel.ReservaId > 0)
                _viewModel.LoadDataCommand.ExecuteAsync(null);
        }
    }
}
