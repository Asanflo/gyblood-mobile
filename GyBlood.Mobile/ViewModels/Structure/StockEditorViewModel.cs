using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Structure;

/// <summary>Stocks : Saisie des quantités par groupe sanguin et rhésus. (US-31)</summary>
public partial class StockEditorViewModel : BaseViewModel
{
    public StockEditorViewModel()
    {
        Title = "Stocks";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
