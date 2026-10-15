using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Patient;

/// <summary>Stocks de sang : Liste filtrable par groupe et rhésus, distance, date de mise à jour. (US-10)</summary>
public partial class StocksViewModel : BaseViewModel
{
    public StocksViewModel()
    {
        Title = "Stocks de sang";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
