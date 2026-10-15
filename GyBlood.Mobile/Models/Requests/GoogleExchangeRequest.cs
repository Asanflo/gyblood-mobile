namespace GyBlood.Mobile.Models.Requests;

/// <summary>Échange du code reçu par gyblood://auth?code=... contre des jetons (PKCE : le verifier n'a jamais quitté l'appareil).</summary>
public sealed record GoogleExchangeRequest(
    string Code,
    string CodeVerifier);
