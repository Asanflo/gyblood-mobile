using Android.App;
using Android.Content;
using Android.Content.PM;

namespace GyBlood.Mobile;

/// <summary>
/// Reçoit la redirection gyblood://auth?... à la fin de la connexion Google (voir AppConstants.OAuthCallbackUrl).
/// </summary>
[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataScheme = CallbackScheme)]
public class WebAuthenticationCallbackActivity : Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity
{
    private const string CallbackScheme = "gyblood";
}
