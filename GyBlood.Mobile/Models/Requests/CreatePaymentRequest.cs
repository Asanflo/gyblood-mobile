namespace GyBlood.Mobile.Models.Requests;

public sealed record CreatePaymentRequest(
    PaymentOperator Operator,
    string PhoneNumber);
