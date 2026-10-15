namespace GyBlood.Mobile.Models.Responses;

public sealed record DonorDto(
    EntityId Id,
    string FullName,
    BloodGroup BloodGroup,
    Rhesus Rhesus,
    double Latitude,
    double Longitude,
    double DistanceKm);
