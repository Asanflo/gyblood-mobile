using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Auth;
using GyBlood.Mobile.Services.Dialogs;
using GyBlood.Mobile.Services.Location;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Patient;

/// <summary>Profil du patient : groupe sanguin, rhésus et position, utilisés pour chercher des donneurs compatibles.</summary>
public partial class PatientProfileViewModel : BaseViewModel
{
    private readonly IPatientApi _api;
    private readonly IAuthService _auth;
    private readonly ILocationService _location;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogs;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string? selectedGroup;

    [ObservableProperty]
    private string? selectedRhesus;

    [ObservableProperty]
    private double? latitude;

    [ObservableProperty]
    private double? longitude;

    [ObservableProperty]
    private string locationText = "Position non renseignée";

    public PatientProfileViewModel(
        IPatientApi api,
        IAuthService auth,
        ILocationService location,
        INavigationService navigation,
        IDialogService dialogs)
    {
        _api = api;
        _auth = auth;
        _location = location;
        _navigation = navigation;
        _dialogs = dialogs;
        Title = "Mon profil";
    }

    public IReadOnlyList<string> GroupChoices => Ui.BloodGroupLabels;

    public IReadOnlyList<string> RhesusChoices => Ui.RhesusLabels;

    public override async Task OnAppearingAsync()
    {
        Email = _auth.CurrentUser?.Email ?? string.Empty;
        await RunAsync(async () =>
        {
            var patient = await _api.GetMeAsync();
            SelectedGroup = Ui.GroupLabel(patient.BloodGroup) is { Length: > 0 } g ? g : null;
            SelectedRhesus = Ui.RhesusLabel(patient.Rhesus) is { Length: > 0 } r ? r : null;
            SetPosition(patient.Latitude, patient.Longitude);
        });
    }

    [RelayCommand]
    private async Task UseMyLocationAsync()
    {
        await RunAsync(async () =>
        {
            var point = await _location.GetCurrentAsync();
            if (point is null)
            {
                ErrorMessage = "Position indisponible. Activez la localisation de l'appareil.";
                return;
            }

            SetPosition(point.Latitude, point.Longitude);
        });
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        var group = Ui.ParseGroup(SelectedGroup);
        var rhesus = Ui.ParseRhesus(SelectedRhesus);
        if (group is null || rhesus is null)
        {
            ErrorMessage = "Choisissez votre groupe sanguin et votre rhésus.";
            return;
        }

        if (Latitude is null || Longitude is null)
        {
            ErrorMessage = "Renseignez votre position avec « Utiliser ma position ».";
            return;
        }

        await RunAsync(async () =>
        {
            await _api.UpdateMeAsync(new UpdatePatientRequest(group.Value, rhesus.Value, Latitude.Value, Longitude.Value));
            await _dialogs.ToastAsync("Profil enregistré");
        });
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _auth.LogoutAsync();
        await _navigation.ShowRoleShellAsync(null);
    }

    private void SetPosition(double? lat, double? lon)
    {
        Latitude = lat;
        Longitude = lon;
        LocationText = lat is null || lon is null ? "Position non renseignée" : $"Position enregistrée ({lat:0.000}, {lon:0.000})";
    }
}
