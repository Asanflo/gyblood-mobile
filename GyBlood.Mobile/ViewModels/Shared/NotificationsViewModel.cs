using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Shared;

/// <summary>Notifications : Notifications reçues, marquer comme lue. (US-22)</summary>
public partial class NotificationsViewModel : BaseViewModel
{
    public NotificationsViewModel()
    {
        Title = "Notifications";
    }

    public override Task OnAppearingAsync()
    {
        // À FAIRE : charger les données via le service ou l'interface Refit concernée.
        return Task.CompletedTask;
    }
}
