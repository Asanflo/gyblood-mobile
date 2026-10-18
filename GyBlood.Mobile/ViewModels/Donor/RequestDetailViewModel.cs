using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Dialogs;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Donor;

/// <summary>Détail d'une demande : le donneur ne voit pas l'identité du patient (RG3). (US-23)</summary>
[QueryProperty(nameof(Appointment), "appointment")]
public partial class RequestDetailViewModel : BaseViewModel
{
    private readonly IAppointmentApi _api;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogs;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StructureName), nameof(DateText), nameof(StatusText), nameof(StatusColor), nameof(FeeText), nameof(HasFee), nameof(CanRespond))]
    private AppointmentDto? appointment;

    public RequestDetailViewModel(IAppointmentApi api, INavigationService navigation, IDialogService dialogs)
    {
        _api = api;
        _navigation = navigation;
        _dialogs = dialogs;
        Title = "Demande de don";
    }

    public string StructureName => Appointment?.StructureName ?? string.Empty;

    public string DateText => Appointment is null ? string.Empty : Ui.DayAndHour(Appointment.ScheduledAt);

    public string StatusText => Appointment is null ? string.Empty : Ui.AppointmentStatusText(Appointment.Status);

    public Color StatusColor => Appointment is null ? Colors.Gray : Ui.StatusColor(Appointment.Status);

    public string FeeText => Appointment?.HospitalFee is { } fee and > 0 ? Ui.Money(fee) : string.Empty;

    public bool HasFee => !string.IsNullOrEmpty(FeeText);

    public bool CanRespond => Appointment?.Status == AppointmentStatus.Requested;

    [RelayCommand]
    private async Task AcceptAsync()
    {
        if (Appointment is null)
        {
            return;
        }

        await RunAsync(async () =>
        {
            Appointment = await _api.AcceptAsync(Appointment.Id);
            await _dialogs.ToastAsync("Demande acceptée");
        });
    }

    [RelayCommand]
    private async Task DeclineAsync()
    {
        if (Appointment is null)
        {
            return;
        }

        if (!await _dialogs.ConfirmAsync("Refuser la demande", "Voulez-vous refuser cette demande de don ?", "Refuser", "Retour"))
        {
            return;
        }

        await RunAsync(async () =>
        {
            await _api.DeclineAsync(Appointment.Id);
            await _navigation.GoBackAsync();
        });
    }
}
