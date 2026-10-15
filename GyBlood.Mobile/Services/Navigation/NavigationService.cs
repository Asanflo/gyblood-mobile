using CommunityToolkit.Mvvm.Messaging;
using GyBlood.Mobile.Services.Auth;
using GyBlood.Mobile.Services.Presence;

namespace GyBlood.Mobile.Services.Navigation;

public sealed class NavigationService : INavigationService, IRecipient<SessionExpiredMessage>
{
    private readonly RoleShellSelector _selector;
    private readonly PresenceService _presence;

    public NavigationService(RoleShellSelector selector, PresenceService presence)
    {
        _selector = selector;
        _presence = presence;
        WeakReferenceMessenger.Default.RegisterAll(this);
    }

    public async Task ShowRoleShellAsync(UserDto? user)
    {
        var window = Application.Current?.Windows.FirstOrDefault();
        if (window is null)
        {
            return;
        }

        var (shell, startRoute) = _selector.Resolve(user);
        window.Page = shell;

        if (startRoute is not null)
        {
            await Task.Yield();
            await Shell.Current.GoToAsync(startRoute);
        }

        if (user is { Role: UserRole.Donor, Status: AccountStatus.Validated })
        {
            _presence.Start();
        }
        else
        {
            _presence.Stop();
        }
    }

    public Task GoToAsync(string route, IDictionary<string, object>? parameters = null)
        => parameters is null
            ? Shell.Current.GoToAsync(route)
            : Shell.Current.GoToAsync(route, new ShellNavigationQueryParameters(parameters));

    public Task GoBackAsync() => Shell.Current.GoToAsync("..");

    public void Receive(SessionExpiredMessage message)
        => MainThread.BeginInvokeOnMainThread(async () => await ShowRoleShellAsync(null));
}
