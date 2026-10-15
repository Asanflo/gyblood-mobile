using GyBlood.Mobile.ViewModels.Shared;

namespace GyBlood.Mobile.Views.Shared;

public partial class CampaignDetailPage : ContentPage
{
    private readonly CampaignDetailViewModel _viewModel;

    public CampaignDetailPage(CampaignDetailViewModel viewModel)
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
