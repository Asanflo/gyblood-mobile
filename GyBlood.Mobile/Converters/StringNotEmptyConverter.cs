using System.Globalization;

namespace GyBlood.Mobile.Converters;

/// <summary>true si le texte n'est ni nul ni vide : sert à afficher un message d'erreur seulement quand il y en a un.</summary>
public sealed class StringNotEmptyConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is string text && !string.IsNullOrWhiteSpace(text);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
