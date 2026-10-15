using GyBlood.Mobile.Core.Constants;
using Refit;

namespace GyBlood.Mobile.Services.Api;


public interface IAdminApi
{
    [Get(ApiRoutes.Admin.Accounts)]
    Task<PageResponse<AccountSummaryDto>> GetAccountsAsync(AccountStatus? status = null, UserRole? role = null, int page = 0, int size = 20);

    [Get(ApiRoutes.Admin.DonorBloodDocument)]
    Task<Stream> GetDonorBloodDocumentAsync(EntityId id);

    [Post(ApiRoutes.Admin.Approve)]
    Task ApproveAsync(EntityId id);

    [Post(ApiRoutes.Admin.Reject)]
    Task RejectAsync(EntityId id, [Body] RejectAccountRequest request);

    [Post(ApiRoutes.Admin.Suspend)]
    Task SuspendAsync(EntityId id);

    [Post(ApiRoutes.Admin.Reactivate)]
    Task ReactivateAsync(EntityId id);

    [Get(ApiRoutes.Admin.Intervals)]
    Task<List<DonationIntervalDto>> GetIntervalsAsync();

    [Put(ApiRoutes.Admin.Intervals)]
    Task<List<DonationIntervalDto>> UpdateIntervalsAsync([Body] List<DonationIntervalDto> intervals);
}
