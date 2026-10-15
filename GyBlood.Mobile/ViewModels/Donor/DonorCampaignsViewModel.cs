using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Donor;

/// <summary>Campagnes : Campagnes ouvertes et campagnes auxquelles j'ai adhéré. (US-25)</summary>
public partial class DonorCampaignsViewModel : BaseViewModel
{
    public DonorCampaignsViewModel()
    {
        Title = "Campagnes";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
