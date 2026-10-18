using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Auth;
using GyBlood.Mobile.Services.Dialogs;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Structure;

/// <summary>Mise à jour des stocks : une ligne par groupe et rhésus, boutons + / −. (US-25)</summary>
public partial class StockEditorViewModel : BaseViewModel
{
    private readonly IStructureApi _api;
    private readonly IAuthService _auth;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogs;

    public StockEditorViewModel(IStructureApi api, IAuthService auth, INavigationService navigation, IDialogService dialogs)
    {
        _api = api;
        _auth = auth;
        _navigation = navigation;
        _dialogs = dialogs;
        Title = "Mes stocks";
    }

    public ObservableCollection<StockEditRow> Rows { get; } = [];

    public override Task OnAppearingAsync() => RunAsync(async () =>
    {
        var me = await _api.GetMeAsync();
        var mine = (await _api.GetStocksAsync()).Where(s => s.StructureId == me.Id).ToList();

        Rows.Clear();
        foreach (var group in Enum.GetValues<BloodGroup>())
        {
            foreach (var rhesus in Enum.GetValues<Rhesus>())
            {
                Rows.Add(new StockEditRow
                {
                    Group = group,
                    Rhesus = rhesus,
                    Quantity = mine.FirstOrDefault(s => s.BloodGroup == group && s.Rhesus == rhesus)?.Quantity ?? 0,
                });
            }
        }
    });

    [RelayCommand]
    private Task SaveAsync() => RunAsync(async () =>
    {
        var items = Rows.Select(r => new StockItemRequest(r.Group, r.Rhesus, r.Quantity)).ToList();
        await _api.UpdateMyStocksAsync(new UpdateStocksRequest(items));
        await _dialogs.ToastAsync("Stocks mis à jour");
        await _navigation.GoBackAsync();
    });
}
