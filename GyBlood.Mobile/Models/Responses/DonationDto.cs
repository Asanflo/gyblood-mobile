namespace GyBlood.Mobile.Models.Responses;

public sealed record DonationDto(
    EntityId Id,
    DateTimeOffset DonatedAt,
    string StructureName,
    int? VolumeMl,
    EntityId AppointmentId);
