using GyBlood.Mobile.ViewModels.Auth;

namespace GyBlood.Mobile.Views.Auth;

public partial class RoleChoicePage : ContentPage
{
    private readonly RoleChoiceViewModel _viewModel;

    public RoleChoicePage(RoleChoiceViewModel viewModel)
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
