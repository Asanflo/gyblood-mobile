using GyBlood.Mobile.Services.Api;
using Refit;

namespace GyBlood.Mobile.Tests.Fakes;

public sealed class FakeDonorApi : IDonorApi
{
    private DonorProfileDto _profile = new(
        Id: 10,
        FullName: "Donneur Démo",
        Sex: Sex.Male,
        MedicalHistory: "Aucun",
        Phone: "697117458",
        BloodGroup: BloodGroup.O,
        Rhesus: Rhesus.Negative,
        DocumentUrl: null,
        Status: AccountStatus.Validated,
        IsAvailable: false,
        ShareLocation: false,
        LastDonationAt: null);

    public Task<DonorProfileDto> GetMeAsync() => Task.FromResult(_profile);

    public Task<DonorProfileDto> UpdateMeAsync(UpdateDonorRequest request)
    {
        _profile = _profile with
        {
            FullName = request.FullName,
            Sex = request.Sex,
            MedicalHistory = request.MedicalHistory,
            Phone = request.Phone,
            BloodGroup = request.BloodGroup,
            Rhesus = request.Rhesus,
        };
        return Task.FromResult(_profile);
    }

    public Task<DonorProfileDto> UploadBloodDocumentAsync(StreamPart file)
    {
        _profile = _profile with { DocumentUrl = "fake://blood-document", Status = AccountStatus.Pending };
        return Task.FromResult(_profile);
    }

    public Task<DonorProfileDto> SetAvailabilityAsync(AvailabilityRequest request)
    {
        _profile = _profile with { IsAvailable = request.IsAvailable, ShareLocation = request.ShareLocation };
        return Task.FromResult(_profile);
    }

    public Task<EligibilityDto> GetEligibilityAsync()
        => Task.FromResult(new EligibilityDto(_profile.LastDonationAt is null, _profile.LastDonationAt?.AddDays(56)));

    public Task<PageResponse<DonationDto>> GetDonationsAsync(int page = 0, int size = 20)
        => Task.FromResult(new PageResponse<DonationDto>([], 0, 0, page, size));

    public Task<PageResponse<CampaignDto>> GetCampaignsAsync(int page = 0, int size = 20)
        => Task.FromResult(new PageResponse<CampaignDto>([], 0, 0, page, size));
}
