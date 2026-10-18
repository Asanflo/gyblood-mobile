using GyBlood.Mobile.ViewModels.Shared;

namespace GyBlood.Mobile.Views.Shared;

public partial class CampaignsPage : ContentPage
{
    private readonly CampaignsViewModel _viewModel;

    public CampaignsPage(CampaignsViewModel viewModel)
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
