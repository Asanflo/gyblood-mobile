namespace GyBlood.Mobile.Models.Responses;

public sealed record EligibilityDto(
    bool CanDonate,
    DateTimeOffset? NextDonationDate);
