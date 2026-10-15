using GyBlood.Mobile.Services.Api;

namespace GyBlood.Mobile.Tests.Fakes;

/// <summary>Faux backend d'authentification : permet de développer et tester sans serveur.</summary>
public sealed class FakeAuthApi : IAuthApi
{
    public static UserDto User(UserRole role = UserRole.Patient, AccountStatus status = AccountStatus.Validated)
        => new(1, "demo@gyblood.test", role, status, "Compte de démonstration", null);

    public Task<AuthResponse> RegisterAsync(RegisterRequest request)
        => Task.FromResult(new AuthResponse("fake-access", "fake-refresh",
            User(request.Role, request.Role is UserRole.Donor or UserRole.Structure ? AccountStatus.Pending : AccountStatus.Validated)));

    public Task<AuthResponse> LoginAsync(LoginRequest request)
        => request.Password == "wrong"
            ? throw new InvalidOperationException("Identifiants incorrects")
            : Task.FromResult(new AuthResponse("fake-access", "fake-refresh", User()));

    public Task<AuthResponse> GoogleAsync(GoogleLoginRequest request)
        => Task.FromResult(new AuthResponse("fake-access", "fake-refresh", User()));

    public Task<AuthResponse> ExchangeGoogleCodeAsync(GoogleExchangeRequest request)
        => Task.FromResult(new AuthResponse("fake-access", "fake-refresh", User()));

    public Task<AuthResponse> RefreshAsync(RefreshRequest request)
        => Task.FromResult(new AuthResponse("fake-access-2", "fake-refresh-2", User()));
}
