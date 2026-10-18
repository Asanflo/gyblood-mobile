using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Auth;
using GyBlood.Mobile.Services.Location;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Patient;

/// <summary>Accueil du patient (maquette « Bonjour, … ») : accès rapides, rendez-vous en cours, campagnes.</summary>
public partial class PatientHomeViewModel : BaseViewModel
{
    private readonly IAuthService _auth;
    private readonly IPatientApi _patientApi;
    private readonly IAppointmentApi _appointmentApi;
    private readonly ICampaignApi _campaignApi;
    private readonly ILocationService _location;
    private readonly INavigationService _navigation;

    [ObservableProperty]
    private string greeting = "Bonjour";

    [ObservableProperty]
    private string bannerTitle = "Trouvez un donneur près de vous";

    [ObservableProperty]
    private string bannerSubtitle = "Voir la carte";

    [ObservableProperty]
    private bool profileIncomplete;

    [ObservableProperty]
    private string appointmentsText = "Aucun rendez-vous en cours";

    [ObservableProperty]
    private bool hasCampaigns;

    public PatientHomeViewModel(
        IAuthService auth,
        IPatientApi patientApi,
        IAppointmentApi appointmentApi,
        ICampaignApi campaignApi,
        ILocationService location,
        INavigationService navigation)
    {
        _auth = auth;
        _patientApi = patientApi;
        _appointmentApi = appointmentApi;
        _campaignApi = campaignApi;
        _location = location;
        _navigation = navigation;
        Title = "Accueil";
    }

    public ObservableCollection<CampaignItem> Campaigns { get; } = [];

    public override async Task OnAppearingAsync()
    {
        var user = _auth.CurrentUser;
        var name = !string.IsNullOrWhiteSpace(user?.DisplayName) ? user!.DisplayName : user?.Email.Split('@')[0];
        Greeting = string.IsNullOrWhiteSpace(name) ? "Bonjour" : $"Bonjour, {name}";

        await RunAsync(async () =>
        {
            await LoadBannerAsync();
            await LoadAppointmentsAsync();
            await LoadCampaignsAsync();
        });
    }

    private async Task LoadBannerAsync()
    {
        var patient = await _patientApi.GetMeAsync();
        if (patient.BloodGroup is not { } group || patient.Rhesus is not { } rhesus)
        {
            ProfileIncomplete = false;
            BannerTitle = "Besoin de sang ?";
            BannerSubtitle = "Choisissez le groupe recherché";
            return;
        }

        ProfileIncomplete = false;
        var point = await _location.GetCurrentAsync();
        var lat = point?.Latitude ?? patient.Latitude;
        var lon = point?.Longitude ?? patient.Longitude;
        if (lat is null || lon is null)
        {
            BannerTitle = "Trouvez un donneur près de vous";
            BannerSubtitle = "Activez la localisation";
            return;
        }

        var donors = await _patientApi.SearchDonorsAsync(group, rhesus, lat.Value, lon.Value, AppConstants.DefaultSearchRadiusKm);
        BannerTitle = donors.Count switch
        {
            0 => "Aucun donneur connecté pour l'instant",
            1 => "1 donneur disponible près de vous",
            _ => $"{donors.Count} donneurs disponibles près de vous",
        };
        BannerSubtitle = "Voir la carte";
    }

    private async Task LoadAppointmentsAsync()
    {
        var page = await _appointmentApi.GetAppointmentsAsync(size: 50);
        var active = page.Content.Count(a => a.Status is AppointmentStatus.Requested or AppointmentStatus.Accepted or AppointmentStatus.Paid);
        AppointmentsText = active switch
        {
            0 => "Aucun rendez-vous en cours",
            1 => "1 rendez-vous en cours",
            _ => $"{active} rendez-vous en cours",
        };
    }

    private async Task LoadCampaignsAsync()
    {
        var page = await _campaignApi.GetCampaignsAsync(size: 3);
        Campaigns.Clear();
        foreach (var dto in page.Content)
        {
            Campaigns.Add(CampaignItems.From(dto, _navigation));
        }

        HasCampaigns = Campaigns.Count > 0;
    }

    [RelayCommand]
    private Task OpenBannerAsync()
        => _navigation.GoToAsync(AppRoutes.Tab(ProfileIncomplete ? AppRoutes.PatientProfile : AppRoutes.PatientMap));

    [RelayCommand]
    private Task OpenMapAsync() => _navigation.GoToAsync(AppRoutes.Tab(AppRoutes.PatientMap));

    [RelayCommand]
    private Task OpenStocksAsync() => _navigation.GoToAsync(AppRoutes.Stocks);

    [RelayCommand]
    private Task OpenCampaignsAsync() => _navigation.GoToAsync(AppRoutes.Tab(AppRoutes.PatientCampaigns));

    [RelayCommand]
    private Task OpenAppointmentsAsync() => _navigation.GoToAsync(AppRoutes.MyAppointments);

    [RelayCommand]
    private Task OpenNotificationsAsync() => _navigation.GoToAsync(AppRoutes.Notifications);

    [RelayCommand]
    private Task OpenProfileAsync() => _navigation.GoToAsync(AppRoutes.Tab(AppRoutes.PatientProfile));
}
