using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Patient;

/// <summary>Mon profil : Groupe sanguin, rhésus et position (GPS ou point posé sur la carte). (US-11)</summary>
public partial class PatientProfileViewModel : BaseViewModel
{
    public PatientProfileViewModel()
    {
        Title = "Mon profil";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
