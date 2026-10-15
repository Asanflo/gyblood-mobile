namespace GyBlood.Mobile.Services.Notifications;

public interface IPushService
{
    /// <summary>Récupère le jeton FCM de l'appareil et l'envoie à POST /me/devices.</summary>
    Task InitializeAsync();
}
