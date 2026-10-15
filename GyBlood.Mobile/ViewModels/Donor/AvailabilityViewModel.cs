using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Donor;

/// <summary>Disponibilité : Interrupteur « disponible » et consentement à la géolocalisation. (US-27)</summary>
public partial class AvailabilityViewModel : BaseViewModel
{
    public AvailabilityViewModel()
    {
        Title = "Disponibilité";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
