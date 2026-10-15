using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Donor;

/// <summary>Mon profil de donneur : Identité, antécédents, document de groupe sanguin, statut de validation. (US-20, US-21)</summary>
public partial class DonorProfileViewModel : BaseViewModel
{
    public DonorProfileViewModel()
    {
        Title = "Mon profil de donneur";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
