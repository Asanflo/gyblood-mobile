using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Sponsor;

/// <summary>Mon profil : Organisation, contact, logo. (US-40)</summary>
public partial class SponsorProfileViewModel : BaseViewModel
{
    public SponsorProfileViewModel()
    {
        Title = "Mon profil";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
