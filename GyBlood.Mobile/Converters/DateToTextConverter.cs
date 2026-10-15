using System.Globalization;
using GyBlood.Mobile.Core.Helpers;

namespace GyBlood.Mobile.Converters;

public sealed class DateToTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value switch
    {
        DateTimeOffset offset => DateHelper.ToDisplay(offset),
        DateTime dateTime => DateHelper.ToDisplay(new DateTimeOffset(dateTime)),
        _ => string.Empty,
    };

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
