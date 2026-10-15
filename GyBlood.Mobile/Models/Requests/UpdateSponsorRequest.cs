namespace GyBlood.Mobile.Models.Requests;

public sealed record UpdateSponsorRequest(
    string Organisation,
    string Contact,
    string? LogoUrl);
