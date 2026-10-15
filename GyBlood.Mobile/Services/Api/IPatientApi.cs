using GyBlood.Mobile.Core.Constants;
using Refit;

namespace GyBlood.Mobile.Services.Api;


public interface IPatientApi
{
    [Get(ApiRoutes.Patients.Me)]
    Task<PatientDto> GetMeAsync();

    [Put(ApiRoutes.Patients.Me)]
    Task<PatientDto> UpdateMeAsync([Body] UpdatePatientRequest request);

    /// <summary>Donneurs compatibles, validés, connectés et hors délai de repos (RG3, RG4, RG5).</summary>
    [Get(ApiRoutes.Patients.SearchDonors)]
    Task<List<DonorDto>> SearchDonorsAsync(BloodGroup bloodGroup, Rhesus rhesus, double lat, double lon, double radiusKm);

    [Get(ApiRoutes.Patients.NearestStructures)]
    Task<List<StructureDto>> GetNearestStructuresAsync(double lat, double lon, int limit = 5);
}
