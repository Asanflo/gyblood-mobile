using CommunityToolkit.Mvvm.ComponentModel;
using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Admin;

/// <summary>Détail d'un compte : Document de groupe sanguin, valider, refuser, suspendre. (US-50, US-51, US-52)</summary>
public partial class AccountDetailViewModel : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    private EntityId? itemId;

    public AccountDetailViewModel()
    {
        Title = "Détail d'un compte";
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
