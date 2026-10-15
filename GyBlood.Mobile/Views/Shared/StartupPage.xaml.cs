using GyBlood.Mobile.ViewModels.Shared;

namespace GyBlood.Mobile.Views.Shared;

public partial class StartupPage : ContentPage
{
    private readonly StartupViewModel _viewModel;

    public StartupPage(StartupViewModel viewModel)
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
