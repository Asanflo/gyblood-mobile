namespace GyBlood.Mobile.Models.Responses;

public sealed record SponsorDto(
    EntityId Id,
    string Organisation,
    string Contact,
    string? LogoUrl);
