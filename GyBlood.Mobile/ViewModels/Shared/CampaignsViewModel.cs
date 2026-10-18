using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Navigation;

namespace GyBlood.Mobile.ViewModels.Shared;

/// <summary>Onglet « Campagnes » du patient : toutes les campagnes de don.</summary>
public sealed class CampaignsViewModel : CampaignListViewModelBase
{
    public CampaignsViewModel(ICampaignApi api, INavigationService navigation)
        : base(api, navigation)
    {
        Title = "Campagnes";
    }
}
