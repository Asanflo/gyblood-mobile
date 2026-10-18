using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Auth;
using GyBlood.Mobile.Services.Dialogs;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Admin;

/// <summary>Paramètres : délai minimal entre deux dons (à faire valider d'après le CNTS). (US-39)</summary>
public partial class SettingsViewModel : BaseViewModel
{
    private readonly IAdminApi _api;
    private readonly IAuthService _auth;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogs;

    public SettingsViewModel(IAdminApi api, IAuthService auth, INavigationService navigation, IDialogService dialogs)
    {
        _api = api;
        _auth = auth;
        _navigation = navigation;
        _dialogs = dialogs;
        Title = "Paramètres";
    }

    public ObservableCollection<IntervalItem> Intervals { get; } = [];

    public override Task OnAppearingAsync() => RunAsync(async () =>
    {
        var intervals = await _api.GetIntervalsAsync();
        Intervals.Clear();
        foreach (var interval in intervals.OrderBy(i => i.DonationType).ThenBy(i => i.Sex))
        {
            Intervals.Add(new IntervalItem { Sex = interval.Sex, DonationType = interval.DonationType, Days = interval.MinDays.ToString() });
        }
    });

    [RelayCommand]
    private async Task SaveAsync()
    {
        var list = new List<DonationIntervalDto>();
        foreach (var row in Intervals)
        {
            if (!int.TryParse(row.Days, out var days) || days <= 0 || days > 3650)
            {
                ErrorMessage = $"Délai invalide pour « {row.Label} » : saisissez un nombre de jours entre 1 et 3650.";
                return;
            }

            list.Add(new DonationIntervalDto(row.Sex, row.DonationType, days));
        }

        await RunAsync(async () =>
        {
            await _api.UpdateIntervalsAsync(list);
            await _dialogs.ToastAsync("Délais enregistrés");
        });
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _auth.LogoutAsync();
        await _navigation.ShowRoleShellAsync(null);
    }
}
