using GyBlood.Mobile.ViewModels.Sponsor;

namespace GyBlood.Mobile.Views.Sponsor;

public partial class FundingPage : ContentPage
{
    private readonly FundingViewModel _viewModel;

    public FundingPage(FundingViewModel viewModel)
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
