using GyBlood.Mobile.ViewModels.Structure;

namespace GyBlood.Mobile.Views.Structure;

public partial class StockEditorPage : ContentPage
{
    private readonly StockEditorViewModel _viewModel;

    public StockEditorPage(StockEditorViewModel viewModel)
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
