using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Services.Auth;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Auth;

public partial class PendingAccountViewModel : BaseViewModel
{
    private readonly IAuthService _auth;
    private readonly INavigationService _navigation;

    [ObservableProperty]
    private string statusMessage = string.Empty;

    [ObservableProperty]
    private string headline = "Compte en attente";

    [ObservableProperty]
    private bool canEditProfile;

    public PendingAccountViewModel(IAuthService auth, INavigationService navigation)
    {
        _auth = auth;
        _navigation = navigation;
        Title = "Compte en attente";
    }

    public override Task OnAppearingAsync()
    {
        UpdateMessage(_auth.CurrentUser);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await RunAsync(async () =>
        {
            var user = await _auth.RestoreSessionAsync();
            UpdateMessage(user);
            if (user is { Status: AccountStatus.Validated })
            {
                await _navigation.ShowRoleShellAsync(user);
            }
        });
    }

    [RelayCommand]
    private async Task EditProfileAsync()
    {
        var route = _auth.CurrentUser?.Role switch
        {
            UserRole.Donor => AppRoutes.CompleteDonorProfile,
            UserRole.Structure => AppRoutes.CompleteStructureProfile,
            _ => null,
        };

        if (route is not null)
        {
            await _navigation.GoToAsync(route);
        }
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _auth.LogoutAsync();
        await _navigation.ShowRoleShellAsync(null);
    }

    private void UpdateMessage(UserDto? user)
    {
        if (user is null)
        {
            StatusMessage = string.Empty;
            CanEditProfile = false;
            return;
        }

        CanEditProfile = user.Role is UserRole.Donor or UserRole.Structure
                         && user.Status is AccountStatus.Pending or AccountStatus.Rejected;

        Headline = user.Status switch
        {
            AccountStatus.Rejected => "Compte refusé",
            AccountStatus.Suspended => "Compte suspendu",
            _ => "Compte en attente",
        };

        StatusMessage = user.Status switch
        {
            AccountStatus.Pending => "Votre compte est en attente de validation par un administrateur. Vous serez notifié dès que ce sera fait.",
            AccountStatus.Rejected => string.IsNullOrWhiteSpace(user.RejectionReason)
                ? "Votre compte a été refusé."
                : $"Votre compte a été refusé. Motif : {user.RejectionReason}",
            AccountStatus.Suspended => "Votre compte est suspendu. Contactez l'administrateur.",
            _ => string.Empty,
        };
    }
}
