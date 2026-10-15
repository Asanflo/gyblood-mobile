using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Refit;

namespace GyBlood.Mobile.Services.Api;

/// <summary>
/// Contrat JSON avec le backend : propriétés en camelCase, énumérations en MAJUSCULES_AVEC_TIRET_BAS
/// (Requested -> REQUESTED, NoShow -> NO_SHOW), aussi bien dans le corps que dans les paramètres d'URL.
/// </summary>
public static class RefitSettingsFactory
{
    public static RefitSettings Create()
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper));

        return new RefitSettings(new SystemTextJsonContentSerializer(options))
        {
            UrlParameterFormatter = new EnumUrlParameterFormatter(),
        };
    }

    private sealed class EnumUrlParameterFormatter : DefaultUrlParameterFormatter
    {
        public override string? Format(object? parameterValue, ICustomAttributeProvider attributeProvider, Type type)
        {
            if (parameterValue is Enum value)
            {
                return JsonNamingPolicy.SnakeCaseUpper.ConvertName(value.ToString());
            }

            return base.Format(parameterValue, attributeProvider, type);
        }
    }
}
