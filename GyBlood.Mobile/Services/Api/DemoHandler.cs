using System.Net;
using System.Text;

namespace GyBlood.Mobile.Services.Api;

/// <summary>
/// MODE DÉMO : répond à la place de l'API pour tester les écrans sans serveur.
/// Le rôle choisi à l'inscription (ou l'email à la connexion) détermine l'espace affiché.
/// </summary>
public sealed class DemoHandler : HttpMessageHandler
{
    private const string EmptyPage = "{\"content\":[],\"totalElements\":0,\"totalPages\":0,\"number\":0,\"size\":20}";

    private static string _role
    {
        get => Preferences.Default.Get("demo_role", "PATIENT");
        set => Preferences.Default.Set("demo_role", value);
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var path = request.RequestUri!.AbsolutePath.TrimEnd('/').ToLowerInvariant();
        var method = request.Method.Method;

        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(Build(request, path, method), Encoding.UTF8, "application/json"),
        });
    }

    private static string Build(HttpRequestMessage request, string path, string method)
    {
        if (path.EndsWith("/auth/login") || path.EndsWith("/auth/register")
            || path.EndsWith("/auth/google") || path.EndsWith("/auth/refresh"))
        {
            return AuthJson(request);
        }

        if (method != "GET")
        {
            return "{}";
        }

        // Profils
        if (path.EndsWith("/donors/me"))
        {
            return "{\"id\":1,\"fullName\":\"Donneur démo\",\"sex\":\"MALE\",\"medicalHistory\":\"\",\"phone\":\"670000000\","
                 + "\"bloodGroup\":\"O\",\"rhesus\":\"POSITIVE\",\"documentUrl\":null,\"status\":\"VALIDATED\","
                 + "\"isAvailable\":true,\"shareLocation\":true,\"lastDonationAt\":null}";
        }

        if (path.EndsWith("/patients/me"))
        {
            return "{\"id\":1,\"bloodGroup\":null,\"rhesus\":null,\"latitude\":null,\"longitude\":null}";
        }

        if (path.EndsWith("/structures/me"))
        {
            return "{\"id\":1,\"name\":\"Hôpital démo\",\"address\":\"Douala, Akwa\",\"latitude\":4.05,\"longitude\":9.7,"
                 + "\"phone\":\"670000000\",\"status\":\"VALIDATED\",\"donationFee\":5000,\"distanceKm\":null}";
        }

        if (path.EndsWith("/sponsors/me"))
        {
            return "{\"id\":1,\"organisation\":\"Sponsor démo\",\"contact\":\"670000000\",\"logoUrl\":null}";
        }

        if (path.EndsWith("/donors/me/eligibility"))
        {
            return "{\"canDonate\":true,\"nextDonationDate\":null}";
        }

        if (path.EndsWith("/me"))
        {
            return UserJson();
        }

        // Listes paginées (objet { content, totalElements, ... })
        if (path.EndsWith("/appointments") || path.EndsWith("/notifications") || path.EndsWith("/donations")
            || path.EndsWith("/campaigns") || path.EndsWith("/fundings") || path.EndsWith("/accounts"))
        {
            return EmptyPage;
        }

        // Listes simples (tableau)
        if (path.EndsWith("/donors/search") || path.EndsWith("/structures") || path.EndsWith("/structures/nearest")
            || path.EndsWith("/stocks") || path.EndsWith("/donation-intervals"))
        {
            return "[]";
        }

        return "{}";
    }

    private static string AuthJson(HttpRequestMessage request)
    {
        var body = request.Content is null ? string.Empty : request.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        var text = body.ToUpperInvariant();

        // À l'inscription, le rôle est dans le corps. À la connexion, on devine avec l'email :
        // donneur@..., structure@..., sponsor@..., admin@... (sinon patient)
        _role = text.Contains("\"ROLE\":\"DONOR\"") || text.Contains("DONNEUR") ? "DONOR"
              : text.Contains("\"ROLE\":\"STRUCTURE\"") || text.Contains("STRUCTURE@") ? "STRUCTURE"
              : text.Contains("\"ROLE\":\"SPONSOR\"") || text.Contains("SPONSOR@") ? "SPONSOR"
              : text.Contains("ADMIN") ? "ADMIN"
              : text.Contains("\"ROLE\":\"PATIENT\"") ? "PATIENT"
              : _role;

        return "{\"accessToken\":\"demo\",\"refreshToken\":\"demo\",\"user\":" + UserJson() + "}";
    }

    private static string UserJson()
        => "{\"id\":1,\"email\":\"demo@gyblood.cm\","
         + $"\"role\":\"{_role}\",\"status\":\"VALIDATED\",\"displayName\":\"Compte démo\"}}";
}