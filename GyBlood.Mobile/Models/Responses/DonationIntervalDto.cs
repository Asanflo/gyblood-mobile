namespace GyBlood.Mobile.Models.Responses;

public sealed record DonationIntervalDto(
    Sex Sex,
    DonationType DonationType,
    int MinDays);
