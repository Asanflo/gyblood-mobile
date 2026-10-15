using GyBlood.Mobile.ViewModels.Donor;

namespace GyBlood.Mobile.Views.Donor;

public partial class DonorCampaignsPage : ContentPage
{
    private readonly DonorCampaignsViewModel _viewModel;

    public DonorCampaignsPage(DonorCampaignsViewModel viewModel)
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
