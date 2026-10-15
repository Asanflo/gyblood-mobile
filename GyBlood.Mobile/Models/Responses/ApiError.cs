namespace GyBlood.Mobile.Models.Responses;

public sealed record ApiError(
    string Code,
    string Message,
    Dictionary<string, string>? Details);
