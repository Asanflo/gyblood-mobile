namespace GyBlood.Mobile.Models.Responses;

public sealed record AppointmentDto(
    EntityId Id,
    EntityId PatientId,
    string PatientName,
    EntityId DonorId,
    string DonorName,
    EntityId StructureId,
    string StructureName,
    DateTimeOffset ScheduledAt,
    AppointmentStatus Status,
    decimal? HospitalFee,
    DateTimeOffset CreatedAt);
