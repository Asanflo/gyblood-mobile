using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Auth;

/// <summary>Choix du rôle : Affiché après une première connexion Google : choisir patient, donneur, banque de sang ou sponsor. (US-02)</summary>
public partial class RoleChoiceViewModel : BaseViewModel
{
    public RoleChoiceViewModel()
    {
        Title = "Choix du rôle";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
