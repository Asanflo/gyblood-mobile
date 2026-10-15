using Mapsui;
using Mapsui.Projections;
using Mapsui.Tiling;
using Mapsui.UI.Maui;

namespace GyBlood.Mobile.Views.Controls;

/// <summary>
/// Carte OpenStreetMap (Mapsui). Version minimale : fond de carte et centrage.
/// À FAIRE (semaine 3) : marqueurs des donneurs (nom, groupe, rhésus), position du patient et des structures,
/// fiche en bas d'écran au toucher. Les noms exacts de l'API de marqueurs dépendent de la version de Mapsui installée.
/// </summary>
public sealed class DonorMapView : ContentView
{
    private readonly MapControl _mapControl = new();

    public DonorMapView()
    {
        _mapControl.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
        Content = _mapControl;
    }

    /// <summary>Centre la carte sur une position. metersPerPixel : 20 donne environ une vue de quartier.</summary>
    public void CenterOn(double latitude, double longitude, double metersPerPixel = 20)
    {
        var (x, y) = SphericalMercator.FromLonLat(longitude, latitude);
        _mapControl.Map.Navigator.CenterOnAndZoomTo(new MPoint(x, y), metersPerPixel);
    }
}
