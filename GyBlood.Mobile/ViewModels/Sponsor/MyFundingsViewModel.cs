using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Sponsor;

/// <summary>Mes financements : Historique : montant, campagne, date. (US-43)</summary>
public partial class MyFundingsViewModel : BaseViewModel
{
    public MyFundingsViewModel()
    {
        Title = "Mes financements";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
