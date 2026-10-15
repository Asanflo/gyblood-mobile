using GyBlood.Mobile.Services.Auth;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Shared;

public partial class StartupViewModel : BaseViewModel
{
    private readonly IAuthService _auth;
    private readonly INavigationService _navigation;

    public StartupViewModel(IAuthService auth, INavigationService navigation)
    {
        _auth = auth;
        _navigation = navigation;
    }

    public override async Task OnAppearingAsync()
    {
        var user = await _auth.RestoreSessionAsync();
        await _navigation.ShowRoleShellAsync(user);
    }
}
