using GyBlood.Mobile.ViewModels.Sponsor;

namespace GyBlood.Mobile.Views.Sponsor;

public partial class SponsorCampaignsPage : ContentPage
{
    private readonly SponsorCampaignsViewModel _viewModel;

    public SponsorCampaignsPage(SponsorCampaignsViewModel viewModel)
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
