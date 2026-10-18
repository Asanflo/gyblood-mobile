using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Core.Helpers;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Auth;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Structure;

/// <summary>Accueil de la structure (maquette : nom, tuiles Stocks / Événements, total de poches, grille des stocks).</summary>
public partial class StructureDashboardViewModel : BaseViewModel
{
    private readonly IStructureApi _api;
    private readonly IAuthService _auth;
    private readonly INavigationService _navigation;

    [ObservableProperty]
    private string structureName = string.Empty;

    [ObservableProperty]
    private string totalText = "0 poches";

    [ObservableProperty]
    private string updatedText = string.Empty;

    [ObservableProperty]
    private bool profileIncomplete;

    public StructureDashboardViewModel(IStructureApi api, IAuthService auth, INavigationService navigation)
    {
        _api = api;
        _auth = auth;
        _navigation = navigation;
        Title = "Accueil";
    }

    public ObservableCollection<StockCell> Cells { get; } = [];

    public override Task OnAppearingAsync() => LoadAsync();

    [RelayCommand]
    private Task LoadAsync() => RunAsync(async () =>
    {
        var me = await _api.GetMeAsync();
        StructureName = me.Name;
        ProfileIncomplete = string.IsNullOrWhiteSpace(me.Name) || string.IsNullOrWhiteSpace(me.Address);

        var all = await _api.GetStocksAsync();
        var mine = all.Where(s => s.StructureId == me.Id).ToList();

        Cells.Clear();
        foreach (var group in Enum.GetValues<BloodGroup>())
        {
            foreach (var rhesus in Enum.GetValues<Rhesus>())
            {
                var quantity = mine.FirstOrDefault(s => s.BloodGroup == group && s.Rhesus == rhesus)?.Quantity ?? 0;
                Cells.Add(new StockCell { BloodText = BloodCompatibility.Format(group, rhesus), Quantity = quantity });
            }
        }

        var total = Cells.Sum(c => c.Quantity);
        TotalText = total switch
        {
            0 or 1 => $"{total} poche",
            _ => $"{total} poches",
        };
        UpdatedText = mine.Count == 0 ? "Stocks non renseignés" : $"Mis à jour le {Ui.Day(mine.Max(s => s.UpdatedAt))}";
    });

    [RelayCommand]
    private Task OpenStocksAsync() => _navigation.GoToAsync(AppRoutes.StockEditor);

    [RelayCommand]
    private Task OpenEventsAsync() => _navigation.GoToAsync(AppRoutes.Tab(AppRoutes.StructureCampaigns));

    [RelayCommand]
    private Task OpenAppointmentsAsync() => _navigation.GoToAsync(AppRoutes.Tab(AppRoutes.StructureAppointments));

    [RelayCommand]
    private Task OpenNotificationsAsync() => _navigation.GoToAsync(AppRoutes.Notifications);

    [RelayCommand]
    private Task OpenProfileAsync() => _navigation.GoToAsync(AppRoutes.Tab(AppRoutes.StructureProfile));
}
