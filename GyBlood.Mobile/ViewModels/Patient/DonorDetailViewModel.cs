using CommunityToolkit.Mvvm.ComponentModel;
using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Patient;

/// <summary>Détail d'un donneur : Nom, groupe, rhésus, distance et bouton « Demander un don ». (US-13)</summary>
public partial class DonorDetailViewModel : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    private EntityId? itemId;

    public DonorDetailViewModel()
    {
        Title = "Détail d'un donneur";
    }

    /// <summary>Reçoit ?id=... depuis la navigation (voir INavigationService.GoToAsync).</summary>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var value) && EntityId.TryParse(value?.ToString(), out var id))
        {
            ItemId = id;
        }
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
