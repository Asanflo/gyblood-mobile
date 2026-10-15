using GyBlood.Mobile.ViewModels.Donor;

namespace GyBlood.Mobile.Views.Donor;

public partial class AvailabilityPage : ContentPage
{
    private readonly AvailabilityViewModel _viewModel;

    public AvailabilityPage(AvailabilityViewModel viewModel)
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
