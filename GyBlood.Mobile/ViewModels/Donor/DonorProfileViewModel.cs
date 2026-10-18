using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Core.Helpers;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Auth;
using GyBlood.Mobile.Services.Dialogs;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;
using Refit;

namespace GyBlood.Mobile.ViewModels.Donor;

/// <summary>
/// Profil du donneur (maquette « Informations médicales ») : identité, groupe, antécédents, justificatif de groupe sanguin.
/// Ouvert depuis l'onglet Profil, ou depuis « Compte en attente » pour compléter le dossier avant validation. (US-20, US-21)
/// </summary>
public partial class DonorProfileViewModel : BaseViewModel
{
    private static readonly FilePickerFileType DocumentTypes = new(new Dictionary<DevicePlatform, IEnumerable<string>>
    {
        [DevicePlatform.Android] = new[] { "application/pdf", "image/*" },
        [DevicePlatform.iOS] = new[] { "com.adobe.pdf", "public.image" },
        [DevicePlatform.WinUI] = new[] { ".pdf", ".png", ".jpg", ".jpeg" },
    });

    private readonly IDonorApi _api;
    private readonly IAuthService _auth;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogs;
    private FileResult? _pickedDocument;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string fullName = string.Empty;

    [ObservableProperty]
    private string? selectedGroup;

    [ObservableProperty]
    private string? selectedRhesus;

    [ObservableProperty]
    private string? selectedSex;

    [ObservableProperty]
    private string phone = string.Empty;

    [ObservableProperty]
    private string medicalHistory = string.Empty;

    [ObservableProperty]
    private string documentText = "Aucun document";

    [ObservableProperty]
    private string statusText = string.Empty;

    [ObservableProperty]
    private Color statusColor = Colors.Gray;

    public DonorProfileViewModel(IDonorApi api, IAuthService auth, INavigationService navigation, IDialogService dialogs)
    {
        _api = api;
        _auth = auth;
        _navigation = navigation;
        _dialogs = dialogs;
        Title = "Mon profil";
    }

    public IReadOnlyList<string> GroupChoices => Ui.BloodGroupLabels;

    public IReadOnlyList<string> RhesusChoices => Ui.RhesusLabels;

    public IReadOnlyList<string> SexChoices => Ui.SexLabels;

    /// <summary>Vrai tant que le compte n'est pas validé : on revient à l'écran précédent après l'enregistrement.</summary>
    private bool IsOnboarding => _auth.CurrentUser?.Status != AccountStatus.Validated;

    public override Task OnAppearingAsync()
    {
        Email = _auth.CurrentUser?.Email ?? string.Empty;
        return RunAsync(async () =>
        {
            var profile = await _api.GetMeAsync();
            FullName = profile.FullName ?? string.Empty;
            SelectedGroup = Ui.GroupLabel(profile.BloodGroup) is { Length: > 0 } g ? g : null;
            SelectedRhesus = Ui.RhesusLabel(profile.Rhesus) is { Length: > 0 } r ? r : null;
            SelectedSex = Ui.SexLabel(profile.Sex) is { Length: > 0 } s ? s : null;
            Phone = profile.Phone ?? string.Empty;
            MedicalHistory = profile.MedicalHistory ?? string.Empty;
            if (_pickedDocument is null)
            {
                DocumentText = string.IsNullOrEmpty(profile.DocumentUrl) ? "Aucun document" : "Document envoyé";
            }

            StatusText = Ui.AccountStatusText(profile.Status);
            StatusColor = Ui.StatusColor(profile.Status);
        });
    }

    [RelayCommand]
    private async Task PickDocumentAsync()
    {
        try
        {
            var file = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Justificatif de groupe sanguin",
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
            _pickedDocument = file;
            DocumentText = file.FileName;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            ErrorMessage = "Impossible de lire ce fichier.";
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        var group = Ui.ParseGroup(SelectedGroup);
        var rhesus = Ui.ParseRhesus(SelectedRhesus);
        var sex = Ui.ParseSex(SelectedSex);

        if (string.IsNullOrWhiteSpace(FullName) || group is null || rhesus is null || sex is null)
        {
            ErrorMessage = "Renseignez votre nom, votre sexe, votre groupe sanguin et votre rhésus.";
            return;
        }

        if (!string.IsNullOrWhiteSpace(Phone) && !InputValidator.IsValidPhone(Phone))
        {
            ErrorMessage = "Numéro de téléphone invalide (9 chiffres).";
            return;
        }

        await RunAsync(async () =>
        {
            var phone = string.IsNullOrWhiteSpace(Phone) ? null : Phone.Trim();
            await _api.UpdateMeAsync(new UpdateDonorRequest(FullName.Trim(), sex.Value, MedicalHistory.Trim(), phone, group.Value, rhesus.Value));

            if (_pickedDocument is not null)
            {
                await using var stream = await _pickedDocument.OpenReadAsync();
                var part = new StreamPart(stream, _pickedDocument.FileName, _pickedDocument.ContentType);
                await _api.UploadBloodDocumentAsync(part);
                _pickedDocument = null;
                DocumentText = "Document envoyé";
            }

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
}
