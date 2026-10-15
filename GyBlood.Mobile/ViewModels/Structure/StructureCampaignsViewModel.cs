using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Structure;

/// <summary>Campagnes : Campagnes de la structure et fonds reçus. (US-32)</summary>
public partial class StructureCampaignsViewModel : BaseViewModel
{
    public StructureCampaignsViewModel()
    {
        Title = "Campagnes";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
