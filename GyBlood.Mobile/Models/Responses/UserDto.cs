namespace GyBlood.Mobile.Models.Responses;

public sealed record UserDto(
    EntityId Id,
    string Email,
    UserRole Role,
    AccountStatus Status,
    string? DisplayName,
    string? RejectionReason);
