using GyBlood.Mobile.ViewModels.Auth;

namespace GyBlood.Mobile.Views.Auth;

public partial class PendingAccountPage : ContentPage
{
    private readonly PendingAccountViewModel _viewModel;

    public PendingAccountPage(PendingAccountViewModel viewModel)
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
