using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Core.Helpers;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Dialogs;
using GyBlood.Mobile.Services.Location;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Patient;

/// <summary>Demande de don : choix de la structure et du créneau, envoi au donneur. (US-13, US-14)</summary>
[QueryProperty(nameof(Donor), "donor")]
public partial class BookAppointmentViewModel : BaseViewModel
{
    private readonly IPatientApi _patientApi;
    private readonly IStructureApi _structureApi;
    private readonly IAppointmentApi _appointmentApi;
    private readonly ILocationService _location;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogs;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DonorName), nameof(BloodText))]
    private DonorDto? donor;

    [ObservableProperty]
    private StructureOption? selectedStructure;

    [ObservableProperty]
    private DateTime date = DateTime.Today.AddDays(1);

    [ObservableProperty]
    private TimeSpan time = new(9, 0, 0);

    public BookAppointmentViewModel(
        IPatientApi patientApi,
        IStructureApi structureApi,
        IAppointmentApi appointmentApi,
        ILocationService location,
        INavigationService navigation,
        IDialogService dialogs)
    {
        _patientApi = patientApi;
        _structureApi = structureApi;
        _appointmentApi = appointmentApi;
        _location = location;
        _navigation = navigation;
        _dialogs = dialogs;
        Title = "Demander un don";
    }

    public ObservableCollection<StructureOption> Structures { get; } = [];

    public DateTime MinimumDate => DateTime.Today;

    public string DonorName => Donor?.FullName ?? string.Empty;

    public string BloodText => Donor is null ? string.Empty : BloodCompatibility.Format(Donor.BloodGroup, Donor.Rhesus);

    public override async Task OnAppearingAsync()
    {
        if (Structures.Count > 0)
        {
            return;
        }

        await RunAsync(async () =>
        {
            var point = await _location.GetCurrentAsync();
            var list = point is null
                ? await _structureApi.GetStructuresAsync()
                : await _patientApi.GetNearestStructuresAsync(point.Latitude, point.Longitude, 10);

            foreach (var structure in list)
            {
                Structures.Add(new StructureOption(structure));
            }

            if (Structures.Count == 0)
            {
                ErrorMessage = "Aucune structure de santé n'est disponible pour le moment.";
            }
        });
    }

    [RelayCommand]
    private async Task SubmitAsync()
    {
        if (Donor is null)
        {
            return;
        }

        if (SelectedStructure is null)
        {
            ErrorMessage = "Choisissez la structure où aura lieu le don.";
            return;
        }

        var local = DateTime.SpecifyKind(Date.Date + Time, DateTimeKind.Local);
        var scheduled = new DateTimeOffset(local);
        if (scheduled <= DateTimeOffset.Now)
        {
            ErrorMessage = "Choisissez un créneau dans le futur.";
            return;
        }

        await RunAsync(async () =>
        {
            await _appointmentApi.CreateAsync(new CreateAppointmentRequest(Donor.Id, SelectedStructure.Dto.Id, scheduled));
            await _dialogs.AlertAsync(
                "Demande envoyée",
                "Le donneur a été prévenu. Vous pourrez régler les frais d'entretien et d'examens dès qu'il aura accepté.");
            await _navigation.GoToAsync(AppRoutes.Tab(AppRoutes.PatientHome));
            await _navigation.GoToAsync(AppRoutes.MyAppointments);
        });
    }
}
