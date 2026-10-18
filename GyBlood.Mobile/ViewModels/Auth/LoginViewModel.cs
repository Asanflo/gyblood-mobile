using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Core.Helpers;
using GyBlood.Mobile.Services.Auth;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Auth;

public partial class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _auth;
    private readonly INavigationService _navigation;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    public LoginViewModel(IAuthService auth, INavigationService navigation)
    {
        _auth = auth;
        _navigation = navigation;
        Title = "Connexion";
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (!InputValidator.IsValidEmail(Email))
        {
            ErrorMessage = "Adresse email invalide.";
            return;
        }

        if (string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Saisissez votre mot de passe.";
            return;
        }

        await RunAsync(async () =>
        {
            var user = await _auth.LoginAsync(Email.Trim(), Password);
            await _navigation.ShowRoleShellAsync(user);
        });
    }

    [RelayCommand]
    private async Task LoginWithGoogleAsync()
    {
        await RunAsync(async () =>
        {
            var user = await _auth.LoginWithGoogleAsync();
            if (user is not null)
            {
                await _navigation.ShowRoleShellAsync(user);
            }
        });
    }

    /// <summary>« Créer un compte » : on commence par le choix du profil.</summary>
    [RelayCommand]
    private Task GoToRegisterAsync() => _navigation.GoToAsync(AppRoutes.RoleChoice);
}
