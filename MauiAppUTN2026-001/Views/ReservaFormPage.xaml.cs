namespace MauiAppUTN2026_001.Views
{
    public partial class ReservaFormPage : ContentPage
    {
        public ReservaFormPage(ViewModels.ReservaFormViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
