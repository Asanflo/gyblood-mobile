namespace GyBlood.Mobile.Models.Responses;

public sealed record DonorProfileDto(
    EntityId Id,
    string FullName,
    Sex? Sex,
    string MedicalHistory,
    string? Phone,
    BloodGroup? BloodGroup,
    Rhesus? Rhesus,
    string? DocumentUrl,
    AccountStatus Status,
    bool IsAvailable,
    bool ShareLocation,
    DateTimeOffset? LastDonationAt);
