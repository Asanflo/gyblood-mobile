using Mapsui;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling;
using Mapsui.UI.Maui;
using MBrush = Mapsui.Styles.Brush;
using MColor = Mapsui.Styles.Color;
using MFont = Mapsui.Styles.Font;

namespace GyBlood.Mobile.Views.Controls;

/// <summary>Donneur à afficher sur la carte.</summary>
public sealed record MapDonor(long Id, double Latitude, double Longitude, string Label);

/// <summary>
/// Carte OpenStreetMap (Mapsui 4.1) : fond de carte, position du patient (point bleu),
/// donneurs (points rouges avec leur groupe sanguin) et événement au toucher.
/// </summary>
public sealed class DonorMapView : ContentView
{
    private const string UserAgent = "GyBlood/1.0 (contact: votre-email@exemple.com)";

    private readonly MapControl _mapControl = new();
    private readonly MemoryLayer _donorLayer = new() { Name = "Donneurs", IsMapInfoLayer = true };
    private readonly MemoryLayer _meLayer = new() { Name = "Ma position" };

    public DonorMapView()
    {
        _mapControl.Map.Layers.Add(OpenStreetMap.CreateTileLayer(UserAgent));
        _mapControl.Map.Layers.Add(_donorLayer);
        _mapControl.Map.Layers.Add(_meLayer);
        _mapControl.Info += OnMapInfo;
        Content = _mapControl;
    }

    /// <summary>Levée quand l'utilisateur touche un donneur (identifiant du donneur).</summary>
    public event EventHandler<long>? DonorTapped;

    /// <summary>Centre la carte sur une position. metersPerPixel : 20 donne environ une vue de quartier.</summary>
    public void CenterOn(double latitude, double longitude, double metersPerPixel = 20)
    {
        var (x, y) = SphericalMercator.FromLonLat(longitude, latitude);
        _mapControl.Map.Navigator.CenterOnAndZoomTo(new MPoint(x, y), metersPerPixel);
    }

    public void ZoomIn() => _mapControl.Map.Navigator.ZoomIn();

    public void ZoomOut() => _mapControl.Map.Navigator.ZoomOut();

    /// <summary>Affiche la position du patient (point bleu).</summary>
    public void SetMyPosition(double latitude, double longitude)
    {
        var (x, y) = SphericalMercator.FromLonLat(longitude, latitude);
        var feature = new PointFeature(x, y);
        feature.Styles.Add(new SymbolStyle
        {
            SymbolType = SymbolType.Ellipse,
            SymbolScale = 0.6,
            Fill = new MBrush(MColor.FromString("#1E88E5")),
            Outline = new Pen(MColor.White, 3),
        });

        _meLayer.Features = new List<IFeature> { feature };
        _meLayer.DataHasChanged();
        _mapControl.Refresh();
    }

    /// <summary>Remplace les marqueurs des donneurs.</summary>
    public void SetDonors(IEnumerable<MapDonor> donors)
    {
        var features = new List<IFeature>();
        foreach (var donor in donors)
        {
            var (x, y) = SphericalMercator.FromLonLat(donor.Longitude, donor.Latitude);
            var feature = new PointFeature(x, y);
            feature["id"] = donor.Id;
            feature.Styles.Add(new SymbolStyle
            {
                SymbolType = SymbolType.Ellipse,
                SymbolScale = 0.8,
                Fill = new MBrush(MColor.FromString("#D50000")),
                Outline = new Pen(MColor.White, 3),
            });
            feature.Styles.Add(new LabelStyle
            {
                Text = donor.Label,
                ForeColor = MColor.White,
                BackColor = new MBrush(MColor.FromString("#D50000")),
                Offset = new Offset(0, -26),
                Font = new MFont { Size = 12 },
            });
            features.Add(feature);
        }

        _donorLayer.Features = features;
        _donorLayer.DataHasChanged();
        _mapControl.Refresh();
    }

    private void OnMapInfo(object? sender, MapInfoEventArgs e)
    {
        if (e.MapInfo?.Feature?["id"] is long id)
        {
            DonorTapped?.Invoke(this, id);
        }
    }
}