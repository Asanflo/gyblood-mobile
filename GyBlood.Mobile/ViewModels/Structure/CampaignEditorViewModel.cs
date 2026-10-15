using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Structure;

/// <summary>Nouvelle campagne : Titre, lieu, dates, objectif, description. (US-32)</summary>
public partial class CampaignEditorViewModel : BaseViewModel
{
    public CampaignEditorViewModel()
    {
        Title = "Nouvelle campagne";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
