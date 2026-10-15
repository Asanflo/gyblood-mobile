namespace GyBlood.Mobile.Models.Requests;

public sealed record UpdatePatientRequest(
    BloodGroup BloodGroup,
    Rhesus Rhesus,
    double Latitude,
    double Longitude);
