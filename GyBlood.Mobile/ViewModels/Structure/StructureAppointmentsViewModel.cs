using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Structure;

/// <summary>Rendez-vous : Liste des rendez-vous, confirmation du don, absence. (US-33)</summary>
public partial class StructureAppointmentsViewModel : BaseViewModel
{
    public StructureAppointmentsViewModel()
    {
        Title = "Rendez-vous";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
