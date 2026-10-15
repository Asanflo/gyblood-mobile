using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Donor;

/// <summary>Demandes : Demandes reçues : accepter ou refuser. (US-22, US-23)</summary>
public partial class RequestsViewModel : BaseViewModel
{
    public RequestsViewModel()
    {
        Title = "Demandes";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
