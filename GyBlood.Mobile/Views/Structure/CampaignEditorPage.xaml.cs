using GyBlood.Mobile.ViewModels.Structure;

namespace GyBlood.Mobile.Views.Structure;

public partial class CampaignEditorPage : ContentPage
{
    private readonly CampaignEditorViewModel _viewModel;

    public CampaignEditorPage(CampaignEditorViewModel viewModel)
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
