using GyBlood.Mobile.ViewModels.Sponsor;

namespace GyBlood.Mobile.Views.Sponsor;

public partial class MyFundingsPage : ContentPage
{
    private readonly MyFundingsViewModel _viewModel;

    public MyFundingsPage(MyFundingsViewModel viewModel)
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
