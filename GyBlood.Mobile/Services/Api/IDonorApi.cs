using GyBlood.Mobile.Core.Constants;
using Refit;

namespace GyBlood.Mobile.Services.Api;


public interface IDonorApi
{
    [Get(ApiRoutes.Donors.Me)]
    Task<DonorProfileDto> GetMeAsync();

    [Put(ApiRoutes.Donors.Me)]
    Task<DonorProfileDto> UpdateMeAsync([Body] UpdateDonorRequest request);

    /// <summary>Document de groupe sanguin (PDF ou image, 5 Mo maximum).</summary>
    [Multipart]
    [Post(ApiRoutes.Donors.BloodDocument)]
    Task<DonorProfileDto> UploadBloodDocumentAsync([AliasAs("file")] StreamPart file);

    [Patch(ApiRoutes.Donors.Availability)]
    Task<DonorProfileDto> SetAvailabilityAsync([Body] AvailabilityRequest request);

    [Get(ApiRoutes.Donors.Eligibility)]
    Task<EligibilityDto> GetEligibilityAsync();

    [Get(ApiRoutes.Donors.Donations)]
    Task<PageResponse<DonationDto>> GetDonationsAsync(int page = 0, int size = 20);

    [Get(ApiRoutes.Donors.Campaigns)]
    Task<PageResponse<CampaignDto>> GetCampaignsAsync(int page = 0, int size = 20);
}
