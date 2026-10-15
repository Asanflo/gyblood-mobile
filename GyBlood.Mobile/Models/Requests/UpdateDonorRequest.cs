namespace GyBlood.Mobile.Models.Requests;

public sealed record UpdateDonorRequest(
    string FullName,
    Sex Sex,
    string MedicalHistory,
    string? Phone,
    BloodGroup BloodGroup,
    Rhesus Rhesus);
