using System.Globalization;
using GyBlood.Mobile.Core.Extensions;

namespace GyBlood.Mobile.Converters;

/// <summary>Couleur d'un statut (rendez-vous, compte, paiement). Utilisable avec ConverterParameter vide.</summary>
public sealed class StatusToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var app = Application.Current;
        return value switch
        {
            AppointmentStatus.Requested or AccountStatus.Pending or PaymentStatus.Pending => app.GetColor("Warning", Colors.Orange),
            AppointmentStatus.Accepted => app.GetColor("Info", Colors.Blue),
            AppointmentStatus.Paid or AppointmentStatus.Completed or AccountStatus.Validated or PaymentStatus.Succeeded
                => app.GetColor("Success", Colors.Green),
            AppointmentStatus.Cancelled or AppointmentStatus.NoShow or AccountStatus.Rejected or AccountStatus.Suspended or PaymentStatus.Failed
                => app.GetColor("Danger", Colors.Red),
            _ => app.GetColor("Gray500", Colors.Gray),
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
