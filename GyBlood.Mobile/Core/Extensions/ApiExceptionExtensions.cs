using System.Net;
using System.Text.Json;
using Refit;

namespace GyBlood.Mobile.Core.Extensions;

public static class ApiExceptionExtensions
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    /// <summary>Message lisible par l'utilisateur à partir d'une erreur HTTP du backend.</summary>
    public static string ToUserMessage(this ApiException exception)
    {
        if (TryReadMessage(exception.Content) is { } serverMessage)
        {
            return serverMessage;
        }

        return exception.StatusCode switch
        {
            HttpStatusCode.Unauthorized => "Identifiants incorrects ou session expirée.",
            HttpStatusCode.Forbidden => "Action non autorisée, ou compte pas encore validé.",
            HttpStatusCode.NotFound => "Élément introuvable.",
            HttpStatusCode.Conflict => "Cet élément existe déjà.",
            HttpStatusCode.BadRequest or HttpStatusCode.UnprocessableEntity => "Les informations saisies sont invalides.",
            _ => "Une erreur est survenue. Réessayez dans un instant.",
        };
    }

    private static string? TryReadMessage(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return null;
        }

        try
        {
            var error = JsonSerializer.Deserialize<ApiError>(content, Json);
            return string.IsNullOrWhiteSpace(error?.Message) ? null : error.Message;
        }
        catch (JsonException)
        {
            return null;
        }
    }
}
