namespace GyBlood.Mobile.Services.Notifications;

/// <summary>
/// À FAIRE (semaine 2) : intégrer Firebase Cloud Messaging pour Android.
/// 1. Ajouter un plugin Firebase pour MAUI et copier google-services.json dans Platforms/Android/.
/// 2. Demander la permission POST_NOTIFICATIONS (Android 13 et plus).
/// 3. Récupérer le jeton FCM et appeler IMeApi.RegisterDeviceAsync(new RegisterDeviceRequest(token)).
/// 4. Réagir au toucher d'une notification : ouvrir AppRoutes.RequestDetail avec l'identifiant du rendez-vous.
/// </summary>
public sealed class PushService : IPushService
{
    public Task InitializeAsync() => Task.CompletedTask;
}
