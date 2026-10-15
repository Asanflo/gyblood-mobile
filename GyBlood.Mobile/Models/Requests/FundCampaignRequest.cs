namespace GyBlood.Mobile.Models.Requests;

public sealed record FundCampaignRequest(
    decimal Amount,
    PaymentOperator Operator,
    string PhoneNumber);
