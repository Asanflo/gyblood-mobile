namespace GyBlood.Mobile.Models.Responses;

public sealed record StructureDto(
    EntityId Id,
    string Name,
    string Address,
    double Latitude,
    double Longitude,
    string Phone,
    AccountStatus Status,
    double? DistanceKm);
