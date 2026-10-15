using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Admin;

/// <summary>Paramètres : Délai minimal entre deux dons par sexe et type de don. (US-53)</summary>
public partial class SettingsViewModel : BaseViewModel
{
    public SettingsViewModel()
    {
        Title = "Paramètres";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
