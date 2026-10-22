using GyBlood.Mobile.Models.Responses;
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
        UserDto? user = null;
        try
        {
            user = await _auth.RestoreSessionAsync();
        }
        catch (Exception ex)
        {
            // Jeton périmé (401) ou serveur injoignable : on revient à l'accueil au lieu de planter.
            System.Diagnostics.Debug.WriteLine($"[Startup] Restauration de session impossible : {ex}");
        }

        await _navigation.ShowRoleShellAsync(user);
    }
}