using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GyBlood.Mobile.Core.Helpers;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Location;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Patient;

/// <summary>Stocks de sang des structures, filtrés par groupe et rhésus, du plus proche au plus éloigné. (US-10, US-11)</summary>
public partial class StocksViewModel : BaseViewModel
{
    private const string All = "Tous";

    private readonly IStructureApi _api;
    private readonly ILocationService _location;
    private Dictionary<EntityId, string> _phones = [];

    [ObservableProperty]
    private string selectedGroup = All;

    [ObservableProperty]
    private string selectedRhesus = All;

    [ObservableProperty]
    private string countText = string.Empty;

    [ObservableProperty]
    private bool isEmpty;

    public StocksViewModel(IStructureApi api, ILocationService location)
    {
        _api = api;
        _location = location;
        Title = "Stocks de sang";
    }

    public ObservableCollection<StockItem> Items { get; } = [];

    public IReadOnlyList<string> GroupChoices { get; } = [All, .. Ui.BloodGroupLabels];

    public IReadOnlyList<string> RhesusChoices { get; } = [All, .. Ui.RhesusLabels];

    public override Task OnAppearingAsync() => LoadAsync();

    partial void OnSelectedGroupChanged(string value) => _ = LoadAsync();

    partial void OnSelectedRhesusChanged(string value) => _ = LoadAsync();

    [RelayCommand]
    private Task LoadAsync() => RunAsync(async () =>
    {
        var point = await _location.GetCurrentAsync();
        var stocks = await _api.GetStocksAsync(
            Ui.ParseGroup(SelectedGroup),
            Ui.ParseRhesus(SelectedRhesus),
            point?.Latitude,
            point?.Longitude);

        if (_phones.Count == 0)
        {
            var structures = await _api.GetStructuresAsync();
            _phones = structures.ToDictionary(s => s.Id, s => s.Phone);
        }

        Items.Clear();
        foreach (var stock in stocks)
        {
            Items.Add(Build(stock));
        }

        IsEmpty = Items.Count == 0;
        var centers = stocks.Select(s => s.StructureId).Distinct().Count();
        CountText = centers switch
        {
            0 => "Aucune structure",
            1 => "1 structure",
            _ => $"{centers} structures",
        };
    });

    private StockItem Build(StockDto stock)
    {
        var (text, color, background) = Level(stock.Quantity);
        return new StockItem
        {
            StructureName = stock.StructureName,
            BloodText = BloodCompatibility.Format(stock.BloodGroup, stock.Rhesus),
            QuantityText = stock.Quantity <= 1 ? $"{stock.Quantity} poche" : $"{stock.Quantity} poches",
            DistanceText = Ui.Distance(stock.DistanceKm),
            UpdatedText = $"Mis à jour le {Ui.Day(stock.UpdatedAt)}",
            LevelText = text,
            LevelColor = color,
            LevelBackground = background,
            ContactCommand = new AsyncRelayCommand(() => ContactAsync(stock.StructureId)),
        };
    }

    /// <summary>Seuils d'affichage : à ajuster avec les structures (le serveur ne renvoie que la quantité).</summary>
    private static (string Text, Color Color, Color Background) Level(int quantity) => quantity switch
    {
        >= 8 => ("Disponible", Ui.Tone("Success", Colors.Green), Ui.Tone("SuccessLight", Colors.LightGreen)),
        >= 3 => ("Stock faible", Ui.Tone("Warning", Colors.Orange), Ui.Tone("WarningLight", Colors.Moccasin)),
        >= 1 => ("Critique", Ui.Tone("Danger", Colors.Red), Ui.Tone("DangerLight", Colors.MistyRose)),
        _ => ("Rupture", Ui.Tone("Danger", Colors.Red), Ui.Tone("DangerLight", Colors.MistyRose)),
    };

    private async Task ContactAsync(EntityId structureId)
    {
        if (_phones.TryGetValue(structureId, out var phone) && !string.IsNullOrWhiteSpace(phone))
        {
            await Launcher.Default.OpenAsync(new Uri($"tel:{phone}"));
        }
        else
        {
            ErrorMessage = "Numéro de la structure indisponible.";
        }
    }
}
