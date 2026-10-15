using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Patient;

/// <summary>Mes rendez-vous : Liste par statut, annulation. (US-16, US-17)</summary>
public partial class MyAppointmentsViewModel : BaseViewModel
{
    public MyAppointmentsViewModel()
    {
        Title = "Mes rendez-vous";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
