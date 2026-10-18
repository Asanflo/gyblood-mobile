using System.Globalization;
using GyBlood.Mobile.Core.Extensions;

namespace GyBlood.Mobile.ViewModels.Items;

/// <summary>Libellés français et couleurs d'affichage. Un seul endroit à modifier pour changer un texte de statut.</summary>
public static class Ui
{
    private static readonly CultureInfo French = CultureInfo.GetCultureInfo("fr-FR");

    public static readonly IReadOnlyList<string> BloodGroupLabels = ["A", "B", "AB", "O"];
    public static readonly IReadOnlyList<string> RhesusLabels = ["Positif", "Négatif"];
    public static readonly IReadOnlyList<string> SexLabels = ["Masculin", "Féminin"];

    public static BloodGroup? ParseGroup(string? label)
        => label is not null && Enum.TryParse<BloodGroup>(label, out var group) ? group : null;

    public static Rhesus? ParseRhesus(string? label) => label switch
    {
        "Positif" => Rhesus.Positive,
        "Négatif" => Rhesus.Negative,
        _ => null,
    };

    public static Sex? ParseSex(string? label) => label switch
    {
        "Masculin" => Sex.Male,
        "Féminin" => Sex.Female,
        _ => null,
    };

    public static string GroupLabel(BloodGroup? group) => group?.ToString() ?? string.Empty;

    public static string RhesusLabel(Rhesus? rhesus) => rhesus switch
    {
        Rhesus.Positive => "Positif",
        Rhesus.Negative => "Négatif",
        _ => string.Empty,
    };

    public static string SexLabel(Sex? sex) => sex switch
    {
        Sex.Male => "Masculin",
        Sex.Female => "Féminin",
        _ => string.Empty,
    };

    public static string Money(decimal amount) => $"{amount.ToString("N0", French)} FCFA";

    public static string Distance(double? km) => km is null ? string.Empty : $"{km.Value.ToString("0.0", French)} km";

    public static string Day(DateTimeOffset value) => value.ToLocalTime().ToString("dd MMM yyyy", French);

    public static string DayAndHour(DateTimeOffset value) => value.ToLocalTime().ToString("dd MMM yyyy 'à' HH:mm", French);

    public static string RoleText(UserRole role) => role switch
    {
        UserRole.Patient => "Patient",
        UserRole.Donor => "Donneur",
        UserRole.Structure => "Banque de sang / hôpital",
        UserRole.Sponsor => "Sponsor",
        UserRole.Admin => "Administrateur",
        _ => role.ToString(),
    };

    public static string AppointmentStatusText(AppointmentStatus status) => status switch
    {
        AppointmentStatus.Requested => "Demandé",
        AppointmentStatus.Accepted => "Accepté",
        AppointmentStatus.Paid => "Payé",
        AppointmentStatus.Completed => "Effectué",
        AppointmentStatus.Cancelled => "Annulé",
        AppointmentStatus.NoShow => "Absent",
        _ => status.ToString(),
    };

    public static string AccountStatusText(AccountStatus status) => status switch
    {
        AccountStatus.Pending => "En attente",
        AccountStatus.Validated => "Validé",
        AccountStatus.Rejected => "Refusé",
        AccountStatus.Suspended => "Suspendu",
        _ => status.ToString(),
    };

    public static string PaymentStatusText(PaymentStatus status) => status switch
    {
        PaymentStatus.Pending => "En cours",
        PaymentStatus.Succeeded => "Réussi",
        PaymentStatus.Failed => "Échoué",
        PaymentStatus.Refunded => "Remboursé",
        _ => status.ToString(),
    };

    public static string OperatorText(PaymentOperator op) => op switch
    {
        PaymentOperator.MtnMomo => "MTN MoMo",
        PaymentOperator.OrangeMoney => "Orange Money",
        _ => op.ToString(),
    };

    public static Color Tone(string key, Color fallback) => Application.Current.GetColor(key, fallback);

    public static Color StatusColor(AppointmentStatus status) => status switch
    {
        AppointmentStatus.Requested => Tone("Warning", Colors.Orange),
        AppointmentStatus.Accepted => Tone("Info", Colors.Blue),
        AppointmentStatus.Paid or AppointmentStatus.Completed => Tone("Success", Colors.Green),
        _ => Tone("Danger", Colors.Red),
    };

    public static Color StatusColor(AccountStatus status) => status switch
    {
        AccountStatus.Pending => Tone("Warning", Colors.Orange),
        AccountStatus.Validated => Tone("Success", Colors.Green),
        _ => Tone("Danger", Colors.Red),
    };

    public static Color StatusColor(PaymentStatus status) => status switch
    {
        PaymentStatus.Pending => Tone("Warning", Colors.Orange),
        PaymentStatus.Succeeded => Tone("Success", Colors.Green),
        _ => Tone("Danger", Colors.Red),
    };
}
