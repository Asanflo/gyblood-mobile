namespace GyBlood.Mobile.Core.Extensions;

public static class ResourceExtensions
{
    /// <summary>Couleur définie dans Colors.xaml, avec une valeur de repli.</summary>
    public static Color GetColor(this Application? application, string key, Color fallback)
        => application?.Resources.TryGetValue(key, out var value) == true && value is Color color ? color : fallback;
}
