using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Auth;
using GyBlood.Mobile.Services.Dialogs;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Shared;

/// <summary>Détail d'une campagne. Le donneur peut s'inscrire ; le sponsor peut la financer. (US-28, US-33)</summary>
[QueryProperty(nameof(Campaign), "campaign")]
public partial class CampaignDetailViewModel : BaseViewModel
{
    private readonly ICampaignApi _api;
    private readonly IAuthService _auth;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogs;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CampaignTitle), nameof(StructureName), nameof(PeriodText), nameof(Location), nameof(TargetText), nameof(FundsText), nameof(HasFunds), nameof(DescriptionText), nameof(IsJoined), nameof(CanJoin), nameof(CanLeave))]
    private CampaignDto? campaign;

    public CampaignDetailViewModel(ICampaignApi api, IAuthService auth, INavigationService navigation, IDialogService dialogs)
    {
        _api = api;
        _auth = auth;
        _navigation = navigation;
        _dialogs = dialogs;
        Title = "Campagne";
    }

    public string CampaignTitle => Campaign?.Title ?? string.Empty;

    public string StructureName => Campaign?.StructureName ?? string.Empty;

    public string Location => Campaign?.Location ?? string.Empty;

    public string PeriodText => Campaign is null
        ? string.Empty
        : Campaign.StartDate.Date == Campaign.EndDate.Date
            ? Ui.Day(Campaign.StartDate)
            : $"Du {Ui.Day(Campaign.StartDate)} au {Ui.Day(Campaign.EndDate)}";

    public string TargetText => Campaign is null ? string.Empty : $"Objectif : {Campaign.TargetDonations} dons";

    public string FundsText => Campaign is { FundsReceived: > 0 } ? $"Financement reçu : {Ui.Money(Campaign.FundsReceived)}" : string.Empty;

    public bool HasFunds => !string.IsNullOrEmpty(FundsText);

    public string DescriptionText => string.IsNullOrWhiteSpace(Campaign?.Description) ? "Pas de description." : Campaign!.Description!;

    public bool IsJoined => Campaign?.Joined == true;

    public bool CanJoin => _auth.CurrentUser?.Role == UserRole.Donor && Campaign is { Joined: false };

    public bool CanLeave => _auth.CurrentUser?.Role == UserRole.Donor && Campaign is { Joined: true };

    public bool CanFund => _auth.CurrentUser?.Role == UserRole.Sponsor;

    [RelayCommand]
    private Task JoinAsync() => RunAsync(async () =>
    {
        if (Campaign is null)
        {
            return;
        }

        await _api.JoinAsync(Campaign.Id);
        Campaign = Campaign with { Joined = true };
        await _dialogs.ToastAsync("Inscription enregistrée");
    });

    [RelayCommand]
    private Task LeaveAsync() => RunAsync(async () =>
    {
        if (Campaign is null)
        {
            return;
        }

        await _api.LeaveAsync(Campaign.Id);
        Campaign = Campaign with { Joined = false };
    });

    [RelayCommand]
    private async Task FundAsync()
    {
        if (Campaign is null)
        {
            return;
        }

        await _navigation.GoToAsync(AppRoutes.Funding, new Dictionary<string, object> { ["campaign"] = Campaign });
    }
}
