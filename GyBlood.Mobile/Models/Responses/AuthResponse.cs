namespace GyBlood.Mobile.Models.Responses;

public sealed record AuthResponse(
    string AccessToken,
    string RefreshToken,
    UserDto User);
