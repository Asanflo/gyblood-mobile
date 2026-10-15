using GyBlood.Mobile.Core.Constants;
using Refit;

namespace GyBlood.Mobile.Services.Api;


public interface IPaymentApi
{
    [Post(ApiRoutes.Appointments.Payments)]
    Task<PaymentDto> PayAppointmentAsync(EntityId id, [Body] CreatePaymentRequest request);

    [Get(ApiRoutes.Payments.ById)]
    Task<PaymentDto> GetPaymentAsync(EntityId id);
}
