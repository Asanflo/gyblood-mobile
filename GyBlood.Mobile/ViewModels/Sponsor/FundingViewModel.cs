using CommunityToolkit.Mvvm.ComponentModel;
using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Sponsor;

/// <summary>Financement : Montant, paiement MoMo ou Orange Money, reçu. (US-42)</summary>
public partial class FundingViewModel : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    private EntityId? itemId;

    public FundingViewModel()
    {
        Title = "Financement";
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
