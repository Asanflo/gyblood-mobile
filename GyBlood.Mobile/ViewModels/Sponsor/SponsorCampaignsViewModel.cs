using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Shared;

namespace GyBlood.Mobile.ViewModels.Sponsor;

/// <summary>Campagnes à financer, vues par le sponsor. (US-33)</summary>
public sealed class SponsorCampaignsViewModel : CampaignListViewModelBase
{
    public SponsorCampaignsViewModel(ICampaignApi api, INavigationService navigation)
        : base(api, navigation)
    {
        Title = "Campagnes à financer";
    }
}
