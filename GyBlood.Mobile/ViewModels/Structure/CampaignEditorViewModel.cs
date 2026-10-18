using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Dialogs;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Structure;

/// <summary>Création d'une campagne de don par la structure. (US-24)</summary>
public partial class CampaignEditorViewModel : BaseViewModel
{
    private readonly ICampaignApi _api;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogs;

    // Image choisie : conservée en mémoire, prête à être envoyée quand l'API le permettra.
    private byte[]? _imageBytes;
    private string? _imageFileName;
    private string? _imageContentType;

    [ObservableProperty]
    private string campaignTitle = string.Empty;

    [ObservableProperty]
    private string location = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private string targetText = string.Empty;

    [ObservableProperty]
    private DateTime startDate = DateTime.Today.AddDays(7);

    [ObservableProperty]
    private DateTime endDate = DateTime.Today.AddDays(7);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasImage), nameof(HasNoImage))]
    private ImageSource? imagePreview;

    public CampaignEditorViewModel(ICampaignApi api, INavigationService navigation, IDialogService dialogs)
    {
        _api = api;
        _navigation = navigation;
        _dialogs = dialogs;
        Title = "Nouvelle campagne";
    }

    public DateTime MinimumDate => DateTime.Today;

    public bool HasImage => ImagePreview is not null;

    public bool HasNoImage => ImagePreview is null;

    [RelayCommand]
    private async Task PickImageAsync()
    {
        try
        {
            var file = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Visuel de la campagne",
                FileTypes = FilePickerFileType.Images,
            });

            if (file is null)
            {
                return;
            }

            await using var stream = await file.OpenReadAsync();
            if (stream.Length > AppConstants.MaxDocumentBytes)
            {
                ErrorMessage = "L'image dépasse 5 Mo.";
                return;
            }

            using var memory = new MemoryStream();
            await stream.CopyToAsync(memory);
            var bytes = memory.ToArray();

            _imageBytes = bytes;
            _imageFileName = file.FileName;
            _imageContentType = file.ContentType;
            ImagePreview = ImageSource.FromStream(() => new MemoryStream(bytes));
            ErrorMessage = null;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            ErrorMessage = "Impossible de lire cette image.";
        }
    }

    [RelayCommand]
    private void RemoveImage()
    {
        _imageBytes = null;
        _imageFileName = null;
        _imageContentType = null;
        ImagePreview = null;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(CampaignTitle) || string.IsNullOrWhiteSpace(Location))
        {
            ErrorMessage = "Renseignez le titre et le lieu de la campagne.";
            return;
        }

        if (!int.TryParse(TargetText, out var target) || target <= 0)
        {
            ErrorMessage = "L'objectif doit être un nombre de dons supérieur à 0.";
            return;
        }

        if (EndDate.Date < StartDate.Date)
        {
            ErrorMessage = "La date de fin doit suivre la date de début.";
            return;
        }

        await RunAsync(async () =>
        {
            var description = string.IsNullOrWhiteSpace(Description) ? null : Description.Trim();
            await _api.CreateAsync(new CreateCampaignRequest(
                CampaignTitle.Trim(),
                Location.Trim(),
                new DateTimeOffset(StartDate.Date),
                new DateTimeOffset(EndDate.Date.AddHours(23).AddMinutes(59)),
                target,
                description));

            // TODO (API) : envoyer _imageBytes / _imageFileName / _imageContentType quand l'API acceptera une image de campagne.
            await _dialogs.ToastAsync("Campagne publiée");
            await _navigation.GoBackAsync();
        });
    }
}