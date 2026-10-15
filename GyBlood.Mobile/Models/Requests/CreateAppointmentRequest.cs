namespace GyBlood.Mobile.Models.Requests;

public sealed record CreateAppointmentRequest(
    EntityId DonorId,
    EntityId StructureId,
    DateTimeOffset ScheduledAt);
