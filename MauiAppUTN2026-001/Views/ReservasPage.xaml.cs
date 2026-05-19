namespace MauiAppUTN2026_001.Views
{
    public partial class ReservasPage : ContentPage
    {
        private readonly ViewModels.ReservasViewModel _viewModel;

        public ReservasPage(ViewModels.ReservasViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadReservasCommand.ExecuteAsync(null);
        }
    }
}
