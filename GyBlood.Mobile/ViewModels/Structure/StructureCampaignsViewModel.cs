using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Auth;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Shared;

namespace GyBlood.Mobile.ViewModels.Structure;

/// <summary>Campagnes publiées par la structure connectée. (US-24)</summary>
public sealed partial class StructureCampaignsViewModel : CampaignListViewModelBase
{
    private readonly IAuthService _auth;
    private readonly INavigationService _navigation;

    public StructureCampaignsViewModel(ICampaignApi api, INavigationService navigation, IAuthService auth)
        : base(api, navigation)
    {
        _auth = auth;
        _navigation = navigation;
        Title = "Mes campagnes";
    }

    protected override bool Keep(CampaignDto campaign) => campaign.StructureId == _auth.CurrentUser?.Id;

    [RelayCommand]
    private Task CreateAsync() => _navigation.GoToAsync(AppRoutes.CampaignEditor);
}
