using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Sponsor;

/// <summary>Campagnes : Campagnes à financer avec objectif et fonds reçus. (US-41)</summary>
public partial class SponsorCampaignsViewModel : BaseViewModel
{
    public SponsorCampaignsViewModel()
    {
        Title = "Campagnes";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
