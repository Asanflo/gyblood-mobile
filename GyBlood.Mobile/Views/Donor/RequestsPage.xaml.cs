using GyBlood.Mobile.ViewModels.Donor;

namespace GyBlood.Mobile.Views.Donor;

public partial class RequestsPage : ContentPage
{
    private readonly RequestsViewModel _viewModel;

    public RequestsPage(RequestsViewModel viewModel)
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
