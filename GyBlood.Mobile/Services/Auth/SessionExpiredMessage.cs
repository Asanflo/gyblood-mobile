namespace GyBlood.Mobile.Services.Auth;

/// <summary>Publié quand le jeton ne peut plus être renouvelé : l'application revient à l'écran de connexion.</summary>
public sealed record SessionExpiredMessage;
