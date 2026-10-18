using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Core.Helpers;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Location;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Patient;

/// <summary>
/// Demande de sang : le patient choisit le groupe recherché, la carte affiche les donneurs compatibles connectés. (US-12)
/// Le groupe choisi est enregistré sur son profil (l'API l'exige pour créer un rendez-vous).
/// </summary>
public partial class DonorMapViewModel : BaseViewModel
{
    private readonly IPatientApi _patientApi;
    private readonly ILocationService _location;
    private readonly INavigationService _navigation;
    private bool _profileLoaded;
    private BloodGroup? _savedGroup;
    private Rhesus? _savedRhesus;

    [ObservableProperty]
    private double radiusKm = AppConstants.DefaultSearchRadiusKm;

    [ObservableProperty]
    private string summary = "Choisissez le groupe sanguin recherché.";

    [ObservableProperty]
    private bool hasDonors;

    [ObservableProperty]
    private string? selectedGroup;

    [ObservableProperty]
    private string? selectedRhesus;

    /// <summary>Dernière position utilisée : la page recentre la carte quand elle change.</summary>
    [ObservableProperty]
    private GeoPoint? center;

    public DonorMapViewModel(IPatientApi patientApi, ILocationService location, INavigationService navigation)
    {
        _patientApi = patientApi;
        _location = location;
        _navigation = navigation;
        Title = "Demander du sang";
    }

    public ObservableCollection<DonorItem> Donors { get; } = [];

    public IReadOnlyList<string> GroupChoices => Ui.BloodGroupLabels;

    public IReadOnlyList<string> RhesusChoices => Ui.RhesusLabels;

    public override async Task OnAppearingAsync()
    {
        if (!_profileLoaded)
        {
            // Si le patient a déjà demandé du sang, on préremplit son dernier groupe.
            await RunAsync(async () =>
            {
                var patient = await _patientApi.GetMeAsync();
                _savedGroup = patient.BloodGroup;
                _savedRhesus = patient.Rhesus;
                SelectedGroup = Ui.GroupLabel(patient.BloodGroup) is { Length: > 0 } g ? g : null;
                SelectedRhesus = Ui.RhesusLabel(patient.Rhesus) is { Length: > 0 } r ? r : null;
                _profileLoaded = true;
            });
        }

        if (Ui.ParseGroup(SelectedGroup) is not null && Ui.ParseRhesus(SelectedRhesus) is not null)
        {
            await SearchAsync();
        }
    }

    [RelayCommand]
    private async Task SetRadiusAsync(string km)
    {
        if (double.TryParse(km, out var value))
        {
            RadiusKm = Math.Min(value, AppConstants.MaxSearchRadiusKm);
            await SearchAsync();
        }
    }

    [RelayCommand]
    private Task SearchAsync() => RunAsync(async () =>
    {
        var group = Ui.ParseGroup(SelectedGroup);
        var rhesus = Ui.ParseRhesus(SelectedRhesus);
        if (group is null || rhesus is null)
        {
            Donors.Clear();
            HasDonors = false;
            Summary = "Choisissez le groupe sanguin et le rhésus recherchés.";
            return;
        }

        var point = await _location.GetCurrentAsync();
        if (point is null)
        {
            Donors.Clear();
            HasDonors = false;
            Summary = "Activez la localisation pour voir les donneurs autour de vous.";
            return;
        }

        // Mémorise le groupe demandé (et la position) sur le profil du patient, nécessaire pour créer le rendez-vous.
        if (group != _savedGroup || rhesus != _savedRhesus)
        {
            await _patientApi.UpdateMeAsync(new UpdatePatientRequest(group.Value, rhesus.Value, point.Latitude, point.Longitude));
            _savedGroup = group;
            _savedRhesus = rhesus;
        }

        Center = point;
        var found = await _patientApi.SearchDonorsAsync(group.Value, rhesus.Value, point.Latitude, point.Longitude, RadiusKm);

        Donors.Clear();
        foreach (var donor in found)
        {
            Donors.Add(new DonorItem
            {
                Dto = donor,
                Name = donor.FullName,
                BloodText = BloodCompatibility.Format(donor.BloodGroup, donor.Rhesus),
                DistanceText = Ui.Distance(donor.DistanceKm),
                OpenCommand = new AsyncRelayCommand(() =>
                    _navigation.GoToAsync(AppRoutes.DonorDetail, new Dictionary<string, object> { ["donor"] = donor })),
            });
        }

        HasDonors = Donors.Count > 0;
        Summary = Donors.Count switch
        {
            0 => $"Aucun donneur connecté dans un rayon de {RadiusKm:0} km.",
            1 => $"1 donneur dans un rayon de {RadiusKm:0} km",
            _ => $"{Donors.Count} donneurs dans un rayon de {RadiusKm:0} km",
        };
    });
}