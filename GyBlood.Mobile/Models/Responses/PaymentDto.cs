namespace GyBlood.Mobile.Models.Responses;

public sealed record PaymentDto(
    EntityId Id,
    EntityId AppointmentId,
    decimal Amount,
    decimal PlatformFee,
    PaymentOperator Operator,
    string? Reference,
    PaymentStatus Status);
