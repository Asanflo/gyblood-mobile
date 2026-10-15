namespace GyBlood.Mobile.Models.Responses;

public sealed record PatientDto(
    EntityId Id,
    BloodGroup? BloodGroup,
    Rhesus? Rhesus,
    double? Latitude,
    double? Longitude);
