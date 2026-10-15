using System.Net.Mail;
using System.Text.RegularExpressions;

namespace GyBlood.Mobile.Core.Helpers;

/// <summary>Contrôles de saisie partagés par les ViewModels et les comportements (Behaviors). Code pur, testable.</summary>
public static partial class InputValidator
{
    public const int MinPasswordLength = 8;

    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            var address = new MailAddress(email.Trim());
            return address.Address == email.Trim() && address.Host.Contains('.');
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public static bool IsValidPassword(string? password)
        => !string.IsNullOrEmpty(password) && password.Length >= MinPasswordLength;

    /// <summary>Numéro camerounais : 9 chiffres commençant par 6 (mobile) ou 2 (fixe), avec ou sans +237.</summary>
    public static bool IsValidPhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            return false;
        }

        var compact = phone.Replace(" ", string.Empty).Replace("-", string.Empty);
        return CameroonPhone().IsMatch(compact);
    }

    [GeneratedRegex(@"^(\+?237)?[26]\d{8}$")]
    private static partial Regex CameroonPhone();
}
