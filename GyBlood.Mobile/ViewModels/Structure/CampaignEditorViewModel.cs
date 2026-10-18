using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

    public CampaignEditorViewModel(ICampaignApi api, INavigationService navigation, IDialogService dialogs)
    {
        _api = api;
        _navigation = navigation;
        _dialogs = dialogs;
        Title = "Nouvelle campagne";
    }

    public DateTime MinimumDate => DateTime.Today;

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

            await _dialogs.ToastAsync("Campagne publiée");
            await _navigation.GoBackAsync();
        });
    }
}
