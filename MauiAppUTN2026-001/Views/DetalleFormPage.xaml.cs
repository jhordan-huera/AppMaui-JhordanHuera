namespace MauiAppUTN2026_001.Views
{
    public partial class DetalleFormPage : ContentPage
    {
        public DetalleFormPage(ViewModels.DetalleFormViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
