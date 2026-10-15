namespace GyBlood.Mobile.Core.Constants;

public static class AppConstants
{
    /// <summary>10.0.2.2 = le localhost de votre PC vu depuis l'émulateur Android. Sur un vrai téléphone : l'IP du PC ou l'URL déployée.</summary>
    public const string ApiBaseUrl = "http://10.0.2.2:8080/api/v1/";

    /// <summary>Point d'entrée OAuth côté backend FastAPI : /oauth2/authorization/google?code_challenge=...&amp;role=...</summary>
    public const string GoogleAuthUrl = "http://10.0.2.2:8080/oauth2/authorization/google";

    /// <summary>Redirection finale du backend vers l'application (schéma déclaré dans WebAuthenticationCallbackActivity).</summary>
    public const string OAuthCallbackUrl = "gyblood://auth";

    public const int ClientTimeoutSeconds = 20;
    public const double DefaultSearchRadiusKm = 10;
    public const double MaxSearchRadiusKm = 20;
    public const long MaxDocumentBytes = 5 * 1024 * 1024;
    public const int DefaultPageSize = 20;

    /// <summary>Un donneur est « connecté » s'il a envoyé un signal depuis moins de 5 minutes (RG11).</summary>
    public static readonly TimeSpan HeartbeatInterval = TimeSpan.FromMinutes(2);
}
