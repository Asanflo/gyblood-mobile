namespace GyBlood.Mobile.Models.Responses;

public sealed record FundingDto(
    EntityId Id,
    EntityId CampaignId,
    string CampaignTitle,
    decimal Amount,
    DateTimeOffset CreatedAt,
    PaymentStatus Status);
