using GyBlood.Mobile.Core.Constants;
using Refit;

namespace GyBlood.Mobile.Services.Api;


/// <summary>Routes publiques : appelées sans jeton (client Refit enregistré sans AuthHeaderHandler).</summary>
public interface IAuthApi
{
    [Post(ApiRoutes.Auth.Register)]
    Task<AuthResponse> RegisterAsync([Body] RegisterRequest request);

    [Post(ApiRoutes.Auth.Login)]
    Task<AuthResponse> LoginAsync([Body] LoginRequest request);

    [Post(ApiRoutes.Auth.Google)]
    Task<AuthResponse> GoogleAsync([Body] GoogleLoginRequest request);

    [Post(ApiRoutes.Auth.GoogleExchange)]
    Task<AuthResponse> ExchangeGoogleCodeAsync([Body] GoogleExchangeRequest request);

    [Post(ApiRoutes.Auth.Refresh)]
    Task<AuthResponse> RefreshAsync([Body] RefreshRequest request);
}
