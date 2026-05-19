namespace MauiAppUTN2026_001.Views
{
    public partial class InicioPage : ContentPage
    {
        private readonly ViewModels.InicioViewModel _viewModel;

        public InicioPage(ViewModels.InicioViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadStatsCommand.ExecuteAsync(null);
        }
    }
}
