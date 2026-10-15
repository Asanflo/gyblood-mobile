using GyBlood.Mobile.ViewModels.Patient;

namespace GyBlood.Mobile.Views.Patient;

public partial class BookAppointmentPage : ContentPage
{
    private readonly BookAppointmentViewModel _viewModel;

    public BookAppointmentPage(BookAppointmentViewModel viewModel)
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
