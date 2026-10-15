using GyBlood.Mobile.ViewModels.Patient;

namespace GyBlood.Mobile.Views.Patient;

public partial class PatientProfilePage : ContentPage
{
    private readonly PatientProfileViewModel _viewModel;

    public PatientProfilePage(PatientProfileViewModel viewModel)
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
