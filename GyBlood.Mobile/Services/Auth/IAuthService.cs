namespace GyBlood.Mobile.Services.Auth;

public interface IAuthService
{
    UserDto? CurrentUser { get; }

    Task<UserDto> LoginAsync(string email, string password);

    Task<UserDto> RegisterAsync(string email, string password, UserRole role, string? displayName = null);

    /// <summary>Connexion Google via le navigateur. Renvoie null si l'utilisateur annule.</summary>
    Task<UserDto?> LoginWithGoogleAsync(UserRole? role = null);

    /// <summary>Relit le compte avec le jeton stocké. Renvoie null s'il n'y a pas de session valide.</summary>
    Task<UserDto?> RestoreSessionAsync();

    Task LogoutAsync();
}