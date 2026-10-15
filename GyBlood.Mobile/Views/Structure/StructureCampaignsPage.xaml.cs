using GyBlood.Mobile.ViewModels.Structure;

namespace GyBlood.Mobile.Views.Structure;

public partial class StructureCampaignsPage : ContentPage
{
    private readonly StructureCampaignsViewModel _viewModel;

    public StructureCampaignsPage(StructureCampaignsViewModel viewModel)
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
