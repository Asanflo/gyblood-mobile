using GyBlood.Mobile.ViewModels.Donor;

namespace GyBlood.Mobile.Views.Donor;

public partial class RequestDetailPage : ContentPage
{
    private readonly RequestDetailViewModel _viewModel;

    public RequestDetailPage(RequestDetailViewModel viewModel)
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
