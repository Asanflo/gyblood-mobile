using System.Globalization;

namespace GyBlood.Mobile.Core.Helpers;

public static class DateHelper
{
    private static readonly CultureInfo French = CultureInfo.GetCultureInfo("fr-FR");

    public static string ToDisplay(DateTimeOffset value)
        => value.ToLocalTime().ToString("dd MMM yyyy HH:mm", French);

    public static string ToDisplayDate(DateTimeOffset value)
        => value.ToLocalTime().ToString("dd MMM yyyy", French);

    /// <summary>Texte du prochain don possible (RG5) : « Vous pouvez donner », « Dans 12 jours »...</summary>
    public static string NextDonationText(DateTimeOffset? next, DateTimeOffset? now = null)
    {
        var current = now ?? DateTimeOffset.UtcNow;
        if (next is null || next <= current)
        {
            return "Vous pouvez donner dès maintenant";
        }

        var days = (int)Math.Ceiling((next.Value - current).TotalDays);
        return days <= 1 ? "Dès demain" : $"Dans {days} jours ({ToDisplayDate(next.Value)})";
    }
}
