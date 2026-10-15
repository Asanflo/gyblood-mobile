namespace GyBlood.Mobile.Models.Requests;

public sealed record AvailabilityRequest(
    bool IsAvailable,
    bool ShareLocation);
