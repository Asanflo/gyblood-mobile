using GyBlood.Mobile.Core.Constants;
using Refit;

namespace GyBlood.Mobile.Services.Api;


public interface ICampaignApi
{
    [Post(ApiRoutes.Campaigns.Root)]
    Task<CampaignDto> CreateAsync([Body] CreateCampaignRequest request);

    [Get(ApiRoutes.Campaigns.Root)]
    Task<PageResponse<CampaignDto>> GetCampaignsAsync(int page = 0, int size = 20);

    [Get(ApiRoutes.Campaigns.ById)]
    Task<CampaignDto> GetAsync(EntityId id);

    [Post(ApiRoutes.Campaigns.Join)]
    Task JoinAsync(EntityId id);

    [Delete(ApiRoutes.Campaigns.Join)]
    Task LeaveAsync(EntityId id);

    [Multipart]
    [Post(ApiRoutes.Campaigns.Image)]
    Task<CampaignDto> UploadImageAsync(EntityId id, [AliasAs("file")] StreamPart file);
}
