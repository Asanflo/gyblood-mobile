using GyBlood.Mobile.ViewModels.Sponsor;

namespace GyBlood.Mobile.Views.Sponsor;

public partial class SponsorProfilePage : ContentPage
{
    private readonly SponsorProfileViewModel _viewModel;

    public SponsorProfilePage(SponsorProfileViewModel viewModel)
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
