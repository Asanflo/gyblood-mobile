using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Helpers;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Auth;
using GyBlood.Mobile.Services.Dialogs;
using GyBlood.Mobile.Services.Location;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Structure;

/// <summary>
/// Profil de la structure : nom, adresse, téléphone, position GPS (affichée aux patients).
/// Ouvert depuis l'onglet Profil, ou depuis « Compte en attente » pour compléter le dossier avant validation.
/// </summary>
public partial class StructureProfileViewModel : BaseViewModel
{
    private readonly IStructureApi _api;
    private readonly IAuthService _auth;
    private readonly ILocationService _location;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogs;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string structureName = string.Empty;

    [ObservableProperty]
    private string address = string.Empty;

    [ObservableProperty]
    private string phone = string.Empty;

    [ObservableProperty]
    private double? latitude;

    [ObservableProperty]
    private double? longitude;

    [ObservableProperty]
    private string locationText = "Position non renseignée";

    [ObservableProperty]
    private string statusText = string.Empty;

    [ObservableProperty]
    private Color statusColor = Colors.Gray;

    public StructureProfileViewModel(
        IStructureApi api,
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

    private bool IsOnboarding => _auth.CurrentUser?.Status != AccountStatus.Validated;

    public override Task OnAppearingAsync()
    {
        Email = _auth.CurrentUser?.Email ?? string.Empty;
        return RunAsync(async () =>
        {
            var me = await _api.GetMeAsync();
            StructureName = me.Name ?? string.Empty;
            Address = me.Address ?? string.Empty;
            Phone = me.Phone ?? string.Empty;
            SetPosition(me.Latitude, me.Longitude);
            StatusText = Ui.AccountStatusText(me.Status);
            StatusColor = Ui.StatusColor(me.Status);
        });
    }

    [RelayCommand]
    private Task UseMyLocationAsync() => RunAsync(async () =>
    {
        var point = await _location.GetCurrentAsync();
        if (point is null)
        {
            ErrorMessage = "Position indisponible. Activez la localisation de l'appareil.";
            return;
        }

        SetPosition(point.Latitude, point.Longitude);
    });

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(StructureName) || string.IsNullOrWhiteSpace(Address))
        {
            ErrorMessage = "Renseignez le nom et l'adresse de la structure.";
            return;
        }

        if (!InputValidator.IsValidPhone(Phone))
        {
            ErrorMessage = "Numéro de téléphone invalide (9 chiffres).";
            return;
        }

        if (Latitude is null || Longitude is null)
        {
            ErrorMessage = "Renseignez la position avec « Utiliser ma position ».";
            return;
        }

        await RunAsync(async () =>
        {
            await _api.UpdateMeAsync(new UpdateStructureRequest(StructureName.Trim(), Address.Trim(), Latitude.Value, Longitude.Value, Phone.Trim()));
            await _dialogs.ToastAsync("Profil enregistré");
        });

        if (string.IsNullOrEmpty(ErrorMessage) && IsOnboarding)
        {
            await _navigation.GoBackAsync();
        }
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
        LocationText = lat is null || lon is null || (lat == 0 && lon == 0)
            ? "Position non renseignée"
            : $"Position enregistrée ({lat:0.000}, {lon:0.000})";
    }
}
