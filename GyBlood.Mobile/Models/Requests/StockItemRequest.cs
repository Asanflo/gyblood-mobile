namespace GyBlood.Mobile.Models.Requests;

public sealed record StockItemRequest(
    BloodGroup BloodGroup,
    Rhesus Rhesus,
    int Quantity);
