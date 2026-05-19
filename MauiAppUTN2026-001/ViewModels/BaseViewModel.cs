using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiAppUTN2026_001.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _title = string.Empty;
    }
}
