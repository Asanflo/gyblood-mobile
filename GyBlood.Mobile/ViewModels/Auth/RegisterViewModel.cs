using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Core.Helpers;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Auth;
using GyBlood.Mobile.Services.Dialogs;
using GyBlood.Mobile.Services.Location;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;
using Refit;

namespace GyBlood.Mobile.ViewModels.Auth;

public sealed record RoleOption(UserRole Role, string Label);

/// <summary>
/// Formulaire d'inscription unique : les champs affichés dépendent du rôle choisi.
/// À la validation : création du compte, puis enregistrement du profil (et de l'attestation pour un donneur).
/// </summary>
[QueryProperty(nameof(RoleParam), "role")]
public partial class RegisterViewModel : BaseViewModel
{
    private const string Individual = "Particulier";
    private const string Organisation_ = "Organisation";

    private static readonly FilePickerFileType DocumentTypes = new(new Dictionary<DevicePlatform, IEnumerable<string>>
    {
        [DevicePlatform.Android] = new[] { "application/pdf", "image/*" },
        [DevicePlatform.iOS] = new[] { "com.adobe.pdf", "public.image" },
        [DevicePlatform.WinUI] = new[] { ".pdf", ".png", ".jpg", ".jpeg" },
    });

    private readonly IAuthService _auth;
    private readonly IDonorApi _donorApi;
    private readonly IStructureApi _structureApi;
    private readonly ISponsorApi _sponsorApi;
    private readonly ILocationService _location;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogs;
    private FileResult? _document;

