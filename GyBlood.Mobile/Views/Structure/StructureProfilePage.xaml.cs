using GyBlood.Mobile.ViewModels.Structure;

namespace GyBlood.Mobile.Views.Structure;

public partial class StructureProfilePage : ContentPage
{
    private readonly StructureProfileViewModel _viewModel;

    public StructureProfilePage(StructureProfileViewModel viewModel)
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
