namespace GyBlood.Mobile.Models.Requests;

public sealed record LoginRequest(
    string Email,
    string Password);
