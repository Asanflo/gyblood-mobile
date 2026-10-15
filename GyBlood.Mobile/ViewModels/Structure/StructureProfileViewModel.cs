using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Structure;

/// <summary>Profil de la structure : Nom, adresse, position, téléphone, justificatif. (US-30)</summary>
public partial class StructureProfileViewModel : BaseViewModel
{
    public StructureProfileViewModel()
    {
        Title = "Profil de la structure";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
