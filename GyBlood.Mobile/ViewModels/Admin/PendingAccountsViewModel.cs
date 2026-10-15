using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Admin;

/// <summary>Comptes à valider : Comptes donneurs et structures en attente. (US-50, US-51)</summary>
public partial class PendingAccountsViewModel : BaseViewModel
{
    public PendingAccountsViewModel()
    {
        Title = "Comptes à valider";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
