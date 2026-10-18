namespace GyBlood.Mobile.Models.Requests;

public sealed record RegisterRequest(
    string Email,
    string Password,
    UserRole Role,
    string? DisplayName = null);
