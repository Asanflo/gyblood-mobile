namespace GyBlood.Mobile.Models.Responses;

public sealed record AccountSummaryDto(
    EntityId Id,
    string Email,
    UserRole Role,
    AccountStatus Status,
    string? DisplayName,
    DateTimeOffset CreatedAt);
