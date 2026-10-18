using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Admin;

/// <summary>Comptes à examiner : filtre par statut et par rôle, ouverture de la fiche. (US-36)</summary>
public partial class PendingAccountsViewModel : BaseViewModel
{
    private const string All = "Tous";

    private static readonly Dictionary<string, AccountStatus?> Statuses = new()
    {
        ["En attente"] = AccountStatus.Pending,
        ["Validés"] = AccountStatus.Validated,
        ["Refusés"] = AccountStatus.Rejected,
        ["Suspendus"] = AccountStatus.Suspended,
        [All] = null,
    };

    private static readonly Dictionary<string, UserRole?> Roles = new()
    {
        [All] = null,
        ["Donneurs"] = UserRole.Donor,
        ["Structures"] = UserRole.Structure,
        ["Sponsors"] = UserRole.Sponsor,
        ["Patients"] = UserRole.Patient,
    };

    private readonly IAdminApi _api;
    private readonly INavigationService _navigation;

    [ObservableProperty]
    private string selectedStatus = "En attente";

    [ObservableProperty]
    private string selectedRole = All;

    [ObservableProperty]
    private string countText = string.Empty;

    public PendingAccountsViewModel(IAdminApi api, INavigationService navigation)
    {
        _api = api;
        _navigation = navigation;
        Title = "Comptes";
    }

    public ObservableCollection<AccountItem> Items { get; } = [];

    public IReadOnlyList<string> StatusChoices { get; } = [.. Statuses.Keys];

    public IReadOnlyList<string> RoleChoices { get; } = [.. Roles.Keys];

    public override Task OnAppearingAsync() => LoadAsync();

    partial void OnSelectedStatusChanged(string value) => _ = LoadAsync();

    partial void OnSelectedRoleChanged(string value) => _ = LoadAsync();

    [RelayCommand]
    private Task LoadAsync() => RunAsync(async () =>
    {
        var status = Statuses.GetValueOrDefault(SelectedStatus);
        var role = Roles.GetValueOrDefault(SelectedRole);
        var page = await _api.GetAccountsAsync(status, role, 0, 50);

        Items.Clear();
        foreach (var dto in page.Content)
        {
            Items.Add(new AccountItem
            {
                Dto = dto,
                Name = string.IsNullOrWhiteSpace(dto.DisplayName) ? "Sans nom" : dto.DisplayName,
                Email = dto.Email,
                RoleText = Ui.RoleText(dto.Role),
                DateText = Ui.Day(dto.CreatedAt),
                StatusText = Ui.AccountStatusText(dto.Status),
                StatusColor = Ui.StatusColor(dto.Status),
                OpenCommand = new AsyncRelayCommand(() =>
                    _navigation.GoToAsync(AppRoutes.AccountDetail, new Dictionary<string, object> { ["account"] = dto })),
            });
        }

        CountText = Items.Count switch
        {
            0 => "Aucun compte",
            1 => "1 compte",
            _ => $"{Items.Count} comptes",
        };
    });
}
