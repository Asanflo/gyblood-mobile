using GyBlood.Mobile.ViewModels.Donor;

namespace GyBlood.Mobile.Views.Donor;

public partial class DonationHistoryPage : ContentPage
{
    private readonly DonationHistoryViewModel _viewModel;

    public DonationHistoryPage(DonationHistoryViewModel viewModel)
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
