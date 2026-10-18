using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Shared;

namespace GyBlood.Mobile.ViewModels.Donor;

/// <summary>Campagnes vues par le donneur (celles où il est inscrit portent la mention « Inscrit »). (US-28)</summary>
public sealed class DonorCampaignsViewModel : CampaignListViewModelBase
{
    public DonorCampaignsViewModel(ICampaignApi api, INavigationService navigation)
        : base(api, navigation)
    {
        Title = "Campagnes";
    }
}
