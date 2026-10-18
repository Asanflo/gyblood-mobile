using GyBlood.Mobile.Core.Constants;
using Refit;

namespace GyBlood.Mobile.Services.Api;


public interface ISponsorApi
{
    [Put(ApiRoutes.Sponsors.Me)]
    Task<SponsorDto> UpdateMeAsync([Body] UpdateSponsorRequest request);

    [Post(ApiRoutes.Campaigns.Fundings)]
    Task<FundingDto> FundCampaignAsync(EntityId id, [Body] FundCampaignRequest request);

    [Get(ApiRoutes.Sponsors.Fundings)]
    Task<PageResponse<FundingDto>> GetFundingsAsync(int page = 0, int size = 20);

    [Get(ApiRoutes.Sponsors.Me)]
    Task<SponsorDto> GetMeAsync();
}
