namespace GyBlood.Mobile.Models.Requests;

public sealed record CreateCampaignRequest(
    string Title,
    string Location,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    int TargetDonations,
    string? Description);
