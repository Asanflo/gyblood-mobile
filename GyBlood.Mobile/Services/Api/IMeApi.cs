using GyBlood.Mobile.Core.Constants;
using Refit;

namespace GyBlood.Mobile.Services.Api;


/// <summary>Compte connecté : session, appareil (FCM) et signal de présence.</summary>
public interface IMeApi
{
    [Get(ApiRoutes.Me.Root)]
    Task<UserDto> GetMeAsync();

    [Post(ApiRoutes.Me.Devices)]
    Task RegisterDeviceAsync([Body] RegisterDeviceRequest request);

    [Post(ApiRoutes.Me.Heartbeat)]
    Task SendHeartbeatAsync([Body] HeartbeatRequest request);

    [Post(ApiRoutes.Auth.Logout)]
    Task LogoutAsync([Body] RefreshRequest request);
}
