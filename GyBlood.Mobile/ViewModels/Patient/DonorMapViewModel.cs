using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Patient;

/// <summary>Carte des donneurs : Carte OpenStreetMap, rayon réglable, marqueurs des donneurs compatibles connectés. (US-12)</summary>
public partial class DonorMapViewModel : BaseViewModel
{
    public DonorMapViewModel()
    {
        Title = "Carte des donneurs";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
