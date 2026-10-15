using GyBlood.Mobile.ViewModels.Structure;

namespace GyBlood.Mobile.Views.Structure;

public partial class StructureAppointmentsPage : ContentPage
{
    private readonly StructureAppointmentsViewModel _viewModel;

    public StructureAppointmentsPage(StructureAppointmentsViewModel viewModel)
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
