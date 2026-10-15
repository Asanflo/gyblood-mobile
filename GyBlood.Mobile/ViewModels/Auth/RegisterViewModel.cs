using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Helpers;
using GyBlood.Mobile.Services.Auth;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Auth;

public sealed record RoleOption(UserRole Role, string Label);

public partial class RegisterViewModel : BaseViewModel
{
    private readonly IAuthService _auth;
    private readonly INavigationService _navigation;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string confirmPassword = string.Empty;

    [ObservableProperty]
    private RoleOption selectedRole;

    public IReadOnlyList<RoleOption> Roles { get; } =
    [
        new(UserRole.Patient, "Patient"),
        new(UserRole.Donor, "Donneur"),
        new(UserRole.Structure, "Banque de sang / hôpital"),
        new(UserRole.Sponsor, "Sponsor"),
    ];

    public RegisterViewModel(IAuthService auth, INavigationService navigation)
    {
        _auth = auth;
        _navigation = navigation;
        selectedRole = Roles[0];
        Title = "Créer un compte";
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (!InputValidator.IsValidEmail(Email))
        {
            ErrorMessage = "Adresse email invalide.";
            return;
        }

        if (!InputValidator.IsValidPassword(Password))
        {
            ErrorMessage = $"Le mot de passe doit contenir au moins {InputValidator.MinPasswordLength} caractères.";
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Les mots de passe ne correspondent pas.";
            return;
        }

        await RunAsync(async () =>
        {
            var user = await _auth.RegisterAsync(Email.Trim(), Password, SelectedRole.Role);
            await _navigation.ShowRoleShellAsync(user);
        });
    }

    [RelayCommand]
    private async Task RegisterWithGoogleAsync()
    {
        await RunAsync(async () =>
        {
            var user = await _auth.LoginWithGoogleAsync(SelectedRole.Role);
            if (user is not null)
            {
                await _navigation.ShowRoleShellAsync(user);
            }
        });
    }

    [RelayCommand]
    private Task BackToLoginAsync() => _navigation.GoBackAsync();
}
