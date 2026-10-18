using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Auth;
using GyBlood.Mobile.Services.Dialogs;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Sponsor;

/// <summary>Profil du sponsor : organisation et contact. (US-32)</summary>
public partial class SponsorProfileViewModel : BaseViewModel
{
    private readonly ISponsorApi _api;
    private readonly IAuthService _auth;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogs;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string organisation = string.Empty;

    [ObservableProperty]
    private string contact = string.Empty;

    public SponsorProfileViewModel(ISponsorApi api, IAuthService auth, INavigationService navigation, IDialogService dialogs)
    {
        _api = api;
        _auth = auth;
        _navigation = navigation;
        _dialogs = dialogs;
        Title = "Mon profil";
    }

    public override Task OnAppearingAsync()
    {
        Email = _auth.CurrentUser?.Email ?? string.Empty;
        if (string.IsNullOrEmpty(Organisation))
        {
            Organisation = _auth.CurrentUser?.DisplayName ?? string.Empty;
        }

        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Organisation) || string.IsNullOrWhiteSpace(Contact))
        {
            ErrorMessage = "Renseignez le nom de l'organisation et un contact.";
            return;
        }

        await RunAsync(async () =>
        {
            await _api.UpdateMeAsync(new UpdateSponsorRequest(Organisation.Trim(), Contact.Trim(), null));
            await _dialogs.ToastAsync("Profil enregistré");
        });
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _auth.LogoutAsync();
        await _navigation.ShowRoleShellAsync(null);
    }
}
