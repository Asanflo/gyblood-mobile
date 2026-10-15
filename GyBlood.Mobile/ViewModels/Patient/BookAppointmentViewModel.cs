using CommunityToolkit.Mvvm.ComponentModel;
using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Patient;

/// <summary>Réservation : Hôpital le plus proche proposé par défaut, choix du créneau. (US-14)</summary>
public partial class BookAppointmentViewModel : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    private EntityId? itemId;

    public BookAppointmentViewModel()
    {
        Title = "Réservation";
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
