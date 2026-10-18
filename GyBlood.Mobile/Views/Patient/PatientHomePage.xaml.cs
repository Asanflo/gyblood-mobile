using GyBlood.Mobile.ViewModels.Patient;

namespace GyBlood.Mobile.Views.Patient;

public partial class PatientHomePage : ContentPage
{
    private readonly PatientHomeViewModel _viewModel;

    public PatientHomePage(PatientHomeViewModel viewModel)
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
