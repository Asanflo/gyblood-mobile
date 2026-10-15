namespace GyBlood.Mobile.Models.Responses;

public sealed record StockDto(
    EntityId StructureId,
    string StructureName,
    BloodGroup BloodGroup,
    Rhesus Rhesus,
    int Quantity,
    DateTimeOffset UpdatedAt,
    double? DistanceKm);
