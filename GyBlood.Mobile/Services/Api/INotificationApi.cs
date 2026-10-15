using GyBlood.Mobile.Core.Constants;
using Refit;

namespace GyBlood.Mobile.Services.Api;


public interface INotificationApi
{
    [Get(ApiRoutes.Notifications.Root)]
    Task<PageResponse<NotificationDto>> GetNotificationsAsync(int page = 0, int size = 20);

    [Patch(ApiRoutes.Notifications.Read)]
    Task MarkReadAsync(EntityId id);
}
