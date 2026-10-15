namespace GyBlood.Mobile.Models.Requests;

public sealed record UpdateStructureRequest(
    string Name,
    string Address,
    double Latitude,
    double Longitude,
    string Phone);
