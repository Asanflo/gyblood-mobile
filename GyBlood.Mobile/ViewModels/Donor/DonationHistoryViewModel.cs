using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Donor;

/// <summary>Historique des dons : Dons passés et date du prochain don possible. (US-24, US-26)</summary>
public partial class DonationHistoryViewModel : BaseViewModel
{
    public DonationHistoryViewModel()
    {
        Title = "Historique des dons";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
