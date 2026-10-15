using CommunityToolkit.Mvvm.ComponentModel;
using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Patient;

/// <summary>Paiement : Montant, frais de la plateforme, opérateur MoMo ou Orange Money. (US-15)</summary>
public partial class PaymentViewModel : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    private EntityId? itemId;

    public PaymentViewModel()
    {
        Title = "Paiement";
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
