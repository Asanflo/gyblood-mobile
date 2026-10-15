using GyBlood.Mobile.ViewModels.Admin;

namespace GyBlood.Mobile.Views.Admin;

public partial class PendingAccountsPage : ContentPage
{
    private readonly PendingAccountsViewModel _viewModel;

    public PendingAccountsPage(PendingAccountsViewModel viewModel)
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
