using GyBlood.Mobile.ViewModels.Admin;

namespace GyBlood.Mobile.Views.Admin;

public partial class AccountDetailPage : ContentPage
{
    private readonly AccountDetailViewModel _viewModel;

    public AccountDetailPage(AccountDetailViewModel viewModel)
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
