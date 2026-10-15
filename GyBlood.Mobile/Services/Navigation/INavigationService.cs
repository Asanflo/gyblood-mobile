namespace GyBlood.Mobile.Services.Navigation;

public interface INavigationService
{
    /// <summary>Remplace le Shell courant par celui du rôle (ou par l'authentification si user est null).</summary>
    Task ShowRoleShellAsync(UserDto? user);

    Task GoToAsync(string route, IDictionary<string, object>? parameters = null);

    Task GoBackAsync();
}
