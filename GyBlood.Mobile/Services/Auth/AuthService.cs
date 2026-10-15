using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Core.Extensions;
using GyBlood.Mobile.Services.Api;
using Refit;

namespace GyBlood.Mobile.Services.Auth;

public sealed class AuthService : IAuthService
{
    private readonly IAuthApi _authApi;
    private readonly IMeApi _meApi;
    private readonly TokenStore _tokens;

    public AuthService(IAuthApi authApi, IMeApi meApi, TokenStore tokens)
    {
        _authApi = authApi;
        _meApi = meApi;
        _tokens = tokens;
    }

    public UserDto? CurrentUser { get; private set; }

    public async Task<UserDto> LoginAsync(string email, string password)
        => await StoreAsync(await _authApi.LoginAsync(new LoginRequest(email, password)));

    public async Task<UserDto> RegisterAsync(string email, string password, UserRole role)
        => await StoreAsync(await _authApi.RegisterAsync(new RegisterRequest(email, password, role)));

    /// <summary>
    /// Connexion Google avec PKCE : l'application garde un verifier secret et n'envoie que son empreinte (challenge).
    /// Le backend redirige vers gyblood://auth?code=... (ou ?error=...) ; le code est ensuite échangé contre les jetons
    /// avec le verifier, donc un code intercepté par une autre application est inutilisable.
    /// </summary>
    public async Task<UserDto?> LoginWithGoogleAsync(UserRole? role = null)
    {
        var verifier = NewCodeVerifier();
        var url = $"{AppConstants.GoogleAuthUrl}?code_challenge={Uri.EscapeDataString(CodeChallenge(verifier))}";
        if (role is not null)
        {
            url += $"&role={Uri.EscapeDataString(role.Value.ToString().ToUpperInvariant())}";
        }

        try
        {
            var result = await WebAuthenticator.Default.AuthenticateAsync(new Uri(url), new Uri(AppConstants.OAuthCallbackUrl));
            if (result.Properties.TryGetValue("error", out var error))
            {
                throw new InvalidOperationException(GoogleErrorMessage(error));
            }

            if (!result.Properties.TryGetValue("code", out var code) || string.IsNullOrEmpty(code))
            {
                throw new InvalidOperationException("Réponse de connexion Google incomplète.");
            }

            return await StoreAsync(await _authApi.ExchangeGoogleCodeAsync(new GoogleExchangeRequest(code, verifier)));
        }
        catch (TaskCanceledException)
        {
            return null; // l'utilisateur a fermé le navigateur
        }
    }

    private static string NewCodeVerifier() => Base64Url(System.Security.Cryptography.RandomNumberGenerator.GetBytes(32));

    private static string CodeChallenge(string verifier)
        => Base64Url(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.ASCII.GetBytes(verifier)));

    private static string Base64Url(byte[] bytes) => Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    private static string GoogleErrorMessage(string error) => error switch
    {
        "account_exists" => "Un compte existe déjà avec cet email. Connectez-vous avec votre mot de passe.",
        "google_email_unverified" => "L'adresse email de ce compte Google n'est pas vérifiée.",
        "access_denied" => "Connexion Google annulée.",
        _ => "La connexion Google a échoué. Réessayez.",
    };

    public async Task<UserDto?> RestoreSessionAsync()
    {
        if (string.IsNullOrEmpty(await _tokens.GetAccessTokenAsync()))
        {
            return null;
        }

        try
        {
            CurrentUser = await _meApi.GetMeAsync();
            return CurrentUser;
        }
        catch (ApiException ex) when (ex.StatusCode is System.Net.HttpStatusCode.Unauthorized or System.Net.HttpStatusCode.Forbidden)
        {
            _tokens.Clear();
            CurrentUser = null;
            return null;
        }
        catch (HttpRequestException)
        {
            // Hors ligne au démarrage : on garde les jetons et on revient à la connexion sans les effacer.
            return null;
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            var refresh = await _tokens.GetRefreshTokenAsync();
            if (!string.IsNullOrEmpty(refresh))
            {
                await _meApi.LogoutAsync(new RefreshRequest(refresh));
            }
        }
        catch (Exception)
        {
            // Déconnexion locale quoi qu'il arrive.
        }
        finally
        {
            _tokens.Clear();
            CurrentUser = null;
        }
    }

    private async Task<UserDto> StoreAsync(AuthResponse response)
    {
        await _tokens.SaveAsync(response.AccessToken, response.RefreshToken);
        CurrentUser = response.User;
        return response.User;
    }
}
