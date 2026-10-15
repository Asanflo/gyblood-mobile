using GyBlood.Mobile.ViewModels.Patient;

namespace GyBlood.Mobile.Views.Patient;

public partial class StocksPage : ContentPage
{
    private readonly StocksViewModel _viewModel;

    public StocksPage(StocksViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.OnAppearingAsync();
    }
}
