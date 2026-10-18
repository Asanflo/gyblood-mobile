using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Core.Helpers;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Auth;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Donor;

/// <summary>Accueil du donneur : prochain don possible, demandes en attente, accès rapides, derniers dons et campagnes.</summary>
public partial class DonorHomeViewModel : BaseViewModel
{
    private readonly IAuthService _auth;
    private readonly IDonorApi _donorApi;
    private readonly IAppointmentApi _appointmentApi;
    private readonly ICampaignApi _campaignApi;
    private readonly INavigationService _navigation;

    [ObservableProperty]
    private string greeting = "Bonjour";

    [ObservableProperty]
    private string bannerTitle = "Merci d'être donneur";

    [ObservableProperty]
    private string bannerSubtitle = string.Empty;

    [ObservableProperty]
    private string availabilityText = "Indisponible";

    [ObservableProperty]
    private string requestsText = "Aucune demande en attente";

    [ObservableProperty]
    private bool profileIncomplete;

    [ObservableProperty]
    private bool hasDonations;

    [ObservableProperty]
    private bool hasCampaigns;

    public DonorHomeViewModel(
        IAuthService auth,
        IDonorApi donorApi,
        IAppointmentApi appointmentApi,
        ICampaignApi campaignApi,
        INavigationService navigation)
    {
        _auth = auth;
        _donorApi = donorApi;
        _appointmentApi = appointmentApi;
        _campaignApi = campaignApi;
        _navigation = navigation;
        Title = "Accueil";
    }

    public ObservableCollection<DonationItem> RecentDonations { get; } = [];

    public ObservableCollection<CampaignItem> Campaigns { get; } = [];

    public override async Task OnAppearingAsync()
    {
        var user = _auth.CurrentUser;
        var name = !string.IsNullOrWhiteSpace(user?.DisplayName) ? user!.DisplayName : user?.Email.Split('@')[0];
        Greeting = string.IsNullOrWhiteSpace(name) ? "Bonjour" : $"Bonjour, {name}";

        await RunAsync(async () =>
        {
            var profile = await _donorApi.GetMeAsync();
            ProfileIncomplete = profile.BloodGroup is null || profile.Rhesus is null || string.IsNullOrWhiteSpace(profile.FullName);
            AvailabilityText = profile.IsAvailable
                ? (profile.ShareLocation ? "Disponible et visible sur la carte" : "Disponible (position non partagée)")
                : "Indisponible";

            var eligibility = await _donorApi.GetEligibilityAsync();
            BannerTitle = eligibility.CanDonate ? "Vous pouvez donner" : "Prochain don possible";
            BannerSubtitle = eligibility.CanDonate
                ? "Votre sang peut sauver une vie."
                : DateHelper.NextDonationText(eligibility.NextDonationDate);

            var page = await _appointmentApi.GetAppointmentsAsync(AppointmentStatus.Requested, size: 50);
            var count = page.Content.Count;
            RequestsText = count switch
            {
                0 => "Aucune demande en attente",
                1 => "1 demande en attente",
                _ => $"{count} demandes en attente",
            };

            await LoadRecentDonationsAsync();
            await LoadCampaignsAsync();
        });
    }

    private async Task LoadRecentDonationsAsync()
    {
        var page = await _donorApi.GetDonationsAsync(size: 3);
        RecentDonations.Clear();
        foreach (var donation in page.Content)
        {
            RecentDonations.Add(new DonationItem
            {
                StructureName = donation.StructureName,
                DateText = Ui.Day(donation.DonatedAt),
                VolumeText = donation.VolumeMl is { } ml ? $"{ml} ml" : string.Empty,
            });
        }

        HasDonations = RecentDonations.Count > 0;
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
    private Task OpenRequestsAsync() => _navigation.GoToAsync(AppRoutes.Tab(AppRoutes.DonorRequests));

    [RelayCommand]
    private Task OpenAvailabilityAsync() => _navigation.GoToAsync(AppRoutes.Availability);

    [RelayCommand]
    private Task OpenCampaignsAsync() => _navigation.GoToAsync(AppRoutes.Tab(AppRoutes.DonorCampaigns));

    [RelayCommand]
    private Task OpenHistoryAsync() => _navigation.GoToAsync(AppRoutes.Tab(AppRoutes.DonorHistory));

    [RelayCommand]
    private Task OpenNotificationsAsync() => _navigation.GoToAsync(AppRoutes.Notifications);

    [RelayCommand]
    private Task OpenProfileAsync() => _navigation.GoToAsync(AppRoutes.Tab(AppRoutes.DonorProfile));
}