    // ----- Communs
    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string confirmPassword = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RoleLabel), nameof(HeaderText), nameof(IsPatient), nameof(IsDonor), nameof(IsStructure), nameof(IsSponsor))]
    private RoleOption selectedRole;

    // ----- Patient et donneur : nom ; donneur et structure : téléphone
    [ObservableProperty]
    private string fullName = string.Empty;

    [ObservableProperty]
    private string phone = string.Empty;

    // ----- Donneur
    [ObservableProperty]
    private string? selectedSex;

    [ObservableProperty]
    private string? selectedGroup;

    [ObservableProperty]
    private string? selectedRhesus;

    [ObservableProperty]
    private string medicalHistory = string.Empty;

    [ObservableProperty]
    private string documentText = "Touchez pour ajouter votre attestation";

    // ----- Structure
    [ObservableProperty]
    private string structureName = string.Empty;

    [ObservableProperty]
    private string address = string.Empty;

    [ObservableProperty]
    private double? latitude;

    [ObservableProperty]
    private double? longitude;

    [ObservableProperty]
    private string locationText = "Position non renseignée";

    // ----- Sponsor (le type ne change que les libellés : l'API ne stocke que « organisation » et « contact »)
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SponsorNameHeader), nameof(SponsorNamePlaceholder), nameof(SponsorContactHeader), nameof(SponsorContactPlaceholder))]
    private string selectedKind = Individual;

    [ObservableProperty]
    private string organisation = string.Empty;

    [ObservableProperty]
    private string contact = string.Empty;

    public RegisterViewModel(
        IAuthService auth,
        IDonorApi donorApi,
        IStructureApi structureApi,
        ISponsorApi sponsorApi,
        ILocationService location,
        INavigationService navigation,
        IDialogService dialogs)
    {
        _auth = auth;
        _donorApi = donorApi;
        _structureApi = structureApi;
        _sponsorApi = sponsorApi;
        _location = location;
        _navigation = navigation;
        _dialogs = dialogs;
        selectedRole = Roles[0];
        Title = "Inscription";
    }

    public IReadOnlyList<RoleOption> Roles { get; } =
    [
        new(UserRole.Patient, "Patient"),
        new(UserRole.Donor, "Donneur"),
        new(UserRole.Structure, "Banque de sang / hôpital"),
        new(UserRole.Sponsor, "Sponsor"),
    ];

    public IReadOnlyList<string> SexChoices => Ui.SexLabels;

    public IReadOnlyList<string> GroupChoices => Ui.BloodGroupLabels;

    public IReadOnlyList<string> RhesusChoices => Ui.RhesusLabels;

    public IReadOnlyList<string> KindChoices { get; } = [Individual, Organisation_];

    /// <summary>Reçu par la navigation (paramètre « role »).</summary>
    public UserRole RoleParam
    {
        set => SelectedRole = Roles.FirstOrDefault(r => r.Role == value) ?? Roles[0];
    }

    public string RoleLabel => SelectedRole.Label;

    public bool IsPatient => SelectedRole.Role == UserRole.Patient;

    public bool IsDonor => SelectedRole.Role == UserRole.Donor;

    public bool IsStructure => SelectedRole.Role == UserRole.Structure;

    public bool IsSponsor => SelectedRole.Role == UserRole.Sponsor;

    public string HeaderText => SelectedRole.Role switch
    {
        UserRole.Donor => "Inscription donneur",
        UserRole.Structure => "Inscription banque de sang / hôpital",
        UserRole.Sponsor => "Inscription particulier / sponsor",
        _ => "Inscription patient",
    };

    public string SponsorNameHeader => SelectedKind == Individual ? "Nom complet" : "Nom de l'organisation";

    public string SponsorNamePlaceholder => SelectedKind == Individual ? "Votre nom et prénom" : "Entreprise, ONG, association…";

    public string SponsorContactHeader => SelectedKind == Individual ? "Téléphone ou email" : "Personne à contacter";

    public string SponsorContactPlaceholder => SelectedKind == Individual ? "6XXXXXXXX ou vous@exemple.com" : "Nom et téléphone du contact";

    [RelayCommand]
    private async Task PickDocumentAsync()
    {
        try
        {
            var file = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Attestation de groupe sanguin",
                FileTypes = DocumentTypes,
            });

            if (file is null)
            {
                return;
            }

            await using var stream = await file.OpenReadAsync();
            if (stream.Length > AppConstants.MaxDocumentBytes)
            {
                ErrorMessage = "Le document dépasse 5 Mo.";
                return;
            }

            ErrorMessage = null;
            _document = file;
            DocumentText = file.FileName;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            ErrorMessage = "Impossible de lire ce fichier.";
        }
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

        Latitude = point.Latitude;
        Longitude = point.Longitude;
        LocationText = $"Position enregistrée ({point.Latitude:0.000}, {point.Longitude:0.000})";
    });

    [RelayCommand]
    private async Task RegisterAsync()
    {
        var problem = Validate();
        if (problem is not null)
        {
            ErrorMessage = problem;
            return;
        }

        await RunAsync(async () =>
        {
            var user = await _auth.RegisterAsync(Email.Trim(), Password, SelectedRole.Role, DisplayNameForRole());

            try
            {
                await SaveProfileAsync();
            }
            catch (Exception ex) when (ex is ApiException or HttpRequestException or TaskCanceledException)
            {
                // Le compte existe déjà : on ne perd rien, l'utilisateur termine son profil sur la page dédiée.
                await _navigation.ShowRoleShellAsync(user);
                await OpenProfileFallbackAsync(user);
                await _dialogs.AlertAsync("Compte créé", "Votre compte est créé, mais votre profil n'a pas pu être enregistré. Complétez-le sur cette page.");
                return;
            }

            await _navigation.ShowRoleShellAsync(user);
        });
    }

    [RelayCommand]
    private async Task RegisterWithGoogleAsync()
    {
        await RunAsync(async () =>
        {
            var user = await _auth.LoginWithGoogleAsync(SelectedRole.Role);
            if (user is null)
            {
                return;
            }

            await _navigation.ShowRoleShellAsync(user);
            if (user.Status == AccountStatus.Pending)
            {
                await OpenProfileFallbackAsync(user);
            }
        });
    }

    [RelayCommand]
    private Task BackToLoginAsync() => _navigation.GoToAsync(AppRoutes.Tab(AppRoutes.Login));

    private string? Validate()
    {
        if (!InputValidator.IsValidEmail(Email))
        {
            return "Adresse email invalide.";
        }

        if (!InputValidator.IsValidPassword(Password))
        {
            return $"Le mot de passe doit contenir au moins {InputValidator.MinPasswordLength} caractères.";
        }

        if (Password != ConfirmPassword)
        {
            return "Les mots de passe ne correspondent pas.";
        }

        switch (SelectedRole.Role)
        {
            case UserRole.Patient:
                return string.IsNullOrWhiteSpace(FullName) ? "Renseignez votre nom." : null;

            case UserRole.Donor:
                if (string.IsNullOrWhiteSpace(FullName) || Ui.ParseSex(SelectedSex) is null
                    || Ui.ParseGroup(SelectedGroup) is null || Ui.ParseRhesus(SelectedRhesus) is null)
                {
                    return "Renseignez votre nom, votre sexe, votre groupe sanguin et votre rhésus.";
                }

                if (!string.IsNullOrWhiteSpace(Phone) && !InputValidator.IsValidPhone(Phone))
                {
                    return "Numéro de téléphone invalide (9 chiffres).";
                }

                return _document is null ? "Ajoutez votre attestation de test de groupe sanguin (PDF ou image)." : null;

            case UserRole.Structure:
                if (string.IsNullOrWhiteSpace(StructureName) || string.IsNullOrWhiteSpace(Address))
                {
                    return "Renseignez le nom et l'adresse de la structure.";
                }

                if (!InputValidator.IsValidPhone(Phone))
                {
                    return "Numéro de téléphone invalide (9 chiffres).";
                }

                return Latitude is null || Longitude is null ? "Renseignez la position avec « Utiliser ma position »." : null;

            case UserRole.Sponsor:
                return string.IsNullOrWhiteSpace(Organisation) || string.IsNullOrWhiteSpace(Contact)
                    ? $"Renseignez « {SponsorNameHeader} » et « {SponsorContactHeader} »."
                    : null;

            default:
                return null;
        }
    }

    private string DisplayNameForRole() => SelectedRole.Role switch
    {
        UserRole.Structure => StructureName.Trim(),
        UserRole.Sponsor => Organisation.Trim(),
        _ => FullName.Trim(),
    };

    /// <summary>Envoie le profil propre au rôle. Le patient n'en a pas : son groupe est demandé quand il lance une demande de sang.</summary>
    private async Task SaveProfileAsync()
    {
        switch (SelectedRole.Role)
        {
            case UserRole.Donor:
                var phoneValue = string.IsNullOrWhiteSpace(Phone) ? null : Phone.Trim();
                await _donorApi.UpdateMeAsync(new UpdateDonorRequest(
                    FullName.Trim(),
                    Ui.ParseSex(SelectedSex)!.Value,
                    MedicalHistory.Trim(),
                    phoneValue,
                    Ui.ParseGroup(SelectedGroup)!.Value,
                    Ui.ParseRhesus(SelectedRhesus)!.Value));

                await using (var stream = await _document!.OpenReadAsync())
                {
                    await _donorApi.UploadBloodDocumentAsync(new StreamPart(stream, _document.FileName, _document.ContentType));
                }

                break;

            case UserRole.Structure:
                await _structureApi.UpdateMeAsync(new UpdateStructureRequest(
                    StructureName.Trim(), Address.Trim(), Latitude!.Value, Longitude!.Value, Phone.Trim()));
                break;

            case UserRole.Sponsor:
                await _sponsorApi.UpdateMeAsync(new UpdateSponsorRequest(Organisation.Trim(), Contact.Trim(), null));
                break;
        }
    }

    private Task OpenProfileFallbackAsync(UserDto user) => user.Role switch
    {
        UserRole.Donor => _navigation.GoToAsync(AppRoutes.CompleteDonorProfile),
        UserRole.Structure => _navigation.GoToAsync(AppRoutes.CompleteStructureProfile),
        UserRole.Sponsor => _navigation.GoToAsync(AppRoutes.Tab(AppRoutes.SponsorProfile)),
        _ => Task.CompletedTask,
    };
}