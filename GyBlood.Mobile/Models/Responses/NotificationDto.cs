namespace GyBlood.Mobile.Models.Responses;

public sealed record NotificationDto(
    EntityId Id,
    string Title,
    string Message,
    bool Read,
    DateTimeOffset CreatedAt,
    EntityId? AppointmentId);
